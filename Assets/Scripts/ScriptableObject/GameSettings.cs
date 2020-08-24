using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Settings/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Settings")]
    public PlayerSettings playerSettings;

    [Header("Camera Objects")]
    public GameObject thirdPersonViewCam;
    public GameObject mainCamera;

    [Header("Game Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyManagerPrefab;
}
