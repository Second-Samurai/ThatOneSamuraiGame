using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Base;
using UnityEngine;

public class LockOnTargetTracking : PausableMonoBehaviour
{

    #region - - - - - - Fields - - - - - -
    
    public LayerMask m_TargetableLayers;

    // Target Tracking Fields
    public List<Transform> m_PossibleTargets;
    public List<Transform> m_ValidTargetableEnemies;
    private Transform m_LastKilledTarget;

    // Runtime Fields
    private Transform m_CameraTransform;
    private Vector3 m_EnemyFeetOffset = Vector3.up * 2; //Offset since the target point is at the enemy's feet
    private Vector3 m_RaycastStartPosition;
    private float m_MaxRaycastDistance = 10f;

    #endregion Fields

    #region - - - - - - Initializers - - - - - -

    public void Initialise()
    {
        this.m_MaxRaycastDistance = this.GetComponent<SphereCollider>().radius * 2;
        this.m_CameraTransform = GameManager.instance.MainCamera.transform;

        ILockOnObserver _LockOnObserver = this.GetComponent<ILockOnObserver>();
        _LockOnObserver.OnRemoveLockOnTarget.AddListener(this.RemoveTarget);
    }

    #endregion Initializers

    #region - - - - - - Unity Methods - - - - - -

    private void FixedUpdate()
    {
        if (this.IsPaused || !this.m_CameraTransform) return;
        
        // Constantly searches for the nearby enemy.
        this.CollectTargetableEnemies();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            this.m_PossibleTargets.Add(other.transform);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")) 
            this.RemoveTarget(other.transform);
    }

    #endregion Methods
  
    #region - - - - - - Unity Methods - - - - - -

    public void ClearTargets()
    {
        this.m_ValidTargetableEnemies.Clear();
        this.m_PossibleTargets.Clear();
    }

    private void RemoveTarget(Transform targetToRemove)
    {
        this.m_ValidTargetableEnemies.Remove(targetToRemove);
        this.m_PossibleTargets.Remove(targetToRemove);
        
        this.m_PossibleTargets.TrimExcess();
        this.m_ValidTargetableEnemies.TrimExcess();
    }

    /* Observations of this class:
     *  - The targetable enemies need to be bigger than 2 units vertically in order to be physically detected
     *  - Only observes whatever is within a target radius determined by the sphere collider used for detection
     *  - The starting point is from the camera's position
     */
    private void CollectTargetableEnemies()
    {
        foreach (var enemyTransform in this.m_PossibleTargets)
        {
            this.m_RaycastStartPosition = this.m_CameraTransform.position;
            
            // Verifies that there are targetable enemies withing a set radius
            if (!Physics.Raycast(
                    origin: this.m_RaycastStartPosition,
                    direction: (enemyTransform.position + this.m_EnemyFeetOffset - this.m_RaycastStartPosition)
                    .normalized,
                    hitInfo: out RaycastHit _Hit,
                    maxDistance: m_MaxRaycastDistance,
                    layerMask: this.m_TargetableLayers))
            {
                // GameLogger.Log(
                //     (nameof(m_RaycastStartPosition), m_RaycastStartPosition),
                //     ("Direction: ", (enemyTransform.position + this.m_EnemyFeetOffset - this.m_RaycastStartPosition).normalized),
                //     ("Hit Info: ", _Hit),
                //     ("Max Distance: ", m_MaxRaycastDistance),
                //     ("Layer Mask: ", m_TargetableLayers.value));
                
                // TODO: Remove this
                Debug.DrawLine(m_RaycastStartPosition, (enemyTransform.position + this.m_EnemyFeetOffset - this.m_RaycastStartPosition).normalized * m_MaxRaycastDistance, Color.yellow);
                
                return;
            }
            
            if (_Hit.collider.CompareTag("Enemy"))
            {
                // TODO: Remove this
                Debug.DrawLine(m_RaycastStartPosition, (enemyTransform.position + this.m_EnemyFeetOffset - this.m_RaycastStartPosition).normalized * m_MaxRaycastDistance, Color.green);
                
                this.AddToTargetableEnemies(enemyTransform);
            }
            else
            {
                // TODO: Remove this
                Debug.DrawLine(m_RaycastStartPosition, (enemyTransform.position + this.m_EnemyFeetOffset - this.m_RaycastStartPosition).normalized * m_MaxRaycastDistance, Color.red);
                
                this.m_ValidTargetableEnemies.Remove(enemyTransform);
            }
        }
    }
    
    private void AddToTargetableEnemies(Transform enemy)
    {
        if (!this.m_ValidTargetableEnemies.Contains(enemy))
            this.m_ValidTargetableEnemies.Add(enemy);
    }
    
    #endregion Methods

}
