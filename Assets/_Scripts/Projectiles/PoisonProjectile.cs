using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonProjectile : MonoBehaviour
{
    private Enemy target;
    private float impactDamage;
    private float poisonDamage;
    public float speed = 7f;
    public float explosionRadius = 2f; // Радіус AoE

    public void Seek(Enemy _target, float _impact, float _poison)
    {
        target = _target;
        impactDamage = _impact;
        poisonDamage = _poison;
    }

    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }

        Vector2 direction = (Vector2)target.transform.position - (Vector2)transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            Explode();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void Explode()
    {
        // Знаходимо всіх у радіусі вибуху (AoE)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in hitEnemies)
        {
            if (col.CompareTag("Enemy"))
            {
                Enemy e = col.GetComponent<Enemy>();
                if (e != null)
                {
                    // 1. Миттєва шкода від вибуху
                    e.TakeDamage(impactDamage);

                    // 2. Накладаємо ефект отрути (якщо ще немає)
                    if (e.gameObject.GetComponent<PoisonEffect>() == null)
                    {
                        PoisonEffect effect = e.gameObject.AddComponent<PoisonEffect>();
                        effect.Initialize(poisonDamage, 4f); // 4 секунди отрути
                    }
                }
            }
        }
        // Тут можна додати візуальний ефект спалаху
        Destroy(gameObject);
    }

    // Малюємо радіус вибуху в редакторі для зручності
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}