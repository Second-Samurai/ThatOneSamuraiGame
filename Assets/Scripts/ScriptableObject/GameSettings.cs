﻿using UnityEngine;

[CreateAssetMenu (menuName = "Settings/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Settings")]
    public PlayerSettings playerSettings;
    public EnemySettings enemySettings;
    public InputSettings inputSettings;

    [Header("Camera Objects")]
    public GameObject thirdPersonViewCam;
    public GameObject mainCamera;
    public GameObject dayPostProcessing;

    [Header("Game Prefabs")]
    public GameObject playerPrefab;
    public GameObject mainPlayerPrefab; //NEW ONE
    public GameObject enemyManagerPrefab;
    public GameObject targetHolderPrefab;

    [Header("Sword Effects")]
    public GameObject swordSlash01;

    [Space]
    public GameObject largeParry;
    public GameObject smallParry;

    [Header("Spark Effects")]
    public GameObject slashImpact01;

    [Space]
    public GameObject sparkFallOff01;

    [Header("UI Prefabs")]
    public GameObject guardCanvasPrefab;

    [Space]
    public GameObject guardMeterPrefab;

    [Header("RewindManager")]
    public GameObject rewindManager;

    [Header("AudioManger")]
    public GameObject audioManger;

    [Header("Weapon Prefabs")]
    public GameObject katanaPrefab;
    public GameObject laserSword; // He he he

}
