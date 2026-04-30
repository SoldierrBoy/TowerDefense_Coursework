using UnityEngine;
using TMPro; // Якщо будеш використовувати TextMeshPro для відображення життів

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Base Settings")]
    public int baseHealth = 20;
    [Header("Economy")]
    public int currentGold = 1000; 
    void Awake()
    {
        
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TakeBaseDamage(int damage)
    {
        baseHealth -= damage;
        Debug.Log("База отримала урон! Залишилось життів: " + baseHealth);

        if (baseHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("ГРА ЗАКІНЧЕНА! Базу зруйновано.");
        // Тут пізніше можна викликати вікно програшу
    }
    public void AddGold(int amount)
    {
        currentGold += amount;
        Debug.Log("Золото отримано! Поточний баланс: " + currentGold);
    }
}