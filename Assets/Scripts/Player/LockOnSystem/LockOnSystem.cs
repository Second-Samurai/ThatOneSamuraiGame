﻿using System;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

public class LockOnSystemInitializationData
{

    #region - - - - - - Properties - - - - - -

    public ICameraController CameraController { get; set; }
    
    public IPlayerAnimationDispatcher AnimationDispatcher { get; set; }

    #endregion Properties

}

/// <summary>
/// Responsible for managing the Player's LockOn behaviour.
/// </summary>
[RequireComponent(typeof(ILockOnObserver))]
public class LockOnSystem : 
    PausableMonoBehaviour, 
    ILockOnSystem, 
    IInitialize<LockOnSystemInitializationData>,
    IDebuggable
{

    #region - - - - - - Fields - - - - - -

    [RequiredField] 
    [SerializeField]
    private LockOnTargetTracking m_LockOnTargetTracker;

    // Required components
    private IPlayerAnimationDispatcher m_AnimationDispatcher;
    private ICameraController m_CameraController;
    private ILockOnObserver m_LockOnObserver;

    private GameObject m_TargetEnemy; 
    private Transform m_TargetTransform;

    private bool m_IsLockedOn;

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public bool IsLockingOnTarget
        => this.m_IsLockedOn;

    #endregion Properties

    #region - - - - - - Initializers - - - - - -

    public void Initialize(LockOnSystemInitializationData initializationData)
    {
        this.m_AnimationDispatcher = initializationData.AnimationDispatcher;
        this.m_CameraController = initializationData.CameraController;

        this.m_LockOnObserver = this.GetComponent<ILockOnObserver>();
        this.m_LockOnTargetTracker.Initialise();
    }

    #endregion Initializers

    #region - - - - - - Methods - - - - - -

    GameObject ILockOnSystem.GetCurrentTarget()
        => this.m_TargetTransform.gameObject;

    public void StartLockOn()
    {
        this.m_IsLockedOn = true;
        this.m_AnimationDispatcher.Dispatch(PlayerAnimationEventStates.StartLockOn);

        // Set target to camera
        Transform _TargetTransform = this.GetNearestTarget();
        if (!_TargetTransform) return;

        this.m_LockOnObserver.OnNewLockOnTarget.Invoke(_TargetTransform);
        this.m_LockOnObserver.OnLockOnEnable.Invoke();
        
        this.m_CameraController.SelectCamera(SceneCameras.LockOn);

        // ******************************************************
        // Logic below is commented out as the Enemy objects will be reworked entirely
        // ******************************************************
        
        // this.m_CameraController.SelectCamera(SceneCameras.LockOn);

        // this.m_EnemyAISystem = _TargetTransform.GetComponent<AISystem>();
        // if (this.m_EnemyAISystem != null)
        // {
        //     // Enable the guard meter
        //     this.m_EnemyAISystem.eDamageController.enemyGuard.EnableGuardMeter();
        //     // Set the guard meter to visible through this event
        //     this.m_EnemyAISystem.eDamageController.enemyGuard.OnGuardEvent.Invoke();
        // }
    }

    public void SelectNewTarget()
    {
        if (!this.m_IsLockedOn) return;

        Transform _TargetTransform = this.GetNearestTarget();
        if (!_TargetTransform) return;

        this.m_LockOnObserver.OnNewLockOnTarget.Invoke(_TargetTransform);
    }

    public void EndLockOn()
    {
        if (!this.m_IsLockedOn) return;

        this.m_LockOnObserver.OnLockOnDisable.Invoke();
        
        this.m_IsLockedOn = false;
        this.m_CameraController.SelectCamera(SceneCameras.FollowPlayer);
        this.m_AnimationDispatcher.Dispatch(PlayerAnimationEventStates.EndLockOn);
        this.m_LockOnTargetTracker.ClearTargets();
    }

    private Transform GetNearestTarget()
    {
        // Initialise variables
        float _ClosestDistance = Mathf.Infinity;
        Transform _NextEnemy = null;

        // GameLogger.Log(
        //     ("Possible Targets: ", this.m_LockOnTargetTracker.m_PossibleTargets.Count),
        //     ("Valid Targets: ", this.m_LockOnTargetTracker.m_ValidTargetableEnemies.Count));
        
        if (this.m_LockOnTargetTracker.m_ValidTargetableEnemies.Count > 0)
        {
            foreach (Transform enemy in this.m_LockOnTargetTracker.m_ValidTargetableEnemies)
            {
                // Finds the closest enemy
                float distance = Vector3.Distance(this.transform.position, enemy.position);
                if (distance < _ClosestDistance && enemy != this.m_TargetTransform)
                {
                    _ClosestDistance = distance;
                    _NextEnemy = enemy;
                }

                // If the closest enemy is not null then set the value
                if (_NextEnemy == null && this.m_TargetTransform != null)
                    _NextEnemy = this.m_TargetTransform;
            }

            if (_NextEnemy == null)
            {
                GameLogger.Log("No Enemies to lock to were found nearby.");
                return null;
            }

            // If the target enemy is not the same as the referred enemy then change target.
            if (_NextEnemy != this.m_TargetTransform)
            {
                // This is a hack to ensure that locking behaviour always 'looks' to its appropriate transform instead of its feet.
                IPreferredLockingTransformProvider _PreferredTransformProvider =
                    _NextEnemy.GetComponent<IPreferredLockingTransformProvider>();
                this.m_TargetEnemy = _NextEnemy.gameObject;
                this.m_TargetTransform = _PreferredTransformProvider.GetPreferredTransform();
            }
            
            Debug.DrawLine(this.transform.position, (_NextEnemy.position + Vector3.up - this.transform.position).normalized * 90, Color.magenta);
        }
        
        return this.m_TargetTransform;
    }

    #endregion Methods

    #region - - - - - - Debugging Methods - - - - - -

    void IDebuggable.DebugInvoke()
    {
    }

    object IDebuggable.GetDebugInfo() 
        => new DebuggingLockOnSystemInfo { TargetEnemy = this.m_TargetEnemy };

    #endregion Debugging Methods
  
}

public class DebuggingLockOnSystemInfo
{

    #region - - - - - - Properties - - - - - -

    public GameObject TargetEnemy { get; set; }
    
    #endregion Properties
  
}