using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Base;
using UnityEngine;

public class LockOnTargetTracking : PausableMonoBehaviour
{

    #region - - - - - - Fields - - - - - -
    
    public LayerMask m_TargetableLayers;

    private float m_MaxRaycastDistance = 10f;

    public List<Transform> m_PossibleTargets;
    public List<Transform> m_ValidTargetableEnemies;
    private Transform m_LastKilledTarget;

    private Transform m_CameraTransform;
    private Vector3 m_EnemyFeetOffset = Vector3.up * 2; //Offset since the target point is at the enemy's feet
    private Vector3 m_RaycastStartPosition;

    #endregion Fields

    #region - - - - - - Initializers - - - - - -

    public void Initialise()
    {
        this.m_MaxRaycastDistance = this.GetComponent<SphereCollider>().radius * 2;
        this.m_CameraTransform = GameManager.instance.MainCamera.transform;
    }

    #endregion Initializers
  
    #region - - - - - - Unity Event Handlers - - - - - -

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

    #endregion Unity Event Handlers

    #region - - - - - - Methods - - - - - -

    private void Update()
    {
        if (this.IsPaused || !this.m_CameraTransform) return;

        // Constantly searches for the nearby enemy.
        this.CollectTargetableEnemies();
    }

    #endregion Methods
  
    #region - - - - - - Methods - - - - - -

    public void RemoveTarget(Transform targetToRemove)
    {
        this.m_ValidTargetableEnemies.Remove(targetToRemove);
        this.m_PossibleTargets.Remove(targetToRemove);
        
        this.m_PossibleTargets.TrimExcess();
        this.m_ValidTargetableEnemies.TrimExcess();
    }

    private void CollectTargetableEnemies()
    {
        foreach (var enemyTransform in this.m_ValidTargetableEnemies)
        {
            this.m_RaycastStartPosition = this.m_CameraTransform.position;

            if (!Physics.Raycast(
                    origin: this.m_RaycastStartPosition,
                    direction: (enemyTransform.position + this.m_EnemyFeetOffset - this.m_RaycastStartPosition).normalized, 
                    hitInfo: out RaycastHit _Hit,
                    maxDistance: m_MaxRaycastDistance, 
                    layerMask: this.m_TargetableLayers))
                return;
            
            if (_Hit.collider.CompareTag("Enemy"))
            {
                // TODO: Remove this
                Debug.DrawRay(this.m_RaycastStartPosition, enemyTransform.position + this.m_EnemyFeetOffset - this.m_RaycastStartPosition, Color.green);
                
                this.AddToTargetableEnemies(enemyTransform);
            }
            else
            {
                // TODO: Remove this
                Debug.DrawRay(this.m_RaycastStartPosition, enemyTransform.position + this.m_EnemyFeetOffset - this.m_RaycastStartPosition, Color.red);
                
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
