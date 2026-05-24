using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [Header("UI Елементи")]
    public GameObject towerPanel; // Твоя велика ShopPanel

    [Header("Префаби Веж")]
    public GameObject archerPrefab;
    public GameObject cannonPrefab;
    public GameObject freezerPrefab;
    public GameObject poisonPrefab;

    [Header("Ціни Веж")]
    public int archerCost = 100;
    public int cannonCost = 150;
    public int freezerCost = 200;
    public int poisonCost = 250;

    private Node selectedNode; 

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (towerPanel != null) towerPanel.SetActive(false);
    }
    
    void Update()
    {
        // Якщо панель магазину активна І гравець натиснув Esc
        if (towerPanel != null && towerPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShopWithoutBuying();
        }
    }

    // Викликається при кліку на земляну пляму (Node)
    public void SelectNode(Node node)
    {
        // ЗАХИСТ: Якщо базу знищено або гра виграна, не даємо відкрити магазин
        if (GameManager.Instance != null && (GameManager.Instance.baseHealth <= 0 || GameManager.Instance.currentRound > 15))
        {
            return;
        }

        selectedNode = node;
        
        if (towerPanel != null) towerPanel.SetActive(true);
        
        // МАГІЯ ПАУЗИ: Зупиняємо весь час у грі (рух ворогів, спавн хвиль тощо)
        Time.timeScale = 0f;
        
        Debug.Log("Магазин відкрито! Гра поставлена на паузу.");
    }

    // Викликається кнопками "Вибрати" всередині карток веж
    public void BuildTower(int towerIndex)
    {
        if (selectedNode == null) return;

        // ЗАХИСТ ВІД КЛІКІВ ПІСЛЯ КІНЦЯ ГРИ:
        // Якщо ХП бази <= 0 або раундів більше 15, блокуємо будівництво і відновлення часу
        if (GameManager.Instance != null && (GameManager.Instance.baseHealth <= 0 || GameManager.Instance.currentRound > 15))
        {
            if (towerPanel != null) towerPanel.SetActive(false);
            return;
        }

        GameObject towerToBuild = null;
        int cost = 0;

        switch (towerIndex)
        {
            case 0: towerToBuild = archerPrefab; cost = archerCost; break;
            case 1: towerToBuild = cannonPrefab; cost = cannonCost; break;
            case 2: towerToBuild = freezerPrefab; cost = freezerCost; break;
            case 3: towerToBuild = poisonPrefab; cost = poisonCost; break;
        }

        if (GameManager.Instance.currentGold < cost)
        {
            Debug.Log("Недостатньо золота! Гра залишається на паузі, виберіть іншу вежу.");
            return; 
        }

        GameManager.Instance.AddGold(-cost);
        
        // 1. Створюємо вежу на сцені
        GameObject newTower = Instantiate(towerToBuild, selectedNode.transform.position, Quaternion.identity);

        selectedNode.towerOnThisNode = newTower; 

        // ВІДНОВЛЕННЯ ГРИ: Повертаємо нормальну швидкість часу (1) після покупки
        Time.timeScale = 1f;

        selectedNode = null; 
        if (towerPanel != null) towerPanel.SetActive(false);
    }

    // Додаткова funkція: якщо захочете зробити кнопку "Закрити магазин" (хрестик)
    public void CloseShopWithoutBuying()
    {
        // Знімаємо з паузи тільки якщо гра ще триває!
        if (GameManager.Instance != null && GameManager.Instance.baseHealth > 0 && GameManager.Instance.currentRound <= 15)
        {
            Time.timeScale = 1f; 
        }
        
        selectedNode = null;
        if (towerPanel != null) towerPanel.SetActive(false);
    }
}