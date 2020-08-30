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
    public AttackChainTracker comboTracker;
    private PSword _playerSword;
    private float _chargeTime;
    private int _comboHits;

    private bool _isInputBlocked = false;
    public bool _isAttacking = false;
    private PlayerInput _playerInput;
    private PlayerFunctions _functions;
    public Collider attackCol;

    public void Init(StatHandler playerStats) {
        this._playerStats = playerStats;
        this._animator = this.GetComponent<Animator>();
        comboTracker = GetComponent<AttackChainTracker>();

        _playerSword = this.GetComponentInChildren<PSword>();
        _playerSword.SetParentTransform(this.gameObject.transform);
        _playerInput = GetComponent<PlayerInput>();
        _functions = GetComponent<PlayerFunctions>();
        attackCol = GetComponentInChildren<BoxCollider>();
    }
     

    public void RunLightAttack()
    {
        if (_isInputBlocked) return;

        _comboHits++;
        _comboHits = Mathf.Clamp(_comboHits, 0, 4);
        _chargeTime = 0;
        if (!_isAttacking)
        {
            comboTracker.RegisterInput();
            _animator.SetTrigger("AttackLight");
        }
        _animator.SetInteger("ComboCount", _comboHits);

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

    //Summary: Enabled collision detection to apply damage to hit target.
    //
    public void BeginAttacking()
    {
        _isAttacking = true;
        _functions.DisableBlock();
        attackCol.enabled = true;
    }

    //Summary: Calls the sword's Slash creation func triggered by animation event.
    //
    public void BeginSwordEffect(float slashAngle)
    {
        _playerSword.CreateSlashEffect(slashAngle);
    }

    //Summary: Disables the detection of the sword.
    //
    public void EndAttacking()
    {
        _isAttacking = false;
        _functions.EnableBlock();
        attackCol.enabled = false;
    }

    private void DetectCollision()
    {

    }

    private float CalculateDamage()
    {
        return _playerStats.baseDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isAttacking) return;
        if (other.GetComponent<IPlayerController>() != null) return;

        IDamageable attackEntity = other.GetComponent<IDamageable>();
        if (attackEntity == null) return;

        attackEntity.OnEntityDamage(CalculateDamage(), this.gameObject);
    }
}
