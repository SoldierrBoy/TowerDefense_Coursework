using UnityEngine;
using TMPro;         // ��� ������ � TextMeshPro (����� ������)
using UnityEngine.UI;   // ��� ������ � Slider (������ ������'�)

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Base Settings")]
    public int baseHealth = 20;
    private int maxHealth = 20; // Գ����� �������� ��� ������� ����������� "20 / 20"

    [Header("Economy")]
    public int currentGold = 1000;

    [Header("Rounds")]
    public int currentRound = 0;

    [Header("ShopPanel UI")]
    public TextMeshProUGUI goldText;  
    public Slider healthSlider;       

    [Header("��� UI ��������� ������")]
    public TextMeshProUGUI newGoldText;       // ���� ������ ���� ������ (�����)
    public TextMeshProUGUI newHealthText;     // ���� ������ ���� �� (������� 20/20)
    public TextMeshProUGUI phaseTitleText;    // ���� ������ ��������� "ϲ��������"
    public TextMeshProUGUI phaseSubText;      // ���� ������ ����������� "ϳ������ �������"
    public TextMeshProUGUI roundProgressText; // ���� ������ ����� "����� 1 / 15"

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // ������ �������� ������� �������� �� ����� ��� ������� ���
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
        Debug.Log("������ ��������! �������� ������: " + currentGold);

        UpdateUI(); // ��������� ������ �� ������
    }

    // ������� ������������� ���� � ����������� (������� �Ѳ ������� ���������)
    public void UpdateUI()
    {
        // --- 1. ��������� ������� UI (������) ---
        if (goldText != null)
        {
            goldText.text = "" + currentGold;
        }

        if (healthSlider != null)
        {
            healthSlider.value = baseHealth;
        }


        // --- 2. ��������� ����� ������ UI ---
        // ��������� ���� ������ (������ �����, ��������� 1000)
        if (newGoldText != null)
        {
            newGoldText.text = currentGold.ToString();
        }

        // ��������� ���� �� � ������ "20 / 20"
        if (newHealthText != null)
        {
            newHealthText.text = $"{baseHealth} / {maxHealth}";
        }
    }

    // ������� �����, ���� ������ ��������� ���� States (�����) ��� ���� ������ ���
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

        // ��������� ����� � ������ ������ (������ ������ 15 ������)
        if (roundProgressText != null)
        {
            roundProgressText.text = $"����� {currentRound} / 15";
        }
    }
}