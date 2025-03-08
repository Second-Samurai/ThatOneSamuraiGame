using System.Collections;
using Enemies;
using Enemies.Attacking;
using Player.Animation;
using ThatOneSamuraiGame;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using UnityEngine;

public interface IPlayerParryActions
{

    #region - - - - - - Methods - - - - - -

    void AttackDeflected();

    #endregion Methods

}

public class ParryAttackHandler : MonoBehaviour, IPlayerParryActions
{

    #region - - - - - - New Fields - - - - - -

    public float m_ParryDetectionTime;
    public bool m_IsParryActive;
    
    public ParryEffects m_ParryEffects;

    private IPlayerAnimationDispatcher m_AnimationDispatcher;

    private EventTimer m_ParryDetectionEventTimer;

    #endregion New Fields
    
    #region - - - - - - Old Fields - - - - - -

    [Header("Parry Variables")]
    public bool bIsParrying = false;
    public float parryTimer = 0f; 
    public float parryTimerTarget;
    bool _bDontCheckParry = false;
    
    private Rigidbody rb;
    public ParryEffects parryEffects;
    
    // Player Component Fields
    private IPlayerMovement m_PlayerMovement;
    private IPlayerDodgeMovement m_DodgeMovement;
    private IPlayerAttackSystem m_AttackHandler;
    private IWeaponSystem m_WeaponSystem;
    [SerializeField] private BlockingAttackHandler m_BlockAttackHandler;
    
    [SerializeField] private LayerMask enemyMask;
    private PlayerAnimationComponent m_PlayerAnimationComponent;
    private HitstopController hitstopController;
    private PlayerSFX playerSFX;
    private  RaycastHit sprintAttackTarget;
    
    public bool bIsDead = false;
    bool bIsSprintAttacking = false;
    public bool bAllowDeathMoveReset = true;

    // Player States
    private PlayerMovementDataContainer _mPlayerMovementDataContainer;
    private PlayerSpecialActionState m_PlayerSpecialActionState;

    #endregion Old Fields

    #region - - - - - - Properties - - - - - -

    public bool IsParrying
        => this.m_IsParryActive;

    #endregion Properties
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_ParryDetectionEventTimer = new EventTimer(
            this.m_ParryDetectionTime, 
            Time.deltaTime, 
            this.EndParry, 
            canRestart: false, 
            startImmediately: false);
        this.m_AnimationDispatcher = this.GetComponent<IPlayerAnimationDispatcher>();
        
        playerSFX = gameObject.GetComponent<PlayerSFX>();
        rb = GetComponent<Rigidbody>();
        m_PlayerAnimationComponent = GetComponent<PlayerAnimationComponent>();
        hitstopController = GameManager.instance.GetComponent<HitstopController>();
        enemyMask = LayerMask.GetMask("Enemy");
        this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
        this.m_DodgeMovement = this.GetComponent<IPlayerDodgeMovement>();
        this.m_WeaponSystem = this.GetComponent<IWeaponSystem>();
        this.m_AttackHandler = this.GetComponent<IPlayerAttackSystem>();

        IPlayerState _PlayerState = this.GetComponent<IPlayerState>();
        this._mPlayerMovementDataContainer = _PlayerState.PlayerMovementDataContainer;
        this.m_PlayerSpecialActionState = _PlayerState.PlayerSpecialActionState;

        IAttackAnimationEvents _AnimationEvents = this.GetComponent<IAttackAnimationEvents>();
        _AnimationEvents.OnForwardImpulse.AddListener(force => 
            StartCoroutine(ImpulseWithTimer(transform.forward, force, .15f)));
        _AnimationEvents.OnForwardJumpImpulse.AddListener(timer =>
        {
            bIsSprintAttacking = true;
            transform.Translate(Vector3.up * 1);
            StartCoroutine(ImpulseWithTimer(transform.forward, 20, timer));
        });
    }
    
    private void Update()
    {
        this.m_ParryDetectionEventTimer.TickTimer();
        
        CheckParry();

        if (bAllowDeathMoveReset)
        {
            if (bIsDead && this._mPlayerMovementDataContainer.IsMovementEnabled)
                m_PlayerMovement.DisableMovement();
            else if (!bIsDead && !this._mPlayerMovementDataContainer.IsMovementEnabled)
                m_PlayerMovement.EnableMovement();
        }
    }

    #endregion Unity Methods

    #region - - - - - - Old Parry Methods - - - - - -

    public IEnumerator ImpulseWithTimer(Vector3 lastDir, float force, float timer)
    {
        float dodgeTimer = timer;
        while (dodgeTimer > 0f)
        {
            this.m_PlayerAnimationComponent.SetRootMotion(false);
            if (bIsSprintAttacking) CorrectAttackAngle(ref lastDir);
            rb.linearVelocity = lastDir.normalized * force ;
            dodgeTimer -= Time.deltaTime;
            yield return null;
        }
        m_PlayerAnimationComponent.SetRootMotion(true);
        this.m_BlockAttackHandler.EnableBlock();
    }
    
    public bool RadialCast(Transform origin, int rayCount, int offsetValue, int layerMask, ref RaycastHit hit)
    {
        Quaternion offsetAngle;
        Vector3 castAngle;
     
        for (int i = 0; i < rayCount; i++)
        {
            RaycastHit _hit;
            offsetAngle = Quaternion.AngleAxis(offsetValue, new Vector3(0, 1, 0));
            castAngle = offsetAngle * origin.forward;
            Debug.DrawRay(origin.position, castAngle*10, Color.red);

            if (Physics.Raycast(origin.position, castAngle, out _hit, 10, layerMask))
            {
                hit = _hit;
                return true;
            }
            offsetValue += 10;
        }
        return false;
    }

    public void HandleParryHit(GameObject attacker, float damage, out bool _IsHitParried)
    {
        if (this.m_PlayerSpecialActionState.IsDodging || !bIsParrying)
        {
            _IsHitParried = false;
            return;
        }
        
        this.TriggerParry(attacker, damage);
        _IsHitParried = true;
    }
    
    public void Knockback(float amount, Vector3 direction, float duration, GameObject attacker)
    {
        if (bIsParrying)
        {
            TriggerParry(attacker, amount);
        }
        else if (!this.m_PlayerSpecialActionState.IsDodging)
        {
            playerSFX.Smack();
            this.m_PlayerMovement.DisableRotation();
            m_PlayerAnimationComponent.TriggerKnockdown();
            StartCoroutine(ImpulseWithTimer(direction, amount, duration));
        }
    }

    public void StopParry()
    {
        bIsParrying = false;
        _bDontCheckParry = false;
        parryTimer = 0f;
    }
    
    public void TriggerParry(GameObject attacker, float damage)
    {
        parryEffects.PlayParry();
        m_PlayerAnimationComponent.TriggerParry();
        if (attacker.GetComponent<AISystem>().enemyType != EnemyType.BOSS) hitstopController.SlowTime(.5f, 1);
        if(attacker != null)
        {
            // TODO: Fix with damage later
            attacker.GetComponent<EDamageController>().OnParried(5); //Damage attacker's guard meter
        }
    }

    public void AttackDeflected()
    {
        this.m_WeaponSystem.WeaponEffectHandler.EndUnblockableEffect();
        this.m_AttackHandler.EndAttack();
        this.m_PlayerAnimationComponent.TriggerIsParried();
        this.m_DodgeMovement.EnableDodge();
    }
    
    private void CheckParry()
    {
        if (!_bDontCheckParry)
        {
            if (parryTimer < parryTimerTarget)
            {
                parryTimer += Time.deltaTime;
                bIsParrying = true;
            }
            if (parryTimer > parryTimerTarget)
                parryTimer = parryTimerTarget;
            if (parryTimer == parryTimerTarget)
            {
                bIsParrying = false;
                _bDontCheckParry = true;
            }
        }
    }
    
    void CorrectAttackAngle(ref Vector3 lastDir)
    {
        if( RadialCast(transform, 10, -45, enemyMask, ref sprintAttackTarget))
        {
            transform.LookAt(sprintAttackTarget.collider.gameObject.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            lastDir = sprintAttackTarget.collider.gameObject.transform.position - transform.position;
        } 
    }
    
    #endregion Old Parry Methods

    #region - - - - - - New Parry Methods - - - - - -

    public void StartParry()
    {
        this.m_IsParryActive = true;
        this.m_ParryDetectionEventTimer.StartTimer();
    }

    public void EndParry()
    {
        this.m_IsParryActive = false;
        this.m_ParryDetectionEventTimer.StopTimer();
    }

    public void PerformParry(GameObject attacker)
    {
        if (this.m_PlayerSpecialActionState.IsDodging || !bIsParrying)
            return;
        
        this.m_ParryEffects.PlayParry();
        this.m_AnimationDispatcher.Dispatch(PlayerAnimationEventStates.Parrying);

        IEnemyAttackSystem _EnemyAttackSystem = attacker.GetComponent<IEnemyAttackSystem>();
        _EnemyAttackSystem.HandleAttackDeflection();
    }
    
    #endregion New Parry Methods

}
