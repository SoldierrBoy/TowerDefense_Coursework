using UnityEngine;
using UnityEngine.UI; // Обов'язково для роботи з Image

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage; // Сюди перетягнемо твій зелений Fill

    // Метод, який будемо викликати з скрипта ворога
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // Рахуємо відсоток від 0 до 1
        fillImage.fillAmount = currentHealth / maxHealth;
    }
}