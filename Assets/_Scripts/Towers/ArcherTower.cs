using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : MonoBehaviour
{
    [Header("Характеристики")]
    public float damage = 10f;
    public float fireRate = 1.2f;
    public float range = 3f;

    [Header("Налаштування снаряда")]
    public GameObject arrowPrefab; // Префаб стріли
    public Transform firePoint;   // Точка, звідки вилітає стріла (наприклад, лук)

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
        // Видаляємо зі списку тих, хто вже помер або вийшов
        enemiesInRange.RemoveAll(e => e == null);

        if (enemiesInRange.Count > 0)
            currentTarget = enemiesInRange[0];
        else
            currentTarget = null;
    }

    void Shoot()
    {
        GameObject arrowGO = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrow = arrowGO.GetComponent<Arrow>();

        if (arrow != null)
        {
            arrow.Seek(currentTarget, damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.GetComponent<Enemy>());
        }
    }
}