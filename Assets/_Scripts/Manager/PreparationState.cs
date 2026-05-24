using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationState : IState
{
    private GameStateMachine _stateMachine;

    // Конструктор: коли ми створюємо цей стан, ми передаємо йому посилання на нашу машину
    public PreparationState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        // 1. ПЕРЕВІРКА НА ПЕРЕМОГУ: Якщо гравець вже пройшов 15 раундів
        if (GameManager.Instance != null && GameManager.Instance.currentRound > 15)
        {
            Debug.Log("--- [PREPARATION STATE]: 15 раундів позаду! Перемикаємо на WinState. ---");
            _stateMachine.ChangeState(new WinState(_stateMachine));
            return; // Зупиняємо виконання методу Enter, щоб не починати новий раунд!
        }

        // Оновлюємо екран на фазу підготовки
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdatePhaseUI("ПІДГОТОВКА", "Підготуй оборону", Color.white);
        }

        Debug.Log($"--- ФАЗА ПІДГОТОВКИ ДО РАУНДУ {GameManager.Instance.currentRound} ---");
        GameManager.Instance.currentRound++;
        Debug.Log("Гроші є, вежі ставляться. Натисніть кнопку 'СТАРТ', щоб пішла хвиля.");

        // 2. Тут логіка для твого колеги:
        // Увімкнути UI кнопку "Почати раунд" (наприклад: UIManager.Instance.ShowStartButton(true);)
    }

    public void Update()
    {
        // Тут можна перевіряти, наприклад, гарячі клавіші під час підготовки
        // Якщо гравець натиснув пробіл — теж можна починати бій
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartBattle();
        }
    }

    public void Exit()
    {
        Debug.Log("Вихід з фази підготовки. Кнопка 'СТАРТ' ховається, режим будівництва блокується (опціонально).");

        // Тут логіка для твого колеги:
        // Вимкнути UI кнопку "Почати раунд", щоб гравець не клацав її під час бою
    }

    // Метод, який ми викличемо, коли гравець натисне на кнопку "Старт" на екрані
    public void StartBattle()
    {
        // Перемикаємо машину на стан бою (BattleState)
        _stateMachine.ChangeState(new BattleState(_stateMachine));
    }
}