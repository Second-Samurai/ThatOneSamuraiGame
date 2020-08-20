using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Empty For now
public interface IPlayerCombat {
    void RunLightAttack(Animator animator);
}

public class PCombatController : MonoBehaviour, IPlayerCombat
{
    private StatHandler _playerStats;
    private float _chargeTime;

    public void Init(StatHandler playerStats) {
        this._playerStats = playerStats;
    }

    public void RunLightAttack(Animator animator)
    {
        float random = Random.Range(0, 20f);

        animator.SetTrigger("AttackLight");
        animator.SetFloat("LightAttackVal", random);
        _chargeTime = 0;
    }

    private void HeavyAttack() {

    }
}
