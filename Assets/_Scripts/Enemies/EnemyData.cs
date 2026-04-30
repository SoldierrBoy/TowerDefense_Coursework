using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Цей рядок дозволить створювати файли даних через праву кнопку миші
[CreateAssetMenu(fileName = "NewEnemyData", menuName = "TowerDefense/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float health;
    public float speed;
    public int attackCost; // Вартість для бюджету Атакуючого
    [Header("Special Abilities")]
    public bool isImmuneToSlow;
    public int goldReward;
}
