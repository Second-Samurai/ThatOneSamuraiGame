using System;
using ThatOneSamuraiGame;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

public class PlayerAttackSystem : 
    PausableMonoBehaviour, 
    IInitialize<PlayerAttackInitializerData>,
    IPlayerAttackSystem
{

    #region - - - - - - Fields - - - - - -
    
    // ***********************
    // Public / Serialized Fields
    // ***********************
    
    [SerializeField, RequiredField] private SphereCollider m_AttackCollider;
    [SerializeField, RequiredField] private PlayerFinisherAttackHandler m_FinisherAttackHandler;
    
    // ***********************
    // Non-Serialized Fields
    // ***********************
    
    // Component Fields
    private EntityAttackRegister m_EntityAttackRegister;
    private HitstopController m_HitstopController;
    private PlayerAttackState m_PlayerAttackState;
    private IPlayerMovement m_PlayerMovement;
    private CloseEnemyGuideControl m_NearEnemyMovementGuideControl;
    private IWeaponSystem m_WeaponSystem;
    private IPlayerAttackAudio m_AttackAudio;
    private StatHandler m_PlayerStats;
    
    // Attack Component Fields
    private BlockingAttackHandler m_BlockingAttackHandler;
    private LightAttackHandler m_LightAttackHandler;
    private HeavyAttackHandler m_HeavyAttackHandler;
    
    private bool m_CanAttack;

    #endregion Fields

    #region - - - - - - Initializers - - - - - -

    public void Initialize(PlayerAttackInitializerData initializerData)
    {
        this.m_PlayerStats = initializerData.StatHandler;
        
        this.m_EntityAttackRegister = new EntityAttackRegister();
        this.m_EntityAttackRegister.Init(this.gameObject, EntityType.Player);
        
        this.m_NearEnemyMovementGuideControl = new CloseEnemyGuideControl();
        this.m_NearEnemyMovementGuideControl.Init(this, this.gameObject.transform, this.GetComponent<Rigidbody>());
    }

    #endregion Initializers

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_AttackAudio = this.GetComponent<IPlayerAttackAudio>();
        this.m_HitstopController = FindFirstObjectByType<HitstopController>();
        this.m_PlayerAttackState = this.GetComponent<IPlayerState>().PlayerAttackState;
        this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
        this.m_WeaponSystem = this.GetComponent<IWeaponSystem>();
        
        this.m_BlockingAttackHandler = this.GetComponent<BlockingAttackHandler>();
        this.m_LightAttackHandler = this.GetComponent<LightAttackHandler>();
        this.m_HeavyAttackHandler = this.GetComponent<HeavyAttackHandler>();

        IAttackAnimationEvents _AnimationEvents = this.GetComponent<IAttackAnimationEvents>();
        _AnimationEvents.OnParryStunStateStart.AddListener(() => this.m_PlayerAttackState.ParryStunned = true);
        _AnimationEvents.OnParryStunStateEnd.AddListener(() => this.m_PlayerAttackState.ParryStunned = false);
        _AnimationEvents.OnAttackStart.AddListener(this.PrepareAttack);
        _AnimationEvents.OnAttackEnd.AddListener(this.EndAttack);
        _AnimationEvents.OnHeavyAttack.AddListener(this.m_AttackAudio.PlayHeavySwing);
    }

    /// <summary>
    /// Handles attack on trigger collision through the attack collider.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(GameTag.Enemy)
            || other.CompareTag("Level")
            || other.gameObject.layer == LayerMask.NameToLayer("Detector")
            || !this.m_WeaponSystem.IsWeaponEquipped()
            || !this.m_WeaponSystem.IsWeaponDrawn
            || !this.m_CanAttack) return;

        IDamageable _EnemyDamageHandler = other.GetComponent<IDamageable>();
        if (_EnemyDamageHandler != null)
        {
            //Registers attack to the attackRegister
            this.m_EntityAttackRegister.RegisterAttackTarget(
                _EnemyDamageHandler, 
                this.m_WeaponSystem.WeaponEffectHandler, 
                other, 
                this.m_PlayerStats.baseDamage,
                true, 
                false); // previously was unblockable
            
            // TODO: Fix when enemy attack states are clarified.
            // if (!this.m_BlockingAttackHandler.CanBlock()) 
            //     this.m_AttackAudio.PlayHit();
            // else 
            
            this.m_FinisherAttackHandler.RunFinishingAttack(other.transform.gameObject);
            
            this.m_PlayerMovement.CancelMove();
            this.m_AttackAudio.PlayHeavyHit();
            this.m_AttackAudio.IgnoreNextSwordPlayerTrack();
            
            return;
        }
        
        this.m_WeaponSystem.WeaponEffectHandler.CreateImpactEffect(other.transform, HitType.GeneralTarget);
    }

    #endregion Unity Methods

    #region - - - - - - General Methods - - - - - -

    private void PrepareAttack()
    {
        this.m_BlockingAttackHandler.DisableBlock();
        this.m_AttackCollider.enabled = true;
        this.m_NearEnemyMovementGuideControl.MoveToNearestEnemy();
    }

    // NOTE: IPlayerAttackHandler.Attack() is a release input option (e.g. OnMouseUp)
    void IPlayerAttackSystem.Attack()
    {
        if (!this.m_WeaponSystem.IsWeaponEquipped() || !this.m_CanAttack) return;
        
        if (this.m_PlayerAttackState.IsHeavyAttackCharging) // HEAVY ATTACK
            this.m_HeavyAttackHandler.PerformHeavyAttack();
        else
            this.m_LightAttackHandler.QueueLightAttack();
        
        if (this.m_HitstopController.bIsSlowing)
            this.m_HitstopController.CancelEffects();
    }

    public void EndAttack()
    {
        this.m_BlockingAttackHandler.EnableBlock();
        this.m_AttackCollider.enabled = false;
    }

    public void EnableAttack()
        => this.m_CanAttack = true;

    public void DisableAttack()
        => this.m_CanAttack = false;
    
    void IPlayerAttackSystem.ResetAttack()
    {
        this.m_PlayerAttackState.CanAttack = true;
        this.EndAttack();
    }

    #endregion General Methods

    #region - - - - - - Parry and Block Methods - - - - - -

    // Tech-Debt: #35 - PlayerFunctions will be refactored to mitigate large class bloat.
    void IPlayerAttackSystem.EndBlock()
        => this.m_BlockingAttackHandler.EndBlock();
    
    void IPlayerAttackSystem.StartBlock() 
        => this.m_BlockingAttackHandler.StartBlock();

    void IPlayerAttackSystem.EndParryAction()
    {
        this.m_PlayerAttackState.ParryStunned = false;
        this.m_HitstopController.CancelEffects();
    }

    #endregion Parry and Block Methods

    #region - - - - - - Heavy Attack Methods - - - - - -

    void IPlayerAttackSystem.StartHeavy() 
        => this.m_HeavyAttackHandler.StartHeavyAttack();

    // Note: This behaviour is not implemented, but will be open for future use.
    void IPlayerAttackSystem.StartHeavyAlternative()
        => throw new NotImplementedException();

    #endregion Heavy Attack Methods
    
}

public class PlayerAttackInitializerData
{

    #region - - - - - - Properties - - - - - -

    public ICameraController CameraController { get; private set; }
    
    public StatHandler StatHandler { get; private set; }

    #endregion Properties

    #region - - - - - - Constructors - - - - - -

    public PlayerAttackInitializerData(ICameraController cameraController, StatHandler playerStats)
    {
        this.CameraController = cameraController ?? throw new ArgumentNullException(nameof(cameraController));
        this.StatHandler = playerStats ?? throw new ArgumentNullException(nameof(playerStats));
    }

    #endregion Constructors

}
