using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class AddToLockOnTracker : MonoBehaviour
{
    private LockOnTracker _lockOnTracker;
    private Transform _cameraTransform;

    public LayerMask layerMask;
    public float raycastMaxDist;
    
    //Offset since the target point is at the enemy's feet
    private Vector3 _offset = Vector3.up * 2;

    public GameEvent CancelLockOnEvent;

    private void Start()
    {
        raycastMaxDist = GetComponent<SphereCollider>().radius * 2;

        _lockOnTracker = GameManager.instance.lockOnTracker;
        
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        RaycastToCurrentEnemies();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _lockOnTracker.AddEnemy(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _lockOnTracker.RemoveEnemy(other.transform);
        }
    }

    private void RaycastToCurrentEnemies()
    {
        foreach (var enemyTransform in _lockOnTracker.currentEnemies)
        {
            RaycastHit hit;
            Vector3 rayStartPos = _cameraTransform.position;
            
            if (Physics.Raycast(rayStartPos, (enemyTransform.position + _offset - rayStartPos).normalized, out hit, raycastMaxDist, layerMask))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.DrawRay(rayStartPos, enemyTransform.position + _offset - rayStartPos, Color.green);
                    
                    AddToTargetableEnemies(enemyTransform);
                }
                else
                {
                    Debug.DrawRay(rayStartPos, enemyTransform.position + _offset - rayStartPos, Color.red);

                    _lockOnTracker.targetableEnemies.Remove(enemyTransform);

                    if (enemyTransform == _lockOnTracker.targetEnemy)
                    {
                       // CancelLockOnEvent.Raise();
                    }
                }
            }
        }
    }

    private void AddToTargetableEnemies(Transform enemy)
    {
        if (!_lockOnTracker.targetableEnemies.Contains(enemy))
        {
            _lockOnTracker.targetableEnemies.Add(enemy);
        }
    }
}
