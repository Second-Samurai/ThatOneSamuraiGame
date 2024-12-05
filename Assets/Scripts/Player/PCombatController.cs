using System;
using System.Collections;
using System.Collections.Generic;
using Player.Animation;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

//Empty For now
public interface ICombatController
{
    void AttemptLightAttack();
    void BlockCombatInputs();
    void UnblockCombatInputs();
    void DrawSword();
    bool CheckIsAttacking();
    void EndAttacking();
    void ResetAttackCombo();
}

public class PCombatController : MonoBehaviour, ICombatController
{
    //Public variables
    public AttackChainTracker comboTracker;
    public Collider attackCol;
    public bool _isAttacking = false;
    public bool isUnblockable = false;
    [HideInInspector] public PSwordManager swordManager;

    //Private Variables
    private PlayerFunctions _functions;
    private PDamageController _damageController;
    private EntityAttackRegister _attackRegister;
    private CloseEnemyGuideControl _guideController;
    private StatHandler _playerStats;
    private PlayerAnimationComponent m_PlayerAnimationComponent;
    private PBufferedInputs m_PlayerBufferedInputs;
    
    private float _chargeTime;
    private int _comboHits;
    private bool _isInputBlocked = false;
    private bool _isSwordDrawn = false;

    [Header("Audio")]
    public AudioPlayer audio;
    public AudioPlayer swordAudio;
    public AudioClip slash1, hit1, heavySlash, heavyHit;
    private AudioManager audioManager;
    public AudioClip[] saberwhoosh;
    public AudioClip[] lightSaberHit;


    /// <summary>
    /// Initialises Combat Controller variables and related class components
    /// </summary>
    public void Init(StatHandler playerStats)
    {
        this._playerStats = playerStats;
        m_PlayerAnimationComponent = GetComponent<PlayerAnimationComponent>();
        comboTracker = GetComponent<AttackChainTracker>();

        swordManager = this.GetComponent<PSwordManager>();
        swordManager.Init();

        _damageController = GetComponent<PDamageController>();
        _functions = GetComponent<PlayerFunctions>();

        m_PlayerBufferedInputs = GetComponent<PBufferedInputs>();

        _attackRegister = new EntityAttackRegister();
        _attackRegister.Init(this.gameObject, EntityType.Player);

        _guideController = new CloseEnemyGuideControl();
        _guideController.Init(this, this.gameObject.transform, this.GetComponent<Rigidbody>());
    }

    #region - - - - - - Lifecycle Methods - - - - - -
    
    public void Start()
    {
        audioManager = GameManager.instance.audioManager;
        lightSaberHit = GameManager.instance.audioManager.FindAll("lightSaber-Slash").ToArray();
        saberwhoosh = GameManager.instance.audioManager.FindAll("lightSaber-Swing ").ToArray();
    }
    
    #endregion Lifecycle Methods
    
    #region - - - - - - Methods - - - - - -
    
    /// <summary>
    /// Draws the player sword
    /// </summary>
    public void DrawSword() //TODO: SHOULD BE AUTOMATIC
    {
        if (!swordManager.hasAWeapon) return;
        _isSwordDrawn = !_isSwordDrawn;

        m_PlayerAnimationComponent.TriggerDrawSword();
    }
    
    /// <summary>
    /// Primary method for running Light Attacks.
    /// </summary>
    public void AttemptLightAttack()
    {
        if (_isInputBlocked)
        {
            Debug.Log("Input blocked");
            return;
        }
        
        if (m_PlayerBufferedInputs.IsAttackInputBufferRunning())
        {
            m_PlayerBufferedInputs.SetAttackInputCached(true);
            return;
        }
        
        // Start the buffer
        m_PlayerBufferedInputs.StartAttackBuffer();
        
        // Increment combo
        _comboHits++;
        _comboHits = Mathf.Clamp(_comboHits, 0, 4);
        _chargeTime = 0;
        
        // Do the "Magic" of attacking
        comboTracker.RegisterInput();
        
        //_animator.SetInteger("ComboCount", _comboHits);
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
        //m_PlayerAnimationComponent.ResetLightAttack();
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
        if (other.CompareTag("Level") || other.gameObject.CompareTag("LOD") || other.gameObject.layer == LayerMask.NameToLayer("Detector")) return;
        if (!swordManager.hasAWeapon) return;

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
        EndUnblockable();
        EndAttacking();
        //_playerInput.RemoveOverride(); // Note: The override is unused
        m_PlayerAnimationComponent.TriggerIsParried();

        IPlayerSpecialAction _PlayerSpecialAction = this.GetComponent<IPlayerSpecialAction>();
        _PlayerSpecialAction.ResetDodge();
    }
 
    public void PlaySlash()
    {
        if (audioManager.LightSaber == false)
        {
            if (!slash1) slash1 = GameManager.instance.audioManager.FindSound("Light Attack Swing 1");
            swordAudio.PlayOnce(slash1, audioManager.SFXVol);
        }
        else if (audioManager.LightSaber == true)
        {
            int j = Random.Range(0, saberwhoosh.Length);
            swordAudio.PlayOnce(saberwhoosh[j], audioManager.SFXVol / 2);
        }

    }

    public void PlayHit()
    {
        if (audioManager.LightSaber == false)
        {
            if (!hit1) hit1 = GameManager.instance.audioManager.FindSound("Light Attack Hit 1");
            audio.PlayOnce(hit1, audioManager.SFXVol);
        }

        else if (audioManager.LightSaber == true)
        {
            int j = Random.Range(0, lightSaberHit.Length);
            swordAudio.PlayOnce(lightSaberHit[j], audioManager.SFXVol / 2);
        }
    }

    public void PlayHeavySwing()
    {
        if (audioManager.LightSaber == false)
        {
            if (!heavySlash) heavySlash = GameManager.instance.audioManager.FindSound("Heavy Attack Swing 2");
            swordAudio.PlayOnce(heavySlash, audioManager.SFXVol);
        }
        else if (audioManager.LightSaber == true)
        {
            int j = Random.Range(0, saberwhoosh.Length);
            swordAudio.PlayOnce(saberwhoosh[j], audioManager.SFXVol / 2, .5f, .7f);
        }
    }

    public void PlayHeavyHit()
    {
        if (audioManager.LightSaber == false)
        {
            if (!heavyHit) heavyHit = GameManager.instance.audioManager.FindSound("Light Attack Hit 3");
            audio.PlayOnce(heavyHit, audioManager.SFXVol);
        }
        else if (audioManager.LightSaber == true)
        {
            int j = Random.Range(0, lightSaberHit.Length);
            swordAudio.PlayOnce(lightSaberHit[j], audioManager.SFXVol * 2, .5f, .7f);
        }
    }
    
    #endregion Methods

}
