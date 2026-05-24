using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : IState
{
    private GameStateMachine _stateMachine;

    // Конструктор (ось тут була помилка з GameState.)
    public BattleState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Time.timeScale = 1f;
        // ДОДАЄМО ЦЕЙ РЯДОК: Оновлюємо екран на фазу бою
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdatePhaseUI("BATTLE", "Вороги наступають!", Color.red);
        }
        Debug.Log("--- [STATE MACHINE]: УСПІШНО УВІЙШЛИ В BATTLE STATE! ---");
        Debug.Log("Кнопка старту схована. Надаємо команду спавнеру...");

        // Знаходимо наш спавнер на сцені
        EnemySpawner spawner = Object.FindAnyObjectByType<EnemySpawner>();

        if (spawner != null)
        {
            // Запускаємо реальну хвилю ворогів!
            spawner.StartEnemyWave();
        }
        else
        {
            Debug.LogError("ПОМИЛКА: Не знайдено EnemySpawner на сцені! Перевірка автоматично повертає в підготовку.");
            _stateMachine.ChangeState(new PreparationState(_stateMachine));
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