using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    private Enemy target;
    private float damage;
    public float speed = 7f;

    private void OnEnable()
    {
        // якщо на стр≥л≥ Ї шлейф (TrailRenderer), скидаЇмо його, щоб в≥н не т€гнувс€ через всю карту
        TrailRenderer trail = GetComponent<TrailRenderer>();
        if (trail != null) trail.Clear();
    }

    public void Seek(Enemy _target, float _damage)
    {
        target = _target;
        damage = _damage;
    }

    void Update()
    {
        if (target == null)
        {
            ReturnToPool(); // «ам≥нено на пул
            return;
        }

        Vector2 direction = (Vector2)target.transform.position - (Vector2)transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void HitTarget()
    {
        target.TakeDamage(damage);
        ReturnToPool();
    }
}