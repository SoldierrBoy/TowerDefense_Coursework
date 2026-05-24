using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    // Метод для кнопки "Грати знову"
    public void RestartGame()
    {
        // ВАЖЛИВО: Повертаємо час у норму, інакше нова гра запуститься на паузі!
        Time.timeScale = 1f;

        // Перезавантажуємо поточну активну сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Метод для кнопки "Головне меню"
    public void LoadMainMenu()
    {
        // Повертаємо час у норму для майбутніх запусків
        Time.timeScale = 1f;

        // Завантажуємо сцену меню за назвою. 
        // Замініть "MainMenu", якщо ваша сцена меню називається інакше!
        SceneManager.LoadScene("MainMenu");
    }
}