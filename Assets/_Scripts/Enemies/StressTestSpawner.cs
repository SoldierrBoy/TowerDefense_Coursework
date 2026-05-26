using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressTestSpawner : MonoBehaviour
{
    [Header("Префаби ворогів (за ТЗ)")]
    public GameObject goblinPrefab;
    public GameObject orcPrefab;
    public GameObject ghostPrefab;
    public GameObject bossPrefab;

    [Header("Параметри спавну")]
    public Transform[] waypoints;

    [Header("Стан хвилі")]
    [SerializeField] private int enemiesLeftAlive = 0;

    [Header("Налаштування стрес-тесту")]
    public int totalEnemiesToSpawn = 60;

    private List<Collider2D> spawnedColliders = new List<Collider2D>();

    void Start()
    {
        Invoke("LaunchCrowdChaosTest", 0.5f);
    }

    private void LaunchCrowdChaosTest()
    {
        Debug.Log($"--- STRESS TEST: Спавнимо ОДНОЧАСНО {totalEnemiesToSpawn} ворогів з розкидом! ---");

        if (GameStateMachine.Instance != null)
        {
            GameStateMachine.Instance.ChangeState(new BattleState(GameStateMachine.Instance));
        }

        enemiesLeftAlive = 0;
        spawnedColliders.Clear();

        for (int i = 0; i < totalEnemiesToSpawn; i++)
        {
            float rand = Random.value;
            GameObject prefabToSpawn = goblinPrefab;

            if (rand > 0.95f && bossPrefab != null) prefabToSpawn = bossPrefab;
            else if (rand > 0.7f && orcPrefab != null) prefabToSpawn = orcPrefab;
            else if (rand > 0.4f && ghostPrefab != null) prefabToSpawn = ghostPrefab;

            if (prefabToSpawn != null)
            {
                Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

                GameObject newEnemy = PoolManager.Instance.Get(prefabToSpawn, spawnPosition, Quaternion.identity);

                Collider2D enemyCollider = newEnemy.GetComponent<Collider2D>();
                if (enemyCollider != null)
                {
                    enemyCollider.isTrigger = true;
                    spawnedColliders.Add(enemyCollider);
                }

                Enemy enemyScript = newEnemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.waypoints = waypoints;
                    enemyScript.myPrefab = prefabToSpawn;
                }

                enemiesLeftAlive++;
            }
        }

        Debug.Log($"[Тест] Лавина спавну завершена! Чекаємо пів секунди перед ввімкненням фізики...");
        StartCoroutine(EnableCollisionsRoutine());
    }

    private IEnumerator EnableCollisionsRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("--- STRESS TEST: Вмикаємо фізичну штовханину! ---");

        foreach (Collider2D col in spawnedColliders)
        {
            if (col != null && col.gameObject.activeSelf)
            {
                col.isTrigger = false;
            }
        }
        spawnedColliders.Clear();
    }

    public void EnemyDestroyed()
    {
        if (enemiesLeftAlive <= 0) return;
        enemiesLeftAlive--;

        Debug.Log($"[Тест] Ворога знищено! Залишилось: {enemiesLeftAlive}");

        CheckWaveEnd();
    }

    private void CheckWaveEnd()
    {
        if (enemiesLeftAlive <= 0)
        {
            enemiesLeftAlive = 0;
            Debug.Log("--- STRESS TEST: Успішно завершено! Усі вороги розбиті пулом. Переходимо у WinState ---");

            if (GameStateMachine.Instance != null)
            {
                GameStateMachine.Instance.ChangeState(new WinState(GameStateMachine.Instance));
            }
        }
    }
}