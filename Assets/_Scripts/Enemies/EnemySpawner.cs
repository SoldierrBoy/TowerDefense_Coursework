using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Сюди перетягнемо наш префаб
    public Transform[] waypoints;  // Сюди перетягнемо точки шляху
    public float spawnInterval = 2f; // Пауза між ворогами

    private void Start()
    {
        // Запускаємо повторюваний виклик функції спавну
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Створюємо ворога в позиції спавнера
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // Передаємо ворогу шлях, щоб він знав, куди йти
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.waypoints = waypoints;
        }
    }
}