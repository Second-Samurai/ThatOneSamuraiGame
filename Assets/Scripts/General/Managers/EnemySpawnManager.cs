using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public List<EnemySpawnCheck> enemies;
    public Dictionary<string, bool> enemySpawnDictionary;

    private void Awake()
    {
        enemySpawnDictionary = new Dictionary<string, bool>();
        CheckIfDataExists();
    }

    

    public void CheckIfDataExists()
    {
        if (GameData.bLoaded)
        {
            LoadEnemyList();
            SetSpawnValue();
            
            Debug.Log(enemySpawnDictionary.Values);
        }
        else Debug.LogError("NO DATA TO SPAWN");
    }

    public void SetSpawnValue()
    {
        foreach(EnemySpawnCheck enemy in enemies)
        {
            enemy.CheckIfSpawned();
        }
    }

    public void SaveEnemyList()
    {
        foreach(EnemySpawnCheck enemy in enemies)
        {
            enemySpawnDictionary[enemy.myName] = enemy.GetValue();
        }
        GameData.EnemyList = enemySpawnDictionary;
        foreach (KeyValuePair<string, bool> val in GameData.EnemyList)
        {
            Debug.LogWarning(val.Key + val.Value.ToString());
        }
    }

    public void LoadEnemyList()
    {
        enemySpawnDictionary = GameData.EnemyList;
    }
}
