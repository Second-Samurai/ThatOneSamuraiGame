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
        if (other.CompareTag("Player") && !_aiSystem.bIsDead)
        {
            _enemyTracker = GameManager.instance.enemyTracker;
            _enemyTracker.AddEnemy(GetComponentInParent<Rigidbody>().gameObject.transform);

            if (_aiSystem.bIsIdle) 
            {
                //Debug.Log("Added " + _aiSystem.name + " to tracker");
                _aiSystem.bIsIdle = false;
                _aiSystem.OnApproachPlayer();
                if (_aiSystem.enemyType == EnemyType.BOSS) 
                {
                    _aiSystem.bossAggro.Raise();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _enemyTracker = GameManager.instance.enemyTracker;
            _enemyTracker.RemoveEnemy(GetComponentInParent<Rigidbody>().gameObject.transform); 
        }
    }


}
