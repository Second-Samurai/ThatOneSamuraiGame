﻿using UnityEngine;

public class TempWinTracker : MonoBehaviour
{
    public int enemyCount = 6;

    public static TempWinTracker instance;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
       // if (enemyCount <= 0) SceneManager.LoadScene("WinScene");
    }
}
