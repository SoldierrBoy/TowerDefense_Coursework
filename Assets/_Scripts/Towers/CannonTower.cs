using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : MonoBehaviour
{
    [Header("Характеристики")]
    public float damage = 50f;
    public float fireRate = 0.3f;  // Постріл раз на 3-4 секунди

    public GameObject cannonBallPrefab;
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
        enemiesInRange.RemoveAll(e => e == null || !e.gameObject.activeSelf);
        if (enemiesInRange.Count == 0) { currentTarget = null; return; }

        float maxHealth = -1f;
        Enemy strongestEnemy = null;

        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy.CurrentHealth > maxHealth)
            {
                maxHealth = enemy.CurrentHealth;
                strongestEnemy = enemy;
            }
        }
        currentTarget = strongestEnemy;
    }

    void Shoot()
    {
        // --- ЗМІНЕНО ДЛЯ OBJECT POOLING ---
        // Замість Instantiate беремо ядро з PoolManager
        GameObject ballGO = PoolManager.Instance.Get(cannonBallPrefab, firePoint.position, Quaternion.identity);

        // Зв'язуємо з базовим класом Projectile
        Projectile projectileScript = ballGO.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.myPrefab = cannonBallPrefab;
        }

        CannonBall ball = ballGO.GetComponent<CannonBall>();
        if (ball != null) ball.Seek(currentTarget, damage);
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