using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerController {

}

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public StatHandler playerStats;
    [HideInInspector] public PlayerSettings playerSettings;
    [HideInInspector] public PlayerStateMachine stateMachine;

    void Start() {
        playerSettings = GameManager.instance.gameSettings.playerSettings;
        EntityStatData playerData = playerSettings.platerStats;

        PlayerInput playerInput = this.GetComponent<PlayerInput>();
        playerStats = new StatHandler();
        playerStats.Init(playerData);

        stateMachine = this.gameObject.AddComponent<PlayerStateMachine>();

        PCombatController combatController = this.gameObject.AddComponent<PCombatController>();
        combatController.Init(playerInput);

        PDamageController damageController = this.gameObject.AddComponent<PDamageController>();
        damageController.Init(playerStats);
    }

    public void SetState<T>() where T : PlayerState 
    {
        stateMachine.AddState<T>();
    }
}
