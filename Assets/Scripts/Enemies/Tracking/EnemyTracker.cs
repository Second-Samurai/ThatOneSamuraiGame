using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ENEMY TRACKER INFO
// Used to track all enemies in a scene through a list of transforms
// This list is obtained by searching for enemy tags on awake

public class EnemyTracker : MonoBehaviour
{
    public List<Transform> currentEnemies;

    public void AddEnemy(Transform enemy)
    {
        if (!currentEnemies.Contains(enemy))
        {
            currentEnemies.Add(enemy);
        }
    }

    public void RemoveEnemy(Transform enemy)
    {
        if (currentEnemies.Contains(enemy))
        {
            currentEnemies.Remove(enemy);
            currentEnemies.TrimExcess();
        }
    }

    private void Awake()
    {
        // GameObject[] addEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        // foreach (GameObject enemy in addEnemies)
        // {
        //     currentEnemies.Add(enemy.transform);
        // }
    }
}
