using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class GameData
{
    public static int currentCheckpoint = 0;
    public static Dictionary<string, bool> EnemyList;
    public static bool bLoaded = false;

    public static SaveData ReturnSaveData()
    {
        SaveData saveData = new SaveData(currentCheckpoint, EnemyList);
        return saveData;
    }

    public static void LoadData(SaveData data)
    {
        currentCheckpoint = data.currentCheckpoint;
        EnemyList = data.EnemyList;
        bLoaded = true;
    }

    [System.Serializable]
    public class SaveData
    {
        public int currentCheckpoint = 0;
        public Dictionary<string, bool> EnemyList;

        public SaveData(int i, Dictionary<string, bool> d)
        {
            currentCheckpoint = i;
            EnemyList = d;
        }
    }

}
