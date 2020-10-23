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

    private Transform _lastKilledEnemy;

    private bool _bIsHeavyCharging = false;

    private void Start()
    {
        _enemySettings = GameManager.instance.gameSettings.enemySettings;
    }

    private void Update()
    {
        // If there are enemies in the list and an impatience timer isn't running, start a new one
        if (!_bReduceImpatience && currentEnemies.Count > 0)
        {
            StartImpatienceCountdown();
        }
        // If there are no enemies in the list and an impatience timer isn't running, don't run the rest of update
        else if (!_bReduceImpatience)
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
            if (currentEnemies.Count > 0)
            {
                StartImpatienceCountdown();
            }
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
        // Don't pick a target if no enemies are in the tracker or if the player is charging a heavy attack
        if (currentEnemies.Count <= 0 || _bIsHeavyCharging)
            return;
        
        // int random is 0 - 9
        int targetSelector = Random.Range(0, 10);

        // 80% chance if there is a targetAISystem
        if(targetSelector >= 2  && _targetEnemyAISystem != null) 
        {
            // If the target enemy isn't suitable (i.e. is not circling, stunned or dead) pick a random target instead
            if (!CheckSuitableApproachTarget(_targetEnemyAISystem))
            {
                PickRandomTarget();
            }
        }
        // 20% chance if there is a suitable _targetEnemyAISystem, 100% if there is no target enemy
        else 
        {
            PickRandomTarget();
        }
    }

    private void PickRandomTarget()
    {
        // Go through and find an enemy that isn't stunned to approach
        foreach (Transform enemy in currentEnemies)
        {
            // If the enemy is set as inactive, ignore it
            if (!enemy.gameObject.activeInHierarchy) return;

            AISystem aiSystem = enemy.GetComponent<AISystem>();
                
            // If the enemy is an archer, ignore it
            if (!aiSystem) return;

            if (CheckSuitableApproachTarget(aiSystem))
            {
                break;
            }
        }
    }

    // Returns true if a suitable target is chosen (i.e. is circling and is not stunned or dead)
    private bool CheckSuitableApproachTarget(AISystem aiSystem)
    {
        // Only close distance if the enemy isn't stunned or dead
        if (!aiSystem.eDamageController.enemyGuard.isStunned && !aiSystem.bIsDead && aiSystem.bIsCircling)
        {
            // Stop circling behaviour in AISystem (this is also called at the end of CircleEnemyState)
            aiSystem.StopCircling();

            if (aiSystem.enemyType == EnemyType.GLAIVEWIELDER) aiSystem.OnChargePlayer();
            else if (aiSystem.enemyType == EnemyType.BOSS) aiSystem.OnJumpAttack();
            else aiSystem.OnCloseDistance();
            return true;
        }

        return false;
    }
    
    // Called when an enemy dies (i.e. Death enemy state)
    public void SwitchDeathTarget(Transform enemyDeathTransform)
    {
        // Save the last killed enemy and find a new target 1 second later
        _lastKilledEnemy = enemyDeathTransform;
        Invoke("FindNewTarget", 1.0f);
    }

    public void FindNewTarget()
    {
        // Only find a new target if the player is still locked onto the dead enemy
        if (_lastKilledEnemy == targetEnemy)
        {
            // Switch targets
            if (currentEnemies.Count > 0)
            {
                GameManager.instance.cameraControl.LockOn();
            }
            // Exit lockon
            else
            {
                GameManager.instance.cameraControl.ToggleLockOn();
            }
        }
    }

    public void SetIsHeavyCharging(bool bIsHeavyCharging)
    {
        _bIsHeavyCharging = bIsHeavyCharging;
    }
}
