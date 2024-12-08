using Enemies;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

public interface ILockOnSystem
{

    #region - - - - - - Methods - - - - - -

    void StartLockOn();

    void EndLockOn();

    #endregion Methods

}

public class LockOnSystem : PausableMonoBehaviour, ILockOnSystem
{

    #region - - - - - - Fields - - - - - -

    public LockOnTargetTracking m_LockOnTargetTracker;
    
    private ICameraController m_CameraController;
    private ILockOnCamera m_LockOnCamera;
    private Animator m_Animator; // Should not be in here as this couples with the animation system.
    private Transform m_TargetTransform;
    private AISystem m_EnemyAISystem; // Maintained from before but should not be coupled.

    private bool m_IsLockedOn;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_LockOnCamera = this.m_CameraController.GetCamera(SceneCameras.LockOn).GetComponent<ILockOnCamera>();
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -
    
    public void StartLockOn()
    {
        if (!GameValidator.NotNull(this.m_TargetTransform, nameof(this.m_TargetTransform))) return;

        this.m_IsLockedOn = true;
        this.m_Animator.SetBool("LockedOn", true);
        
        // Find Target
        Transform _TargetTransform = this.GetNearestTarget();
        
        this.m_EnemyAISystem = _TargetTransform.GetComponent<AISystem>();
        if (this.m_EnemyAISystem != null)
        {
            // Enable the guard meter
            this.m_EnemyAISystem.eDamageController.enemyGuard.EnableGuardMeter();
            // Set the guard meter to visible through this event
            this.m_EnemyAISystem.eDamageController.enemyGuard.OnGuardEvent.Invoke();
        }
        
        // Set target to camera
        this.m_LockOnCamera.SetLockOnTarget(_TargetTransform);
        this.m_CameraController.SelectCamera(SceneCameras.LockOn);
    }

    public void EndLockOn()
    {
        if (!this.m_IsLockedOn) return;
        
        this.m_IsLockedOn = false;
        this.m_CameraController.SelectCamera(SceneCameras.FollowPlayer);
        
        this.ResetLockOnSystem();
    }

    // TODO: There needs to be a case when 
    private Transform GetNearestTarget()
    {
        // Initialise variables
        float _ClosestDistance = Mathf.Infinity;
        Transform _NextEnemy = null;

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
        }
        
        return this.m_TargetTransform;
    }

    private void ResetLockOnSystem()
    {
        
    }

    #endregion Methods
  
}
