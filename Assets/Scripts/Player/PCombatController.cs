using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Empty For now
public interface ICombatController
{
    void RunLightAttack();  // May be redundant
    void BlockCombatInputs();
    void UnblockCombatInputs();
    void DrawSword();
    bool CheckIsAttacking();
}

public class PCombatController : MonoBehaviour, ICombatController
{
    //Public variables
    public AttackChainTracker comboTracker;
    public Collider attackCol;
    public bool _isAttacking = false;
    public bool isUnblockable = false;
    public PSwordManager swordManager;

    //Private Variables
    private PlayerInputScript _playerInput;
    private PlayerFunctions _functions;
    private PDamageController _damageController;
    private EntityAttackRegister _attackRegister;
    private CloseEnemyGuideControl _guideController;
    private StatHandler _playerStats;
    private Animator _animator;

    private float _chargeTime;
    private int _comboHits;
    private bool _isInputBlocked = false;
    private bool _isSwordDrawn = false;

    [Header("Audio")]
    public AudioPlayer audio, swordAudio;
    public AudioClip slash1, hit1, heavySlash, heavyHit;

    /// <summary>
    /// Initialises Combat Controller variables and related class components
    /// </summary>
    public void Init(StatHandler playerStats)
    {
        this._playerStats = playerStats;
        this._animator = this.GetComponent<Animator>();
        comboTracker = GetComponent<AttackChainTracker>();

        swordManager = this.GetComponent<PSwordManager>();
        swordManager.Init();

        _playerInput = GetComponent<PlayerInputScript>();
        _damageController = GetComponent<PDamageController>();
        _functions = GetComponent<PlayerFunctions>();

        _attackRegister = new EntityAttackRegister();
        _attackRegister.Init(this.gameObject, EntityType.Player);

        _guideController = new CloseEnemyGuideControl();
        _guideController.Init(this, this.gameObject.transform, this.GetComponent<Rigidbody>());

        
    }

    /// <summary>
    /// Draws the player sword
    /// </summary>
    public void DrawSword() //TODO: SHOULD BE AUTOMATIC
    {
        if (!swordManager.hasAWeapon) return;
        _isSwordDrawn = !_isSwordDrawn;
        _animator.SetBool("IsDrawn", _isSwordDrawn);
        _animator.ResetTrigger("AttackLight");
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
        //if (!_isAttacking)
        //{
            comboTracker.RegisterInput();
            _animator.SetTrigger("AttackLight");
        //}
        _animator.SetInteger("ComboCount", _comboHits);
    }

    private void HeavyAttack()
    {
        if (_isInputBlocked) return;
    }

    //Summary: Resets the AttackCombo after 'Animation Event' has finished.
    public void ResetAttackCombo() {
        //_animator.ResetTrigger("AttackLight");
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
        _animator.ResetTrigger("AttackLight");
        _guideController.MoveToNearestEnemy();
    }

    //Summary: Disables the detection of the sword.
    //
    public void EndAttacking()
    {
        _isAttacking = false;
        _functions.EnableBlock();
        attackCol.enabled = false;
    }

    //Summary: Methods towards enabling and disabling player blocking
    //

    public void Unblockable()
    {
        isUnblockable = true;
        swordManager.swordEffect.BeginUnblockableEffect();
    }

    public void EndUnblockable()
    {
        isUnblockable = false;
        swordManager.swordEffect.EndUnblockableEffect();
    }

    public bool CheckIsAttacking()
    {
        return _isAttacking;
    }

    private float CalculateDamage()
    {
        return _playerStats.baseDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isAttacking) return;
        if (other.CompareTag("Level")) return;
        if (!swordManager.hasAWeapon) return;

        Debug.Log(other.name);

        //Gets IDamageable component of the entity
        IDamageable attackEntity = other.GetComponent<IDamageable>();
        if (attackEntity == null)
        {
            swordManager.swordEffect.CreateImpactEffect(other.transform, HitType.GeneralTarget);
            return;
        }

        //Registers attack to the attackRegister
        _attackRegister.RegisterAttackTarget(attackEntity, swordManager.swordEffect, other, CalculateDamage(), true, isUnblockable);
        if (!isUnblockable) PlayHit();
        else PlayHeavyHit();
        _functions.CancelMove();
        swordAudio.bIgnoreNext = true;
    }

    public void IsParried()
    {
        Debug.Log("Player Parried!");
        EndUnblockable();
        EndAttacking();
        _playerInput.RemoveOverride();
        _animator.SetTrigger("IsParried");
        _playerInput.ResetDodge();
    }
 
    public void PlaySlash()
    {
        if(!slash1) slash1 = GameManager.instance.audioManager.FindSound("Light Attack Swing 1");
        swordAudio.PlayOnce(slash1);
    }

    public void PlayHit()
    {
        if (!hit1) hit1 = GameManager.instance.audioManager.FindSound("Light Attack Hit 1");
        audio.PlayOnce(hit1);
    }

    public void PlayHeavySwing()
    {
        if (!heavySlash) heavySlash = GameManager.instance.audioManager.FindSound("Heavy Attack Swing 2");
        swordAudio.PlayOnce(heavySlash);
    }

    public void PlayHeavyHit()
    {
        if (!heavyHit) heavyHit = GameManager.instance.audioManager.FindSound("Light Attack Hit 3");
        audio.PlayOnce(heavyHit);
    }

}
