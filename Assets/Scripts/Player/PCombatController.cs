using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Empty For now
public interface ICombatController
{
    EntityCombatType GetCombatType();

    void RunLightAttack();
    void BlockCombatInputs();
    void UnblockCombatInputs();
    void SheathSword();
    void UnsheathSword();

}

public class PCombatController : MonoBehaviour, ICombatController
{
    //Public variables
    public AttackChainTracker comboTracker;
    public Collider attackCol;
    public bool _isAttacking = false;
    public bool isUnblockable = false;

    //Private Variables
    private PlayerInput _playerInput;
    private PlayerFunctions _functions;
    private PDamageController _damageController;
    private EntityCombatType _combatType; //TODO: May need to relook at this variable
    private WSwordEffect _playerSword;
    private EntityAttackRegister _attackRegister;
    private CloseEnemyGuideControl _guideController;
    private StatHandler _playerStats;
    private Animator _animator;

    private float _chargeTime;
    private int _comboHits;
    private bool _isInputBlocked = false;

    public void Init(StatHandler playerStats) {
        this._playerStats = playerStats;
        this._animator = this.GetComponent<Animator>();
        comboTracker = GetComponent<AttackChainTracker>();

        _playerSword = this.GetComponentInChildren<WSwordEffect>();
        _playerSword.SetParentTransform(this.gameObject.transform);
        _playerInput = GetComponent<PlayerInput>();
        _damageController = GetComponent<PDamageController>();
        _functions = GetComponent<PlayerFunctions>();
        attackCol = GetComponentInChildren<BoxCollider>();

        _attackRegister = new EntityAttackRegister();
        _attackRegister.Init(this.gameObject, EntityType.Player, _playerSword);

        _guideController = new CloseEnemyGuideControl();
        _guideController.Init(this, this.gameObject.transform, this.GetComponent<Rigidbody>());
    }

    /// <summary>
    /// Primary method for running Light Attacks.
    /// </summary>
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
        _guideController.MoveToNearestEnemy();
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

    public void Unblockable()
    {
        isUnblockable = true;
    }

    public void EndUnblockable()
    {
        isUnblockable = false;
    }

    public void SheathSword()
    {
        throw new System.NotImplementedException();
    }

    public void UnsheathSword()
    {
        throw new System.NotImplementedException();
    }

    private float CalculateDamage()
    {
        return _playerStats.baseDamage;
    }

    public EntityCombatType GetCombatType()
    {
        return _combatType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isAttacking) return;

        //Collects IDamageable component of the entity
        IDamageable attackEntity = other.GetComponent<IDamageable>();
        if (attackEntity == null)
        {
            _playerSword.CreateImpactEffect(other.transform, HitType.GeneralTarget);
            return;
        }

        //Registers attack to the attackRegister
        _attackRegister.RegisterAttackTarget(attackEntity, other, CalculateDamage(), true, isUnblockable);
    }
}
