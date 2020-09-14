using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [Header("Test Data Objects")]

    [Space]
    public EntityStatData enemyData;

    public float circleSpeed;

    public float chaseToCircleRange;
    
    public float circleToChaseRange;

    public float circleThreatenRange;

    public float followUpAttackRange;

    // Scriptable object items saved in runtime are to be made private
    private Transform _target;

    public Transform GetTarget()
    {
        return _target;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
