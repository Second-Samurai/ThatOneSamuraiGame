using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class AddToTracker : MonoBehaviour
{
    private AISystem _aiSystem;
    private EnemyTracker _enemyTracker;

    private void Start()
    {
        _aiSystem = GetComponentInParent<AISystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _enemyTracker == null)
        {
            _enemyTracker = GameManager.instance.enemyTracker;
            _enemyTracker.AddEnemy(GetComponentInParent<Rigidbody>().gameObject.transform);
                
            _aiSystem.OnApproachPlayer();
        }
    }
}
