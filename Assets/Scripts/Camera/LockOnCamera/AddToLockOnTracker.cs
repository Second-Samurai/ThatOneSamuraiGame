using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class AddToLockOnTracker : MonoBehaviour
{
    private LockOnTracker _lockOnTracker;
    private LineRenderer _lineRenderer;

    private LayerMask _layerMask;

    private Transform _cameraTransform;

    //Offset since the target point is at the enemy's feet
    private Vector3 _offset = Vector3.up * 2;

    private void Start()
    {
        _lockOnTracker = GameManager.instance.lockOnTracker;
        
        _cameraTransform = Camera.main.transform;

        _layerMask = GetLayerMask();

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.widthMultiplier = 0.2f;
    }

    private void Update()
    {
        if (_lockOnTracker.currentEnemies.Count == 0)
        {
            _lineRenderer.enabled = false;
            return;
        }
        
        RaycastHit hit;

        foreach (var enemyTransform in _lockOnTracker.currentEnemies)
        {
            Vector3 rayStartPos = _cameraTransform.position;

            if (Physics.Raycast(rayStartPos, (enemyTransform.position - rayStartPos).normalized, out hit, _layerMask))
            {
                _lineRenderer.enabled = true;
                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("Success: " + hit.collider.gameObject.name);
                    _lineRenderer.startColor = Color.white;
                }
                else
                {
                    Debug.Log(hit.collider.gameObject.name);
                    _lineRenderer.startColor = Color.black;
                }
                _lineRenderer.SetPosition(0, rayStartPos);
                _lineRenderer.SetPosition(1, enemyTransform.position + _offset);
            }
        }
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

    private LayerMask GetLayerMask()
    {
        int defaultLayer = 1 << LayerMask.NameToLayer("Default");
        int navMeshPathableLayer = 1 << LayerMask.NameToLayer("NavMeshPathable");
        int structuresLayer = 1 << LayerMask.NameToLayer("Structures");
        int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        LayerMask layerMask = defaultLayer | navMeshPathableLayer | structuresLayer | enemyLayer;
        
        Debug.Log("Layermask: " + layerMask.value);

        return layerMask;
    }
}
