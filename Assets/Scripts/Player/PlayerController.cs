using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerController {

}

public class PlayerController : MonoBehaviour
{
    public StatHandler playerStats;
    public PlayerStateMachine stateMachine;

    void Awake() 
    {
        PCombatController combatController = this.gameObject.AddComponent<PCombatController>();
        combatController.Init();

        PDamageController damageController = this.gameObject.AddComponent<PDamageController>();
        damageController.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetState<T>() where T : PlayerState 
    {
        stateMachine.AddState<T>();
    }
}
