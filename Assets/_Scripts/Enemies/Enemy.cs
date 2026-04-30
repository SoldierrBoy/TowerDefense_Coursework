using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public Transform[] waypoints;

    [Header("UI Settings")]
    public Image healthFill;

    private float currentHealth;
    private int currentWaypointIndex = 0;
    private float currentSpeed;

    void Start()
    {
        if (data != null)
        {
            currentHealth = data.health;
            currentSpeed = data.speed;
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

        Destroy(gameObject);
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
            Destroy(gameObject);
        }
    }

    void UpdateHealthBar()
    {
        if (healthFill != null && data != null)
        {
            healthFill.fillAmount = currentHealth / data.health;
        }
    }
}