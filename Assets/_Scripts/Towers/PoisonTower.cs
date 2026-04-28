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
        // 1. Оновлюємо ціль (шукаємо найближчу)
        UpdateTarget();

        if (currentTarget != null)
        {
            // 2. Ось тут використовується fireCountdown!
            // Він зменшується кожну секунду
            fireCountdown -= Time.deltaTime;

            if (fireCountdown <= 0f)
            {
                Shoot();
                // Скидаємо таймер згідно з темпом стрільби
                fireCountdown = 1f / fireRate;
            }
        }
    }
    void UpdateTarget()
    {
        // Очищуємо список від мертвих ворогів
        enemiesInRange.RemoveAll(e => e == null);

        if (enemiesInRange.Count == 0)
        {
            currentTarget = null;
            return;
        }

        // Шукаємо найближчого
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
        if (currentTarget == null) return; // Стріляємо тільки якщо є ціль

        GameObject projGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        PoisonProjectile proj = projGO.GetComponent<PoisonProjectile>();

        if (proj != null)
            proj.Seek(currentTarget, impactDamage, poisonDamage); // Використовуємо вибрану ціль
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