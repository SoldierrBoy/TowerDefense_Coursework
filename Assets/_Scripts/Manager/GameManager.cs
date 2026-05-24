using UnityEngine;
using TMPro;         // Для роботи з TextMeshPro (текстові панелі)
using UnityEngine.UI;   // Для роботи з Slider (смужка здоров'я)

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Base Settings")]
    public int baseHealth = 20;
    private int maxHealth = 20; // Фіксоване значення для відображення "20 / 20"

    [Header("Economy")]
    public int currentGold = 1000;

    [Header("Rounds")]
    public int currentRound = 0;

    [Header("ShopPanel UI")]
    public TextMeshProUGUI goldText;  
    public Slider healthSlider;       

    [Header("Нові UI посилання (Раунди)")]
    public TextMeshProUGUI newGoldText;       // Сюди кидаємо нове золото (жовте)
    public TextMeshProUGUI newHealthText;     // Сюди кидаємо нове ХП (червоне 20/20)
    public TextMeshProUGUI phaseTitleText;    // Сюди кидаємо заголовок "ПІДГОТОВКА"
    public TextMeshProUGUI phaseSubText;      // Сюди кидаємо підзаголовок "Підготуй оборону"
    public TextMeshProUGUI roundProgressText; // Сюди кидаємо текст "РАУНД 1 / 15"

    [Header("Кінцеве Меню")]
    public GameObject endGamePanel; // Сюди закинемо панель з кнопками "Грати знову" / "Меню"

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Оновлюємо інтерфейс при старті гри
        UpdateUI();
    }

    public void TakeBaseDamage(int damage)
    {
        // Якщо гра вже закінчилася (ХП вже 0), ігноруємо подальші удари
        if (baseHealth <= 0) return;

        baseHealth -= damage;

        // Захист: не даємо ХП впасти нижче нуля (замість -6 буде красиво показувати 0)
        if (baseHealth < 0) baseHealth = 0;

        Debug.Log("База отримала урон! Залишилось життів: " + baseHealth);

        UpdateUI(); // Оновлюємо ХП на екрані (тепер тут буде "0 / 20")

        if (baseHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("ГРА ЗАКІНЧЕНА! Базу зруйновано.");

        // ПЕРЕМИКАЄМО СТАН ГРИ НА ПРОГРАШ:
        if (GameStateMachine.Instance != null)
        {
            GameStateMachine.Instance.ChangeState(new GameOverState(GameStateMachine.Instance));
        }
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        Debug.Log("Золото отримано! Поточний баланс: " + currentGold);

        UpdateUI(); // Оновлюємо золото на екрані
    }

    // Головна функція для оновлення всього UI
    public void UpdateUI()
    {
        // --- 1. Оновлення старого UI (ShopPanel) ---
        if (goldText != null)
        {
            goldText.text = "" + currentGold;
        }

        if (healthSlider != null)
        {
            healthSlider.value = baseHealth;
        }

        // --- 2. Оновлення нового UI (Раунди) ---
        if (newGoldText != null)
        {
            newGoldText.text = currentGold.ToString();
        }

        // Показуємо ХП у вигляді "20 / 20"
        if (newHealthText != null)
        {
            newHealthText.text = $"{baseHealth} / {maxHealth}";
        }
    }

    // Функція для оновлення текстів фаз (Підготовка/Бій)
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

        // Оновлюємо текст з номером раунду
        if (roundProgressText != null)
        {
            roundProgressText.text = $"РАУНД {currentRound} / 15";
        }
    }
}