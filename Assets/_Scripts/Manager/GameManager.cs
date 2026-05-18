using UnityEngine;
using TMPro;         // Для роботи з TextMeshPro (текст золота)
using UnityEngine.UI;   // Для роботи зі Slider (смужка здоров'я)

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Base Settings")]
    public int baseHealth = 20;

    [Header("Economy")]
    public int currentGold = 1000;

    [Header("UI References")]
    public TextMeshProUGUI goldText;  // Посилання на твій об'єкт GoldText
    public Slider healthSlider;       // Посилання на твій об'єкт HealthSlider

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Одразу виводимо стартові значення на екран
        UpdateUI();
    }

    public void TakeBaseDamage(int damage)
    {
        baseHealth -= damage;
        Debug.Log("База отримала урон! Залишилось життів: " + baseHealth);

        UpdateUI(); // Оновлюємо ХП на екрані

        if (baseHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("ГРА ЗАКІНЧЕНА! Базу зруйновано.");
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        Debug.Log("Золото отримано! Поточний баланс: " + currentGold);

        UpdateUI(); // Оновлюємо золото на екрані
    }

    // Функція синхронізації коду з інтерфейсом
    void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = "Золото: " + currentGold;
        }

        if (healthSlider != null)
        {
            healthSlider.value = baseHealth;
        }
    }
}