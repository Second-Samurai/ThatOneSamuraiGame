using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Settings/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Settings")]
    public PlayerSettings playerSettings;
    public EnemySettings enemySettings;

    [Header("Camera Objects")]
    public GameObject thirdPersonViewCam;
    public GameObject mainCamera;
    public GameObject dayPostProcessing;

    [Header("Game Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyManagerPrefab;
    public GameObject targetHolderPrefab;

    [Header("Sword Effects")]
    public GameObject swordSlash01;
}
