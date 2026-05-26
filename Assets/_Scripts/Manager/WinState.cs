using UnityEngine;

public class WinState : IState
{
    private GameStateMachine _stateMachine;

    // Конструктор ПОВИНЕН бути тут, щоб приймати 1 аргумент!
    public WinState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        // Повністю зупиняємо час у грі
        Time.timeScale = 0f;

        if (GameManager.Instance != null)
        {
            // Вмикаємо твою нову панель перемоги
            if (GameManager.Instance.WinPanel != null)
            {
                GameManager.Instance.WinPanel.SetActive(true);
                Debug.Log("[WinState]: Екран ПЕРЕМОГИ успішно активовано!");
            }
            else
            {
                Debug.LogError("[WinState]: ПОМИЛКА! WinPanel не призначена в GameManager.");
            }
        }

        Debug.Log("--- [STATE MACHINE]: ГРАВЕЦЬ ПЕРЕМІГ! ---");
    }

    public void Update() { }

    public void Exit() { }
}