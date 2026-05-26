using UnityEngine;

public class GameOverState : IState
{
    private GameStateMachine _stateMachine;

    public GameOverState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        // Зупиняємо час у грі
        Time.timeScale = 0f;

        if (GameManager.Instance != null)
        {
            // Вмикаємо твою нову панель поразки
            if (GameManager.Instance.LosePanel != null)
            {
                GameManager.Instance.LosePanel.SetActive(true);
                Debug.Log("[GameOverState]: Екран ПОРАЗКИ успішно активовано!");
            }
            else
            {
                Debug.LogError("[GameOverState]: ПОМИЛКА! LosePanel не призначена в GameManager.");
            }
        }

        Debug.Log("--- [STATE MACHINE]: GAME OVER ---");
    }

    public void Update() { }

    public void Exit() { }
}