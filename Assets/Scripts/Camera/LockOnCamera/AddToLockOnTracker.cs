using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class AddToLockOnTracker : MonoBehaviour
{
    private LockOnTracker _lockOnTracker;
    private LineRenderer _lineRenderer;

    public LayerMask layerMask;
    public float _raycastMaxDist;

    private Transform _cameraTransform;

    //Offset since the target point is at the enemy's feet
    private Vector3 _offset = Vector3.up * 2;
    
    //Debug controls to show the line renderer
    [SerializeField] private bool bLineRendererDebug = false;

    private void Start()
    {
        _raycastMaxDist = GetComponent<SphereCollider>().radius * 2;

        _lockOnTracker = GameManager.instance.lockOnTracker;
        
        _cameraTransform = Camera.main.transform;

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

            if (Physics.Raycast(rayStartPos, (enemyTransform.position - rayStartPos).normalized, out hit, _raycastMaxDist, layerMask))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    
                }
                else
                {
                    
                }
                
                #region Debug for Line Rendering
                if (bLineRendererDebug)
                {
                    _lineRenderer.enabled = true;
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        Debug.Log("Success: " + hit.collider.gameObject.name);
                        _lineRenderer.startColor = Color.green;
                    }
                    else
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        _lineRenderer.startColor = Color.red;
                    }
                    _lineRenderer.SetPosition(0, rayStartPos);
                    _lineRenderer.SetPosition(1, enemyTransform.position + _offset);
                }
                #endregion
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
}
