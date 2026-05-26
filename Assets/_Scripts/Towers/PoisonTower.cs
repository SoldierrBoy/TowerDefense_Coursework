using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : MonoBehaviour
{
    public float impactDamage = 5f; // Шкода від самого вибуху
    public float poisonDamage = 2f; // Шкода від отрути щосекунди
    public float fireRate = 0.5f;   // Стріляє повільно, бо AoE

    public GameObject projectilePrefab;
    public Transform firePoint;

    private float fireCountdown = 0f;
    private List<Enemy> enemiesInRange = new List<Enemy>();
    private Enemy currentTarget;

    void Update()
    {
        UpdateTarget();

        if (currentTarget != null)
        {
            fireCountdown -= Time.deltaTime;

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
        }
    }

    void UpdateTarget()
    {
        enemiesInRange.RemoveAll(e => e == null);

        if (enemiesInRange.Count == 0)
        {
            currentTarget = null;
            return;
        }

        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;

        foreach (Enemy enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        currentTarget = nearestEnemy;
    }

    void Shoot()
    {
        if (currentTarget == null) return;

        // --- ЗМІНЕНО ДЛЯ OBJECT POOLING ---
        // Замість Instantiate беремо отруйну колбу/снаряд з PoolManager
        GameObject projGO = PoolManager.Instance.Get(projectilePrefab, firePoint.position, Quaternion.identity);

        // Зв'язуємо з базовим класом Projectile
        Projectile projectileScript = projGO.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.myPrefab = projectilePrefab;
        }

        PoisonProjectile proj = projGO.GetComponent<PoisonProjectile>();
        if (proj != null)
            proj.Seek(currentTarget, impactDamage, poisonDamage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) enemiesInRange.Add(other.GetComponent<Enemy>());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) enemiesInRange.Remove(other.GetComponent<Enemy>());
    }
}