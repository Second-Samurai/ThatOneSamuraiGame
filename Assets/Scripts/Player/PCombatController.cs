using Player.Animation;
using ThatOneSamuraiGame;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using UnityEngine;

//Empty For now
public interface ICombatController
{
    void AttemptLightAttack();
    bool IsSwordDrawn { get; }
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
    // [HideInInspector] public PSwordManager swordManager;
    private IWeaponSystem m_WeaponSystem;

    //Private Variables
    private PlayerFunctions _functions;
    private KnockbackAttackHandler m_KnockbackAttackHandler;
    private BlockingAttackHandler m_BlockingAttackHandler;
    
    private PDamageController _damageController;
    private EntityAttackRegister _attackRegister;
    private CloseEnemyGuideControl _guideController;
    private StatHandler _playerStats;
    private PlayerMovement m_PlayerMovement;
    private PlayerAnimationComponent m_PlayerAnimationComponent;
    private PBufferedInputs m_PlayerBufferedInputs;

    private float m_MinSprintTime = 0.5f;
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

    public bool IsSwordDrawn => this._isSwordDrawn;

    /// <summary>
    /// Initialises Combat Controller variables and related class components
    /// </summary>
    public void Init(StatHandler playerStats)
    {
        this._playerStats = playerStats;
        m_PlayerAnimationComponent = GetComponent<PlayerAnimationComponent>();
        comboTracker = GetComponent<AttackChainTracker>();

        this.m_WeaponSystem = this.GetComponent<IWeaponSystem>();

        _damageController = GetComponent<PDamageController>();
        _functions = GetComponent<PlayerFunctions>();
        this.m_KnockbackAttackHandler = this.GetComponent<KnockbackAttackHandler>();
        this.m_BlockingAttackHandler = this.GetComponent<BlockingAttackHandler>();
        m_PlayerMovement = GetComponent<PlayerMovement>();

        m_PlayerBufferedInputs = GetComponent<PBufferedInputs>();

        _attackRegister = new EntityAttackRegister();
        _attackRegister.Init(this.gameObject, EntityType.Player);

        _guideController = new CloseEnemyGuideControl();
        _guideController.Init(this, this.gameObject.transform, this.GetComponent<Rigidbody>());
    }

    #region - - - - - - Lifecycle Methods - - - - - -
    
    public void Start()
    {
        audioManager = AudioManager.instance;
        lightSaberHit = AudioManager.instance.FindAll("lightSaber-Slash").ToArray();
        saberwhoosh = AudioManager.instance.FindAll("lightSaber-Swing ").ToArray();

    }
    
    #endregion Lifecycle Methods
    
    #region - - - - - - Methods - - - - - -
    
    /// <summary>
    /// Draws the player sword
    /// </summary>
    public void DrawSword() //TODO: SHOULD BE AUTOMATIC
    {
        if (!this.m_WeaponSystem.IsWeaponEquipped()) return;
        _isSwordDrawn = !_isSwordDrawn;

        _isInputBlocked = false;

        m_PlayerAnimationComponent.TriggerDrawSword();
    }
    
    /// <summary>
    /// Primary method for running Light Attacks.
    /// </summary>
    public void AttemptLightAttack()
    {
        if (_isInputBlocked || !_isSwordDrawn)
        {
            //Debug.Log("Input blocked");
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
        //_comboHits = Mathf.Clamp(_comboHits, 0, 4);
        _chargeTime = 0;
        
        // Determine if the next attack should be a sprint attack
        bool isSprintAttack = m_PlayerMovement.IsSprinting() && m_PlayerMovement.GetSprintDuration() > m_MinSprintTime;
        
        // Do the "Magic" of attacking
        comboTracker.RegisterInput(isSprintAttack);
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
    
    // ======== EVENT CALLED ========

    //Summary: Enabled collision detection to apply damage to hit target.
    // 1stAttackEdit - 00:01
    public void BeginAttacking()
    {
        _isAttacking = true;
        this.m_BlockingAttackHandler.DisableBlock();
        attackCol.enabled = true;
        //m_PlayerAnimationComponent.ResetLightAttack();
        _guideController.MoveToNearestEnemy();
    }

    //Summary: Disables the detection of the sword.
    //
    public void EndAttacking()
    {
        _isAttacking = false;
        // _functions.EnableBlock();
        this.m_BlockingAttackHandler.EnableBlock();
        attackCol.enabled = false;
    }

    //Summary: Methods towards enabling and disabling player blocking
    //

    public void Unblockable()
    {
        isUnblockable = true;
        this.m_WeaponSystem.WeaponEffectHandler.BeginUnblockableEffect();
    }

    public void EndUnblockable()
    {
        isUnblockable = false;
        this.m_WeaponSystem.WeaponEffectHandler.EndUnblockableEffect();
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
        if (!this.m_WeaponSystem.IsWeaponEquipped()) return;

        //Gets IDamageable component of the entity
        IDamageable attackEntity = other.GetComponent<IDamageable>();
        if (attackEntity == null)
        {
            this.m_WeaponSystem.WeaponEffectHandler.CreateImpactEffect(other.transform, HitType.GeneralTarget);
            return;
        }

        //Registers attack to the attackRegister
        _attackRegister.RegisterAttackTarget(attackEntity, this.m_WeaponSystem.WeaponEffectHandler, other, CalculateDamage(), true, isUnblockable);
        if (!isUnblockable) PlayHit();
        else PlayHeavyHit();
        this.m_PlayerMovement.CancelMove();
        swordAudio.bIgnoreNext = true;
    }

    public void IsParried()
    {
        EndUnblockable();
        EndAttacking();
        //_playerInput.RemoveOverride(); // Note: The override is unused
        m_PlayerAnimationComponent.TriggerIsParried();

        // IPlayerSpecialAction _PlayerSpecialAction = this.GetComponent<IPlayerSpecialAction>();
        // _PlayerSpecialAction.ResetDodge();

        IPlayerDodgeMovement _playerDodgeMovement = this.GetComponent<IPlayerDodgeMovement>();
        _playerDodgeMovement.EnableDodge();
    }
 
    public void PlaySlash()
    {
        if (audioManager.LightSaber == false)
        {
            if (!slash1) slash1 = AudioManager.instance.FindSound("Light Attack Swing 1");
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
            if (!hit1) hit1 = AudioManager.instance.FindSound("Light Attack Hit 1");
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
            if (!heavySlash) heavySlash = AudioManager.instance.FindSound("Heavy Attack Swing 2");
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
            if (!heavyHit) heavyHit = AudioManager.instance.FindSound("Light Attack Hit 3");
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
