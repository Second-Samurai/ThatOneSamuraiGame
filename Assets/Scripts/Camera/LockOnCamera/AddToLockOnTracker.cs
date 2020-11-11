using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class AddToLockOnTracker : MonoBehaviour
{
    private LockOnTracker _lockOnTracker;

    private void Start()
    {
        _lockOnTracker = GameManager.instance.lockOnTracker;
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
