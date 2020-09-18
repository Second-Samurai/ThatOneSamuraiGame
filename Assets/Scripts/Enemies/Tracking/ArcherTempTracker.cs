using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTempTracker : MonoBehaviour
{
    EnemyTracker _enemyTracker;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _enemyTracker == null)
        {
            _enemyTracker = GameManager.instance.enemyTracker;
            _enemyTracker.AddEnemy(GetComponentInParent<Rigidbody>().gameObject.transform, false);
             
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _enemyTracker == null)
        {
            _enemyTracker = GameManager.instance.enemyTracker;
            _enemyTracker.RemoveEnemy(GetComponentInParent<Rigidbody>().gameObject.transform, false);

        }
        else if (other.CompareTag("Player"))
        {
            Debug.LogError("REMOVED");
            _enemyTracker.RemoveEnemy(GetComponentInParent<Rigidbody>().gameObject.transform);
        }
    }
}
