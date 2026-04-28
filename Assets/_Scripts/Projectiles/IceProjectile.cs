using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    private Enemy target;
    private float damage;
    private float slowFactor;
    public float speed = 8f;

    public void Seek(Enemy _target, float _damage, float _slow)
    {
        target = _target;
        damage = _damage;
        slowFactor = _slow;
    }

    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }

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

        // Додаємо ефект уповільнення, якщо його ще немає
        if (target.gameObject.GetComponent<SlowEffect>() == null)
        {
            SlowEffect effect = target.gameObject.AddComponent<SlowEffect>();
            effect.Initialize(slowFactor, 6f); // Уповільнення на 3 секунди
        }

        Destroy(gameObject);
    }
}