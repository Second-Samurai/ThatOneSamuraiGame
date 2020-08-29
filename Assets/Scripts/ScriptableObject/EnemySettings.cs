using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [Header("Test Data Objects")]

    [Space]
    public EntityStatData enemyData;
    
    public Transform target;
}
