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
        Debug.Log("--- [STATE MACHINE]: УСПІШНО УВІЙШЛИ В BATTLE STATE! ---");
        Debug.Log("Вороги спавняться, вежі стріляють. Чекаємо завершення хвилі...");

        // Запускаємо тестову корутину через наш синглтон машини станів
        GameStateMachine.Instance.StartCoroutine(TemporaryWaveRoutine());
    }

    public void Update()
    {
        // Тут буде перевірка стану бою кожного кадру
    }

    public void Exit()
    {
        Debug.Log("--- [STATE MACHINE]: ВИХІД З BATTLE STATE ---");
    }

    // Тимчасова корутина для тесту
    private System.Collections.IEnumerator TemporaryWaveRoutine()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Хвиля закінчилася (тестові 5 секунд минули)!");

        // Повертаємося у фазу підготовки до наступного раунду
        _stateMachine.ChangeState(new PreparationState(_stateMachine));
    }
}