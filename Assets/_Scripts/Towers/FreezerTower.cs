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

    /*void UpdateTarget()
    {
        enemiesInRange.RemoveAll(e => e == null);

        if (enemiesInRange.Count == 0) { currentTarget = null; return; }

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
    }*/
    void UpdateTarget()
    {
        enemiesInRange.RemoveAll(e => e == null);

        if (enemiesInRange.Count == 0) { currentTarget = null; return; }

        float shortestDistance = Mathf.Infinity;
        Enemy nearestNotSlowedEnemy = null;
        Enemy nearestAnyEnemy = null; // Про всяк випадок, якщо всі вже сповільнені

        foreach (Enemy enemy in enemiesInRange)
        {
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

            // Паралельно запам'ятовуємо просто найближчого (на випадок, якщо всі сповільнені)
            if (nearestAnyEnemy == null || distanceToEnemy < Vector2.Distance(transform.position, nearestAnyEnemy.transform.position))
            {
                nearestAnyEnemy = enemy;
            }
        }

        // Якщо знайшли несповільненого — стріляємо в нього. 
        // Якщо таких немає — стріляємо в просто найближчого.
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
        GameObject projGO = Instantiate(iceProjectilePrefab, firePoint.position, Quaternion.identity);
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