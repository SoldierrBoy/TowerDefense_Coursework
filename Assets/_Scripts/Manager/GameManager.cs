using UnityEngine;
using TMPro;         
using UnityEngine.UI;   

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Base Settings")]
    public int baseHealth = 20;
    private int maxHealth = 20; 

    [Header("Economy")]
    public int currentGold = 1000;

    [Header("Rounds")]
    public int currentRound = 0;

    [Header("ShopPanel UI")]
    public TextMeshProUGUI goldText;  
    public Slider healthSlider;       

    [Header("Нові UI посилання (Раунди)")]
    public TextMeshProUGUI newGoldText;  
    public TextMeshProUGUI newHealthText;     
    public TextMeshProUGUI phaseTitleText;    
    public TextMeshProUGUI phaseSubText;      
    public TextMeshProUGUI roundProgressText; 

    [Header("Кінцеве Меню")]
    public GameObject WinPanel;  
    public GameObject LosePanel; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            UpdateUI();
        }
        else Destroy(gameObject);
    }

    void Start()
    {

    }

    public void TakeBaseDamage(int damage)
    {
        if (baseHealth <= 0) return;
        baseHealth -= damage;
        if (baseHealth < 0) baseHealth = 0;

        Debug.Log("База отримала урон! Залишилось життів: " + baseHealth);

        UpdateUI(); 

        if (baseHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("ГРА ЗАКІНЧЕНА! Базу зруйновано.");


        if (GameStateMachine.Instance != null)
        {
            GameStateMachine.Instance.ChangeState(new GameOverState(GameStateMachine.Instance));
        }
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        Debug.Log("Золото отримано! Поточний баланс: " + currentGold);

        UpdateUI(); 
    }

    public void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = "" + currentGold;
        }

        if (healthSlider != null)
        {
            healthSlider.value = baseHealth;
        }

        if (newGoldText != null)
        {
            newGoldText.text = currentGold.ToString();
        }

        if (newHealthText != null)
        {
            newHealthText.text = $"{baseHealth} / {maxHealth}";
        }
    }

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

        if (roundProgressText != null)
        {
            roundProgressText.text = $"ROUND {currentRound} / 16";
        }
    }
}