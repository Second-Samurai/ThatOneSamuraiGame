using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class LockOnTracker : MonoBehaviour
{
    public List<Transform> currentEnemies;
    public Transform targetEnemy;
    public AISystem targetEnemyAISystem;
    
    private CameraControl _cameraControl;

    private Transform _lastKilledEnemy;

    #region Enemy List Adding/Subtracting

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

    #endregion

    #region Lock On Camera Targeting

    public void SetTarget(Transform newTargetEnemy)
    {
        targetEnemy = newTargetEnemy;
        targetEnemyAISystem = targetEnemy.GetComponent<AISystem>();

        // Stop here if there is no AI system (i.e. archers)
        if (targetEnemyAISystem == null) return;
        
        // Enable the guard meter
        targetEnemyAISystem.eDamageController.enemyGuard.EnableGuardMeter();
        // Set the guard meter to visible through this event
        targetEnemyAISystem.eDamageController.enemyGuard.OnGuardEvent.Invoke();
    }
    
    public void ClearTarget()
    {
        targetEnemy = null;
        targetEnemyAISystem = null;
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

    #endregion
    

}
