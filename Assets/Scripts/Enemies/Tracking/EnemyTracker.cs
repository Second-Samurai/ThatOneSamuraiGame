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
    private AISystem _targetEnemyAISystem;

    private EnemySettings _enemySettings;
    private float _impatienceMeter;
    private bool _bReduceImpatience;

    private CameraControl _cameraControl;

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
            //Debug.Log("Reducing impatience");
            _impatienceMeter -= Time.deltaTime;
        }
        else
        {
            StopImpatienceCountdown();
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
        _targetEnemyAISystem = targetEnemy.GetComponent<AISystem>();

        // Stop here if there is no AI system (i.e. archers)
        if (_targetEnemyAISystem == null) return;
        
        // Enable the guard meter
        _targetEnemyAISystem.eDamageController.enemyGuard.EnableGuardMeter();
        // Set the guard meter to visible through this event
        _targetEnemyAISystem.eDamageController.enemyGuard.OnGuardEvent.Invoke();
    }

    public void ClearTarget()
    {
        _bReduceImpatience = false;
        targetEnemy = null;
        _targetEnemyAISystem = null;
    }

    // Called when an enemy enters the circling state, stunned state or death state
    public void StartImpatienceCountdown()
    {
        _bReduceImpatience = true;
                
        _impatienceMeter = Random.Range(_enemySettings.minImpatienceTime, _enemySettings.maxImpatienceTime);
    }
    
    // Called to stop counting down the impatience meter. Called in EnemyTracker and certain states
    public void StopImpatienceCountdown()
    {
        _impatienceMeter = 0;
        _bReduceImpatience = false;
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
            // Go through and find an enemy that isn't stunned to approach
            foreach (Transform enemy in currentEnemies)
            {
                if (!enemy.gameObject.activeInHierarchy) return;

                AISystem aiSystem = enemy.GetComponent<AISystem>();
                
                // Only close distance if the enemy isn't stunned and is strafing
                if (!aiSystem.eDamageController.enemyGuard.isStunned && !aiSystem.bIsDead)
                {
                    if (aiSystem.enemyType == EnemyType.GLAIVEWIELDER) aiSystem.OnChargePlayer();
                    else if (aiSystem.enemyType == EnemyType.BOSS) aiSystem.OnJumpAttack();
                    else aiSystem.OnCloseDistance();
                    break;
                }
            }
        }
        else // 70% chance
        {
            if (!_targetEnemyAISystem.eDamageController.enemyGuard.isStunned && !_targetEnemyAISystem.bIsDead)
            {
                if (_targetEnemyAISystem.enemyType == EnemyType.GLAIVEWIELDER ) _targetEnemyAISystem.OnChargePlayer();
                else if (_targetEnemyAISystem.enemyType == EnemyType.BOSS) _targetEnemyAISystem.OnJumpAttack();
                else _targetEnemyAISystem.OnCloseDistance();
            }
        }
    }
    
    // Called when an enemy dies (i.e. Death enemy state)
    public void SwitchDeathTarget(Transform enemyDeathTransform)
    {
        if (enemyDeathTransform == targetEnemy)
        {
            Invoke("FindNewTarget", 1.0f);
        }
    }

    public void FindNewTarget()
    {
        if (currentEnemies.Count > 0)
        {
            GameManager.instance.cameraControl.LockOn();
        }
        else
        {
            GameManager.instance.cameraControl.ToggleLockOn();
        }
    }
}
