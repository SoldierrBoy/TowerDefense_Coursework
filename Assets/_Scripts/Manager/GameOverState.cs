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
        // Зупиняємо час
        Time.timeScale = 0f;

        if (GameManager.Instance != null)
        {
        
            
            // Намагаємося увімкнути панель і перевіряємо, чи вона є
            if (GameManager.Instance.endGamePanel != null)
            {
                GameManager.Instance.endGamePanel.SetActive(true);
                Debug.Log("[GameOverState]: Панель кінця гри успішно активована з коду!");
            }
            else
            {
                Debug.LogError("[GameOverState]: ПОМИЛКА! endGamePanel не призначена в GameManager. Перетягни її в Інспекторі!");
            }
        }

        Debug.Log("--- [STATE MACHINE]: GAME OVER ---");
    }

    public void Update() { }

    public void Exit() { }
}