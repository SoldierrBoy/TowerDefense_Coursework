using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : Projectile
{
    private Enemy target;
    private float damage;
    public float speed = 12f;

    private void OnEnable()
    {
        // Скидаємо обертання ядра при старті з пулу
        transform.rotation = Quaternion.identity;

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
            ReturnToPool(); // ЗАМІНЕНО: тепер повертаємо в пул, якщо ціль зникла
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

        transform.Rotate(0, 0, 500 * Time.deltaTime);
    }

    void HitTarget()
    {
        target.TakeDamage(damage);
        ReturnToPool();
    }
}