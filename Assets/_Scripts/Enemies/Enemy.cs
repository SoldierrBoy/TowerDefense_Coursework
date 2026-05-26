using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public Transform[] waypoints;

    [Header("UI Settings")]
    public Image healthFill;

    [HideInInspector] public GameObject myPrefab; // Посилання на рідний префаб для пулу

    private float currentHealth;
    public float CurrentHealth => currentHealth;
    private int currentWaypointIndex = 0;
    private float currentSpeed;

    // ВАЖЛИВО: Замість Start використовуємо OnEnable.
    // Цей метод викликається АВТОМАТИЧНО щоразу, коли об'єкт робить SetActive(true)
    void OnEnable()
    {
        if (data != null)
        {
            currentHealth = data.health;
            currentSpeed = data.speed;
            currentWaypointIndex = 0; // Скидаємо індекс точок на початок!
            UpdateHealthBar();
        }
    }

    void Update()
    {
        if (data == null || waypoints == null) return;

        if (currentWaypointIndex >= waypoints.Length)
        {
            ReachBase();
            return;
        }

        Move();
    }

    void Move()
    {
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex++;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        if (data != null && data.isImmuneToSlow)
        {
            currentSpeed = data.speed;
            return;
        }

        currentSpeed = newSpeed;
    }

    public float GetOriginalSpeed()
    {
        return data.speed;
    }

    void ReachBase()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeBaseDamage(1);
        }

        NotifySpawner();

        // ЗАМІСТЬ DESTROY ПОКЛИКАЄМО ПУЛ:
        ReturnToPool();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddGold(data.goldReward);
            }

            NotifySpawner();

            // ЗАМІСТЬ DESTROY ПОКЛИКАЄМО ПУЛ:
            ReturnToPool();
        }
    }

    void UpdateHealthBar()
    {
        if (healthFill != null && data != null)
        {
            healthFill.fillAmount = currentHealth / data.health;
        }
    }

    private void NotifySpawner()
    {

        EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.EnemyDestroyed();
            return; 
        }

        StressTestSpawner testSpawner = FindAnyObjectByType<StressTestSpawner>();
        if (testSpawner != null)
        {
            testSpawner.EnemyDestroyed();
        }
    }

    // Новий внутрішній метод для безпечного повернення в пул
    private void ReturnToPool()
    {
        if (myPrefab != null && PoolManager.Instance != null)
        {
            // Ховаємо об'єкт у заначку
            PoolManager.Instance.Release(myPrefab, gameObject);
        }
        else
        {
            // Якщо раптом щось пішло не так або тестуємо без пулу — просто видаляємо
            Destroy(gameObject);
        }
    }
}