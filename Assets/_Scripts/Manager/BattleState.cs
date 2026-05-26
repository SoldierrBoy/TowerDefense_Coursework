using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : IState
{
    private GameStateMachine _stateMachine;

    public BattleState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdatePhaseUI("     BATTLE", "Enemies are attacking!", Color.red);
        }

        Debug.Log("--- [STATE MACHINE]: УСПІШНО УВІЙШЛИ В BATTLE STATE! ---");
        Debug.Log("Кнопка старту схована. Надаємо команду спавнеру...");

        EnemySpawner spawner = Object.FindAnyObjectByType<EnemySpawner>();

        if (spawner != null)
        {
            spawner.StartEnemyWave();
        }
        else
        {
            StressTestSpawner testSpawner = Object.FindAnyObjectByType<StressTestSpawner>();

            if (testSpawner != null)
            {
                Debug.Log("[STATE MACHINE]: Виявлено StressTestSpawner. Стрес-тест успішно активовано!");
            }
            else
            {
                Debug.LogError("ПОМИЛКА: Не знайдено жодного спавнера ворогів на сцені! Автоматично повертаємо в підготовку.");
                _stateMachine.ChangeState(new PreparationState(_stateMachine));
            }
        }
    }

    public void Update()
    {
        // Тут буде перевірка стану бою кожного кадру
    }

    public void Exit()
    {
        Debug.Log("--- [STATE MACHINE]: ВИХІД З BATTLE STATE ---");
    }

}