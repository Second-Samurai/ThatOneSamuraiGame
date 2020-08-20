using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerController {
    string GetStringID();
}

public class PlayerController : MonoBehaviour, IPlayerController
{
    public string playerID = "defaultID1234";
    [HideInInspector] public StatHandler playerStats;
    [HideInInspector] public PlayerSettings playerSettings;
    [HideInInspector] public PlayerStateMachine stateMachine;

    //NOTE: Once object is spawned through code init through awake instead.
    void Awake() {
    }

    void Start() {
        playerSettings = GameManager.instance.gameSettings.playerSettings;
        EntityStatData playerData = playerSettings.platerStats;

        PlayerInput playerInput = this.GetComponent<PlayerInput>();
        playerStats = new StatHandler();
        playerStats.Init(playerData);

        stateMachine = this.gameObject.AddComponent<PlayerStateMachine>();

        PCombatController combatController = this.GetComponent<PCombatController>();
        combatController.Init(playerStats);

        PDamageController damageController = this.GetComponent<PDamageController>();
        damageController.Init(playerStats);
    }

    //Summary: Clears and Sets the new specified state for player.
    public void SetState<T>() where T : PlayerState 
    {
        stateMachine.AddState<T>();
    }

    public string GetStringID() {
        return playerID;
    }
}
