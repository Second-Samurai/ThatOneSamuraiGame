using Enemies;
using Enemies.Enemy_States;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class EnemyRewindData
{
    public EnemyState enemyState;
    public bool swordCollider;
    public bool canGuard;
    public bool canParry;
    public bool isStunned;
    public float currentGuard;
    public bool bIsDead;
    public bool bIsUnblockable;
    public Transform[] trackedCurrentEnemies;
    
    

    public EnemyRewindData(EnemyState _enemyState, bool _swordCollider, bool _canGuard, bool _canParry, bool _isStunned, float _currentGuard, bool _bIsDead, bool _bIsUnblockable, List<Transform> _trackedCurrentEnemies) 
    {
        enemyState = _enemyState;
        swordCollider = _swordCollider;
        canGuard = _canGuard;
        canParry = _canParry;
        isStunned = _isStunned;
        currentGuard = _currentGuard;
        bIsDead = _bIsDead;
        bIsUnblockable = _bIsUnblockable;
        trackedCurrentEnemies = _trackedCurrentEnemies.ToArray<Transform>();
    }
}
