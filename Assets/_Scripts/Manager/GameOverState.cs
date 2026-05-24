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
        Time.timeScale = 0f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdatePhaseUI("ПРОГРАШ", "Базу знищено!", Color.red);
        if (GameManager.Instance.endGamePanel != null)
            {
                GameManager.Instance.endGamePanel.SetActive(true);
            }
        }

        Debug.Log("--- [STATE MACHINE]: GAME OVER ---");
    }

    public void Update() { }

    public void Exit() { }
}