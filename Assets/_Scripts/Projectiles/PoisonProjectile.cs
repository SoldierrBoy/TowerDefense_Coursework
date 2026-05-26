using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonProjectile : Projectile
{
    private Enemy target;
    private float impactDamage;
    private float poisonDamage;
    public float speed = 7f;
    public float explosionRadius = 2f;

    private void OnEnable()
    {
        TrailRenderer trail = GetComponent<TrailRenderer>();
        if (trail != null) trail.Clear();
    }

    public void Seek(Enemy _target, float _impact, float _poison)
    {
        target = _target;
        impactDamage = _impact;
        poisonDamage = _poison;
    }

    void Update()
    {
        if (target == null)
        {
            ReturnToPool(); 
            return;
        }

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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in hitEnemies)
        {
            if (col.CompareTag("Enemy"))
            {
                Enemy e = col.GetComponent<Enemy>();
                if (e != null)
                {

                    e.TakeDamage(impactDamage);
                    if (e.gameObject.activeInHierarchy)
                    {
                        if (e.gameObject.GetComponent<PoisonEffect>() == null)
                        {
                            PoisonEffect effect = e.gameObject.AddComponent<PoisonEffect>();
                            effect.Initialize(poisonDamage, 4f);
                        }
                    }
                }
            }
        }
        ReturnToPool();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}