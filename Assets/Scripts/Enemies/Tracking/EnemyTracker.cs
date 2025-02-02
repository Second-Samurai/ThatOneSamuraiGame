using System;
using System.Collections.Generic;
using Enemies;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;
using Random = UnityEngine.Random;

// ENEMY TRACKER INFO
// Used to track all enemies in a scene through a list of transforms
// This list is obtained by searching for enemy tags on awake

public class EnemyTrackerInitializerData
{

    #region - - - - - - Properties - - - - - -

    public ILockOnSystem LockOnSystem { get; set; }

    #endregion Properties

    #region - - - - - - Constructors - - - - - -

    public EnemyTrackerInitializerData(ILockOnSystem lockOnSystem) 
        => this.LockOnSystem = lockOnSystem;

    #endregion Constructors
  
}

public class EnemyTracker : MonoBehaviour, IInitialize<EnemyTrackerInitializerData>
{
    public List<Transform> currentEnemies;

    private EnemySettings _enemySettings;
    private ILockOnSystem m_LockOnSystem;
    
    private float _impatienceMeter;
    private bool _bReduceImpatience;
    private bool _bIsHeavyCharging = false;
    bool bFadedInDrums = false;

    public bool bAtVillage = false;
    
    //Debug controls to stop enemies from approaching
    public bool bDebugStopApproaching;

    public void Initialize(EnemyTrackerInitializerData initializerData)
    {
        this.m_LockOnSystem = initializerData.LockOnSystem 
            ?? throw new ArgumentNullException(nameof(initializerData.LockOnSystem));
    }

    private void Start() 
        => _enemySettings = GameManager.instance.gameSettings.enemySettings;

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

        //if (!bFadedInDrums && currentEnemies.Count > 0 && !bAtVillage)
        //{
        //    AudioManager.instance.trackManager.DrumsFadeBetween(true);
        //    bFadedInDrums = true;
        //}
        //else if (bFadedInDrums && currentEnemies.Count == 0 && !bAtVillage)
        //{
        //    AudioManager.instance.trackManager.DrumsFadeBetween(false);
        //    bFadedInDrums = false;
        //}

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
        if (!bDebugStopApproaching)
        {
            AISystem _CurrentlyTargeteEnemy = this.m_LockOnSystem.IsLockingOnTarget 
                ? this.m_LockOnSystem.GetCurrentTarget().GetComponent<AISystem>()
                : null;
            
            // Don't pick a target if no enemies are in the tracker or if the player is charging a heavy attack
            if (currentEnemies.Count <= 0 || _bIsHeavyCharging)
                return;
        
            // int random is 0 - 9
            int targetSelector = Random.Range(0, 10);
        
            // 80% chance if there is a targetAISystem
            if(targetSelector >= 2  && _CurrentlyTargeteEnemy!= null) 
            {
                // If the target enemy isn't suitable (i.e. is not circling, stunned or dead) pick a random target instead
                if (!CheckSuitableApproachTarget(_CurrentlyTargeteEnemy))
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

    public void SetIsHeavyCharging(bool bIsHeavyCharging)
    {
        _bIsHeavyCharging = bIsHeavyCharging;
    }
}
