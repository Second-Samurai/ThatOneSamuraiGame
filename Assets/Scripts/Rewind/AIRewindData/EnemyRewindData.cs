using Enemies;
using Enemies.Enemy_States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRewindData
{
    public EnemyState enemyState;
    public StatHandler statHandler;

    public EnemyRewindData(EnemyState _enemyState, StatHandler _statHandler) 
    {
        enemyState = _enemyState;
        statHandler = _statHandler;
    }
}
