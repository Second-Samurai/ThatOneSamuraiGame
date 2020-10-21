using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTempTracker : MonoBehaviour
{
    private BasicArcher _basicArcher;
    private EnemyTracker _enemyTracker;

    private void Start()
    {
        _basicArcher = GetComponentInParent<BasicArcher>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _basicArcher.currentState != CurrentState.Dead)
        {
            _enemyTracker = GameManager.instance.enemyTracker;
            _enemyTracker.AddEnemy(GetComponentInParent<Rigidbody>().gameObject.transform);
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
