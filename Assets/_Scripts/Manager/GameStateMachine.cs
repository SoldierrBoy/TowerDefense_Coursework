using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine Instance { get; private set; }

    private IState _currentState;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // На старті гри ми маємо автоматично увійти у фазу підготовки.
        // Але спочатку нам треба створити цей стан. Про це нижче!

        // При запуску гри створюємо стан підготовки і входимо в нього
        ChangeState(new PreparationState(this));
    }

    private void Update()
    {
        // Кожного кадру викликаємо Update того стану, який зараз активний
        if (_currentState != null)
        {
            _currentState.Update();
        }
    }

    // Головна функція для зміни станів
    public void ChangeState(IState newState)
    {
        // 1. Якщо якийсь стан уже працює — виходимо з нього
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        // 2. Міняємо стан на новий
        _currentState = newState;

        // 3. Входимо в новий стан
        if (_currentState != null)
        {
            _currentState.Enter();
        }
    }
}