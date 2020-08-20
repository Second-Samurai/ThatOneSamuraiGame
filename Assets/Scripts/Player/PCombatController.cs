using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Empty For now
public interface IPlayerCombat {

}

public class PCombatController : MonoBehaviour, IPlayerCombat
{
    PlayerInput playerInput;

    public void Init(PlayerInput playerInput) {
        this.playerInput = playerInput;
    }

    
}
