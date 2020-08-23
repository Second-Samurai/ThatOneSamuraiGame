using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Empty For now
public interface IPlayerCombat {
    void RunLightAttack();
    void BlockCombatInputs();
    void UnblockCombatInputs();
}

public class PCombatController : MonoBehaviour, IPlayerCombat
{
    private StatHandler _playerStats;
    private Animator _animator;
    private float _chargeTime;
    private int _comboHits;

    private bool _isInputBlocked = false;

    public void Init(StatHandler playerStats) {
        this._playerStats = playerStats;
        this._animator = this.GetComponent<Animator>();
    }

    public void RunLightAttack()
    {
        if (_isInputBlocked) return;

        _comboHits++;
        _comboHits = Mathf.Clamp(_comboHits, 0, 4);
        _chargeTime = 0;

        _animator.SetTrigger("AttackLight");
        _animator.SetInteger("ComboAttacks", _comboHits);
    }

    private void HeavyAttack()
    {
        if (_isInputBlocked) return;
    }

    //Summary: Resets the AttackCombo after 'Animation Event' has finished.
    public void ResetAttackCombo() {
        _animator.ResetTrigger("AttackLight");
        _comboHits = 0;
    }

    public void BlockCombatInputs()
    {
        _isInputBlocked = true;
    }

    public void UnblockCombatInputs()
    {
        _isInputBlocked = false;
    }
}
