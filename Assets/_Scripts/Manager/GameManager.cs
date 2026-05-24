using UnityEngine;
using TMPro;         // Для роботи з TextMeshPro (текст золота)
using UnityEngine.UI;   // Для роботи зі Slider (смужка здоров'я)

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Base Settings")]
    public int baseHealth = 20;
    private int maxHealth = 20; // Фіксуємо максимум для гарного відображення "20 / 20"

    [Header("Economy")]
    public int currentGold = 1000;

    [Header("Rounds")]
    public int currentRound = 0;

    [Header("Старі UI посилання колеги")]
    public TextMeshProUGUI goldText;  // Посилання на старий GoldText
    public Slider healthSlider;       // Посилання на старий HealthSlider

    [Header("Нові UI посилання Раундів")]
    public TextMeshProUGUI newGoldText;       // Сюди кидаємо нове золото (жовте)
    public TextMeshProUGUI newHealthText;     // Сюди кидаємо нове ХП (червоне 20/20)
    public TextMeshProUGUI phaseTitleText;    // Сюди кидаємо заголовок "ПІДГОТОВКА"
    public TextMeshProUGUI phaseSubText;      // Сюди кидаємо підзаголовок "Підготуй оборону"
    public TextMeshProUGUI roundProgressText; // Сюди кидаємо текст "РАУНД 1 / 15"

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Одразу виводимо стартові значення на екран при запуску гри
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

    // Функція синхронізації коду з інтерфейсом (Оновлює ВСІ канваси одночасно)
    public void UpdateUI()
    {
        // --- 1. Оновлення СТАРОГО UI (колеги) ---
        if (goldText != null)
        {
            goldText.text = "Золото: " + currentGold;
        }

        if (healthSlider != null)
        {
            healthSlider.value = baseHealth;
        }


        // --- 2. Оновлення ТВОГО НОВОГО UI ---
        // Оновлюємо нове золото (просто цифра, наприклад 1000)
        if (newGoldText != null)
        {
            newGoldText.text = currentGold.ToString();
        }

        // Оновлюємо нове ХП у форматі "20 / 20"
        if (newHealthText != null)
        {
            newHealthText.text = $"{baseHealth} / {maxHealth}";
        }
    }

    // Окремий метод, який будуть викликати наші States (стани) для зміни написів фаз
    public void UpdatePhaseUI(string title, string subtitle, Color titleColor)
    {
        if (phaseTitleText != null)
        {
            phaseTitleText.text = title;
            phaseTitleText.color = titleColor;
        }

        if (phaseSubText != null)
        {
            phaseSubText.text = subtitle;
        }

        // Оновлюємо також і плашку раунду (всього задамо 15 раундів)
        if (roundProgressText != null)
        {
            roundProgressText.text = $"РАУНД {currentRound} / 15";
        }
    }
}