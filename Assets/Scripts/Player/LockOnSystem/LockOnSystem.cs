using Enemies;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

// TODO: Refactor to seperate file
public interface ILockOnSystem
{

    #region - - - - - - Methods - - - - - -

    void RemoveTargetFromTracking(Transform targetToRemove); //TODO replace all instances calling upon the death or removal of target from list.

    void SelectNewTarget();

    void StartLockOn();

    void EndLockOn();

    #endregion Methods

}

public class LockOnSystemInitializationData
{

    #region - - - - - - Properties - - - - - -

    public ICameraController CameraController { get; private set; }

    #endregion Properties

    #region - - - - - - Constructors - - - - - -

    public LockOnSystemInitializationData(ICameraController cameraController)
    {
        this.CameraController = cameraController;
    }

    #endregion Constructors
  
}

[RequireComponent(typeof(ILockOnObserver))]
public class LockOnSystem : PausableMonoBehaviour, ILockOnSystem, IInitialize<LockOnSystemInitializationData>
{

    #region - - - - - - Fields - - - - - -

    [RequiredField] public LockOnTargetTracking m_LockOnTargetTracker;
    [RequiredField] public Animator m_Animator; // Should not be in here as this couples with the animation system.

    private ICameraController m_CameraController;
    private ILockOnObserver m_LockOnObserver;
    private Transform m_TargetTransform;
    private AISystem m_EnemyAISystem; // Maintained from before but should not be coupled.

    private bool m_IsLockedOn;

    #endregion Fields

    #region - - - - - - Initializers - - - - - -

    public void Initialize(LockOnSystemInitializationData initializationData)
    {
        this.m_CameraController = initializationData.CameraController;
        
        this.m_LockOnObserver = this.GetComponent<ILockOnObserver>();
        this.m_LockOnTargetTracker.Initialise();
        
        this.m_LockOnObserver.OnLockOnDisable.AddListener(this.EndLockOn);
    }

    #endregion Initializers

    #region - - - - - - Methods - - - - - -
    
    public void StartLockOn()
    {
        this.m_IsLockedOn = true;
        this.m_Animator.SetBool("LockedOn", true);
        
        // Set target to camera
        Transform _TargetTransform = this.GetNearestTarget();
        if (!_TargetTransform) return;

        this.m_LockOnObserver.OnNewLockOnTarget.Invoke(_TargetTransform);
        this.m_LockOnObserver.OnLockOnEnable.Invoke();
        
        // // TODO: Remove the lockOn camera to instead use the observer
        // this.m_LockOnCamera.SetLockOnTarget(_TargetTransform);
        // this.m_LockOnCamera.SetFollowedTransform(this.m_FollowTransform);
        
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
        this.m_LockOnTargetTracker.ClearTargets();
    }

    private Transform GetNearestTarget()
    {
        // Initialise variables
        float _ClosestDistance = Mathf.Infinity;
        Transform _NextEnemy = null;

        GameLogger.Log(
            ("Possible Targets: ", this.m_LockOnTargetTracker.m_PossibleTargets.Count),
            ("Valid Targets: ", this.m_LockOnTargetTracker.m_ValidTargetableEnemies.Count));
        
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

            // If the target enemy is not the same as the referred enemy then change target.
            if (_NextEnemy != this.m_TargetTransform)
                this.m_TargetTransform = _NextEnemy;
            
            Debug.DrawLine(this.transform.position, (_NextEnemy.position + Vector3.up - this.transform.position).normalized * 90, Color.magenta);
        }
        
        return this.m_TargetTransform;
    }

    public void RemoveTargetFromTracking(Transform targetToRemove)
        => this.m_LockOnTargetTracker.RemoveTarget(targetToRemove);

    #endregion Methods

}
