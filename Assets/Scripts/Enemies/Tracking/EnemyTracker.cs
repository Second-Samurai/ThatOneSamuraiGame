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
            Debug.Log("Reducing impatience");
            _impatienceMeter -= Time.deltaTime;
        }
        else
        {
            _impatienceMeter = 0;
            _bReduceImpatience = false;
            PickApproachingTarget();
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
    }

    public void ClearTarget()
    {
        _bReduceImpatience = false;
        targetEnemy = null;
    }

    // Called when an enemy enters the circling state, stunned state or death state
    public void StartImpatienceCountdown()
    {
        _bReduceImpatience = true;
        _impatienceMeter = Random.Range(_enemySettings.minImpatienceTime, _enemySettings.maxImpatienceTime);
    }
    
    private void PickApproachingTarget()
    {
        // Don't pick a target if no enemies are in the tracker
        if (currentEnemies.Count <= 0)
            return;
        
        int targetSelector = Random.Range(0, 10);

        // 30% chance if there are enemies in the list, 100% if there is no target enemy
        if(targetSelector < 3 || targetEnemy == null) 
        {
            currentEnemies[0].GetComponent<AISystem>().OnCloseDistance();
        }
        else // 70% chance
        {
            targetEnemy.GetComponent<AISystem>().OnCloseDistance();
        }
    }
}
