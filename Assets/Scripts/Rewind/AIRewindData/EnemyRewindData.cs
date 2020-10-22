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
    public bool b_isIdle;
    public bool bisCircling;
    public bool bSuperArmour;
    public float previousAttackSpeed;
    public float attackSpeed;
    public int armourCount;
    public bool bIsClosingDistance;

    public EnemyRewindData(EnemyState _enemyState, bool _swordCollider, bool _canGuard, bool _canParry, bool _isStunned, float _currentGuard,
                                    bool _bIsDead, bool _bIsUnblockable, List<Transform> _trackedCurrentEnemies, bool _b_isIdle, bool _bisCircling, bool _bSuperArmour, 
                                    float _previousAttackSoeed, float _attackSpeed, int _armourCount, bool _bIsClosingDistance) 
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
        b_isIdle = _b_isIdle;
        bisCircling = _bisCircling;
        bSuperArmour = _bSuperArmour;
        previousAttackSpeed = _previousAttackSoeed;
        attackSpeed = _attackSpeed;
        armourCount = _armourCount;
        bIsClosingDistance = _bIsClosingDistance;
    }
}
