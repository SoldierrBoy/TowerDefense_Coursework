using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezerTower : MonoBehaviour
{
    [Header("Базові налаштування")]
    public float damage = 2f;      // Заморозка зазвичай б'є слабо
    public float fireRate = 1f;
    public float slowFactor = 0.2f; // 0.5 = 50% швидкості

    [Header("Об'єкти")]
    public GameObject iceProjectilePrefab;
    public Transform firePoint;

    private float fireCountdown = 0f;
    private Enemy currentTarget;
    private List<Enemy> enemiesInRange = new List<Enemy>();

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
        // 1. Видаляємо тих, хто null, або вже спить у пулі
        enemiesInRange.RemoveAll(e => e == null || !e.gameObject.activeSelf);

        if (enemiesInRange.Count == 0) { currentTarget = null; return; }

        float shortestDistance = Mathf.Infinity;
        Enemy nearestNotSlowedEnemy = null;
        Enemy nearestAnyEnemy = null;

        foreach (Enemy enemy in enemiesInRange)
        {

            if (enemy.data != null && enemy.data.isImmuneToSlow)
            {
                continue; 
            }

            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            // Спочатку шукаємо того, хто НЕ сповільнений
            if (enemy.GetComponent<SlowEffect>() == null)
            {
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestNotSlowedEnemy = enemy;
                }
            }

            if (nearestAnyEnemy == null || distanceToEnemy < Vector2.Distance(transform.position, nearestAnyEnemy.transform.position))
            {
                nearestAnyEnemy = enemy;
            }
        }

        if (nearestNotSlowedEnemy != null)
        {
            currentTarget = nearestNotSlowedEnemy;
        }
        else
        {
            currentTarget = nearestAnyEnemy;
        }
    }

    void Shoot()
    {
        // --- ЗМІНЕНО ДЛЯ OBJECT POOLING ---
        // Замість Instantiate беремо крижаний снаряд з PoolManager
        GameObject projGO = PoolManager.Instance.Get(iceProjectilePrefab, firePoint.position, Quaternion.identity);

        // Зв'язуємо з базовим класом Projectile
        Projectile projectileScript = projGO.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.myPrefab = iceProjectilePrefab;
        }

        IceProjectile proj = projGO.GetComponent<IceProjectile>();
        if (proj != null) proj.Seek(currentTarget, damage, slowFactor);
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