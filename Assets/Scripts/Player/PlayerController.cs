using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public interface IPlayerController {
    string GetStringID();
    StatHandler GetPlayerStats();
}

public class PlayerController : MonoBehaviour, IPlayerController
{
    public string playerID = "defaultID1234";
    [HideInInspector] public StatHandler playerStats;
    [HideInInspector] public PlayerSettings playerSettings;
    [HideInInspector] public PlayerStateMachine stateMachine;

    [HideInInspector] public CameraControl cameraController;

    //NOTE: Once object is spawned through code init through awake instead.
    void Awake() {
        //NOTE: This is only temporary to fix the camera referencing issues
        GameManager.instance.playerController = this;
    }

    //Summary: Sets initial state and initialise variables
    //
    void Start()
    {
        GameManager gameManager = GameManager.instance;
        playerSettings = gameManager.gameSettings.playerSettings;
        EntityStatData playerData = playerSettings.platerStats;

        playerStats = new StatHandler();
        playerStats.Init(playerData);

        stateMachine = this.gameObject.AddComponent<PlayerStateMachine>();

        cameraController = this.GetComponent<CameraControl>();
        cameraController.player = this.transform;
        //cameraController.unlockedCam = gameManager.thirdPersonCamera;

        //This assigns the thirdperson camera targets to this player
        CinemachineFreeLook freeLockCamera = gameManager.thirdPersonCamera.GetComponent<CinemachineFreeLook>();
        freeLockCamera.Follow = this.transform;
        freeLockCamera.LookAt = this.transform;

        SetState<PNormalState>();
    }

    //Summary: Clears and Sets the new specified state for player.
    public void SetState<T>() where T : PlayerState 
    {
        stateMachine.AddState<T>();
    }

    public string GetStringID() {
        return playerID;
    }

    public StatHandler GetPlayerStats()
    {
        return playerStats;
    }
}
