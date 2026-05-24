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
            // Виводимо красивий зелений напис
            GameManager.Instance.UpdatePhaseUI("ПЕРЕМОГА!", "Ви захистили базу від усіх хвиль!", Color.green);
            
            // ВМИКАЄМО ПАНЕЛЬ З КНОПКАМИ (Ось цей новий шматок!)
            if (GameManager.Instance.endGamePanel != null)
            {
                GameManager.Instance.endGamePanel.SetActive(true);
            }
        }
        
        Debug.Log("--- [STATE MACHINE]: ГРАВЕЦЬ ПЕРЕМІГ! ---");
    }

    public void Update() { }

    public void Exit() { }
}