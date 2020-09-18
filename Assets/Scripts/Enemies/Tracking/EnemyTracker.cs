using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

// ENEMY TRACKER INFO
// Used to track all enemies in a scene through a list of transforms
// This list is obtained by searching for enemy tags on awake

public class EnemyTracker : MonoBehaviour
{
    public List<Transform> currentEnemies;
    public Transform targetEnemy;

    private EnemySettings _enemySettings;
    private float _impatienceMeter;
    private bool _bReduceImpatience;

    private void Start()
    {
        _enemySettings = GameManager.instance.gameSettings.enemySettings;
    }

    private void Update()
    {
        // Only run update if _bReduceImpatience is true
        if (!_bReduceImpatience)
        {
            return;
        }
        
        if (_impatienceMeter > 0)
        {
            _impatienceMeter -= Time.deltaTime;
        }
        else
        {
            _impatienceMeter = 0;
            _bReduceImpatience = false;
            targetEnemy.GetComponent<AISystem>().OnCloseDistance();
        }
    }

    public void AddEnemy(Transform enemy)
    {
        if (!currentEnemies.Contains(enemy))
        {
            currentEnemies.Add(enemy);
        }
    }

    public void RemoveEnemy(Transform enemy)
    {
        if (currentEnemies.Contains(enemy))
        {
            currentEnemies.Remove(enemy);
            currentEnemies.TrimExcess();
        }
    }

    public void SetTarget(Transform newTargetEnemy)
    {
        targetEnemy = newTargetEnemy;
        StartImpatienceCountdown();
    }

    public void ClearTarget()
    {
        _bReduceImpatience = false;
        targetEnemy = null;
    }

    // Called when target is locked on OR when the enemy circling you and is the lock on target
    public void StartImpatienceCountdown()
    {
        _bReduceImpatience = true;
        _impatienceMeter = Random.Range(_enemySettings.minImpatienceTime, _enemySettings.maxImpatienceTime);
    }
}
