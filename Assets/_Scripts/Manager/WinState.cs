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
            
            
            // Намагаємося увімкнути панель і перевіряємо, чи вона є
            if (GameManager.Instance.endGamePanel != null)
            {
                GameManager.Instance.endGamePanel.SetActive(true);
                Debug.Log("[WinState]: Панель кінця гри успішно активована з коду!");
            }
            else
            {
                Debug.LogError("[WinState]: ПОМИЛКА! endGamePanel не призначена в GameManager. Перетягни її в Інспекторі!");
            }
        }
        
        Debug.Log("--- [STATE MACHINE]: ГРАВЕЦЬ ПЕРЕМІГ! ---");
    }

    public void Update() { }

    public void Exit() { }
}