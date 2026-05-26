using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Префаби ворогів (за ТЗ)")]
    public GameObject goblinPrefab;
    public GameObject orcPrefab;
    public GameObject ghostPrefab;
    public GameObject bossPrefab;

    [Header("Параметри спавну")]
    public Transform[] waypoints;
    public float spawnInterval = 1.5f;

    [Header("Стан хвилі")]
    [SerializeField] private int enemiesLeftAlive = 0;
    private bool isSpawning = false;

    private Queue<GameObject> _generatedWaveQueue = new Queue<GameObject>();

    public void StartEnemyWave()
    {
        if (isSpawning || enemiesLeftAlive > 0) return;

        // На всяк випадок скидаємо лічильник перед початком нової хвилі
        enemiesLeftAlive = 0;

        int currentRound = GameManager.Instance.currentRound;
        Debug.Log($"--- AI DIRECTOR: Формую хвилю для Раунду {currentRound} ---");

        int waveBudget = currentRound * 10;
        GenerateWaveWithAI(waveBudget, currentRound);

        StartCoroutine(SpawnWaveRoutine());
    }

    private void GenerateWaveWithAI(int budget, int round)
    {
        _generatedWaveQueue.Clear();

        while (budget > 0)
        {
            if (round >= 5 && budget >= 15 && Random.value > 0.7f && bossPrefab != null)
            {
                _generatedWaveQueue.Enqueue(bossPrefab);
                budget -= 15;
            }
            else if (round >= 3 && budget >= 7 && Random.value > 0.5f && ghostPrefab != null)
            {
                _generatedWaveQueue.Enqueue(ghostPrefab);
                budget -= 7;
            }
            else if (round >= 2 && budget >= 5 && Random.value > 0.4f && orcPrefab != null)
            {
                _generatedWaveQueue.Enqueue(orcPrefab);
                budget -= 5;
            }
            else if (budget >= 2 && goblinPrefab != null)
            {
                _generatedWaveQueue.Enqueue(goblinPrefab);
                budget -= 2;
            }
            else
            {
                break;
            }
        }

        Debug.Log($"AI DIRECTOR: Хвилю згенеровано! Ворогів у черзі: {_generatedWaveQueue.Count}");
    }

    private IEnumerator SpawnWaveRoutine()
    {
        isSpawning = true;

        while (_generatedWaveQueue.Count > 0)
        {
            GameObject enemyPrefab = _generatedWaveQueue.Dequeue();
            if (enemyPrefab != null)
            {
                SpawnEnemy(enemyPrefab);
            }
            yield return new WaitForSecondsRealtime(spawnInterval);
        }

        isSpawning = false;
        Debug.Log("EnemySpawner: AI закінчив випуск ворогів.");

        // Перевіряємо, чи раптом хвиля не закінчилась одразу (на випадок, якщо вежі вбили всіх миттєво)
        CheckWaveEnd();
    }

    void SpawnEnemy(GameObject prefab)
    {

        GameObject newEnemy = PoolManager.Instance.Get(prefab, transform.position, Quaternion.identity);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.waypoints = waypoints;
            enemyScript.myPrefab = prefab;
        }

        enemiesLeftAlive++;
    }

    public void EnemyDestroyed()
    {
        // Якщо ворогів уже 0, ми не даємо лічильнику йти в мінус через баги сцени
        if (enemiesLeftAlive <= 0) return;

        enemiesLeftAlive--;
        Debug.Log($"Ворога знищено! На карті залишилось: {enemiesLeftAlive}");

        CheckWaveEnd();
    }

    // Окремий чистий метод для перевірки кінця раунду
    private void CheckWaveEnd()
    {
        // Раунд закінчено ТІЛЬКИ якщо ШІ закінчив спавнити І на карті дійсно 0 живих ворогів
        if (!isSpawning && enemiesLeftAlive <= 0)
        {
            enemiesLeftAlive = 0;
            Debug.Log("EnemySpawner: Хвиля AI повністю знищена! Переходимо в підготовку.");
            GameStateMachine.Instance.ChangeState(new PreparationState(GameStateMachine.Instance));
        }
    }
}