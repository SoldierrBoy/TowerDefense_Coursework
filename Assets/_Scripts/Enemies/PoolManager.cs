using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    // Словник, де для кожного префабу буде свій унікальний пул об'єктів
    private Dictionary<GameObject, ObjectPool<GameObject>> _pools = new Dictionary<GameObject, ObjectPool<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Головний метод: дістає об'єкт із потрібного пулу за префабом
    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // Якщо пулу для цього префабу ще немає — створюємо його на льоту
        if (!_pools.ContainsKey(prefab))
        {
            _pools[prefab] = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(prefab),             // Що робити, якщо пул пустий (створити новий)
                actionOnGet: (obj) => obj.SetActive(true),         // Що робити, коли дістаємо з пулу
                actionOnRelease: (obj) => obj.SetActive(false),     // Що робити, коли повертаємо в пул
                actionOnDestroy: (obj) => Destroy(obj),            // Що робити, якщо пул переповнений і треба видалити
                collectionCheck: false,
                defaultCapacity: 20,                               // Початковий розмір заначки
                maxSize: 100                                       // Максимальний розмір заначки
            );
        }

        // Дістаємо об'єкт із пулу
        GameObject objFromPool = _pools[prefab].Get();
        objFromPool.transform.position = position;
        objFromPool.transform.rotation = rotation;

        return objFromPool;
    }

    // Метод для повернення об'єкта назад у заначку
    public void Release(GameObject prefab, GameObject obj)
    {
        if (_pools.ContainsKey(prefab))
        {
            _pools[prefab].Release(obj);
        }
        else
        {
            // Якщо раптом пулу немає, просто видаляємо (захист від багів)
            Destroy(obj);
        }
    }
}