using Enemies;
using Enemies.Enemy_States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRewindData
{
    public EnemyState enemyState;

    public EnemyRewindData(EnemyState _enemyState) 
    {
        enemyState = _enemyState;
    }
}
