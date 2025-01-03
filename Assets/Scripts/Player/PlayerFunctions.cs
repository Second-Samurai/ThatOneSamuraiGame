using System.Collections;
using Enemies;
using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = ThatOneSamuraiGame.Scripts.Scene.SceneManager.SceneManager;

// Tech-Debt: #35 - PlayerFunctions will be refactored to mitigate large class bloat.
public class PlayerFunctions : MonoBehaviour
{
    [Header("Block Variables")]
    public bool bIsBlocking = false;
    public float blockTimer = 0f;
    public float blockCooldown;
    public bool bCanBlock = true;
    public bool bInputtingBlock = false;

    [Header("Parry Variables")]
    public bool bIsParrying = false;
    public float parryTimer = 0f; 
    public float parryTimerTarget;
    bool _bDontCheckParry = false;

    public ILockOnSystem LockOnSystem;

    [Header("IK Functions")]
    IKPuppet _IKPuppet;

    Animator _animator;
    PDamageController _pDamageController;
    public Rigidbody rb;

    HitstopController hitstopController;

    public bool bIsDead = false;

    public RectTransform screenCenter;

    public ParryEffects parryEffects;

    public GameObject pauseMenu;

    public GameObject lSword, rSword;

    public bool bSlide = false;

    public bool bAllowDeathMoveReset = true;

    private PlayerSFX playerSFX;

    bool bIsSprintAttacking = false;

    RaycastHit sprintAttackTarget;
    [SerializeField] LayerMask enemyMask;
    
    // Player States
    private PlayerMovementState m_PlayerMovementState;
    private PlayerSpecialActionState m_PlayerSpecialActionState;

    private IPlayerMovement m_PlayerMovement;

    private void Start()
    {
        playerSFX = gameObject.GetComponent<PlayerSFX>();

        _IKPuppet = GetComponent<IKPuppet>();

        rb = GetComponent<Rigidbody>();

        _pDamageController = GetComponent<PDamageController>();

        _animator = GetComponent<Animator>();

        hitstopController = GameManager.instance.GetComponent<HitstopController>();

        enemyMask = LayerMask.GetMask("Enemy");
        //enemyMask = ~enemyMask;

        this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();

        this.LockOnSystem = SceneManager.Instance.LockOnSystem;

        IPlayerState _PlayerState = this.GetComponent<IPlayerState>();
        this.m_PlayerMovementState = _PlayerState.PlayerMovementState;
        this.m_PlayerSpecialActionState = _PlayerState.PlayerSpecialActionState;
    }
    public void SetBlockCooldown()
    {
        blockTimer = blockCooldown;
    }

    public void StartBlock()
    {

        if (!bIsBlocking && blockTimer == 0f && bCanBlock)
        {
            playerSFX.Armour();
            bIsBlocking = true;
            _bDontCheckParry = false;
            parryEffects.PlayGleam();
            _IKPuppet.EnableIK();
            bInputtingBlock = true;
            bIsParrying = true;
            parryTimer = 0f;
        }
        else
        {
            Debug.LogError("bIsblocking: " + bIsBlocking + " blockTimer: " + blockTimer + " bcanBlock: " + bCanBlock);
        }
    }

    public void EndBlock()
    {
        // if (bIsBlocking)
        // { 
        bIsBlocking = false;
        bIsParrying = false;
        parryTimer = 0f;
        _IKPuppet.DisableIK();
        SetBlockCooldown();
       // }
    }

    private void Update()
    {
        CheckBlockCooldown();
        CheckParry();
        //remove this
        if (_bDontCheckParry && !bInputtingBlock && bIsBlocking) EndBlock(); 

        if (bAllowDeathMoveReset)
        {
            if (bIsDead && this.m_PlayerMovementState.IsMovementEnabled)
                m_PlayerMovement.DisableMovement();
            else if (!bIsDead && !this.m_PlayerMovementState.IsMovementEnabled)
                m_PlayerMovement.EnableMovement();
        }
    }
     

    public void ForwardImpulse(float force)
    {

        StartCoroutine(ImpulseWithTimer(transform.forward, force, .15f));
    }

    public void JumpImpulseWithTimer(float timer)
    {
        bIsSprintAttacking = true;
        transform.Translate(Vector3.up * 1);
        StartCoroutine(ImpulseWithTimer(transform.forward, 20, timer));
    }

    public void ImpulseMove(Vector3 dir, float force)
    {
        StartCoroutine(DodgeImpulse(dir, force));
    }

    private void CheckParry()
    {
        if (!_bDontCheckParry)
        {
            if (bIsBlocking && parryTimer < parryTimerTarget)
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
                if (!bInputtingBlock && bIsBlocking) EndBlock();
            }
        }
    }
    private void CheckBlockCooldown()
    {
        if (blockTimer != 0f)
        {
            if (blockTimer > 0f)
            {
                blockTimer -= Time.deltaTime;
            }
            if (blockTimer < 0f)
                blockTimer = 0f;
        }
    }

    public IEnumerator ImpulseWithTimer(Vector3 lastDir, float force, float timer)
    {
        float dodgeTimer = timer;
        while (dodgeTimer > 0f)
        {
            // if(bLockedOn)
            //transform.Translate(lastDir.normalized * force * Time.deltaTime);
            _animator.applyRootMotion = false;
            if (bIsSprintAttacking) CorrectAttackAngle(ref lastDir);
            rb.linearVelocity = lastDir.normalized * force ;
           // rb.MovePosition(transform.position + lastDir.normalized * force * Time.deltaTime);
            //else
            //    transform.position += lastDir.normalized * force * Time.deltaTime;
            dodgeTimer -= Time.deltaTime;
            yield return null;
        }
        _animator.applyRootMotion = true;
        EnableBlock();
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

    public IEnumerator DodgeImpulse(Vector3 lastDir, float force)
    {
        float dodgeTimer = .15f;
        while (dodgeTimer > 0f)
        {
            // if(bLockedOn)
            transform.Translate(lastDir.normalized * force * Time.deltaTime); 
            //else
            //    transform.position += lastDir.normalized * force * Time.deltaTime;
            dodgeTimer -= Time.deltaTime;
            yield return null;
        }
        EnableBlock();
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

    public void ApplyHit(GameObject attacker, bool unblockable, float damage)
    {
        if (!this.m_PlayerSpecialActionState.IsDodging)
        { 
            if (bIsParrying && !unblockable)
            {
                TriggerParry(attacker, damage);
            }
            else if (!unblockable)
            {
                if (bIsBlocking)
                {
                    TriggerBlock(attacker);
                }
                else
                {

                    KillPlayer();
                }
            }
            else KillPlayer();
        }
    }

    public void CancelMove()
    {
        StopAllCoroutines(); 
        this.m_PlayerMovement.EnableMovement();
        this.m_PlayerMovement.EnableRotation();
        rb.linearVelocity = Vector3.zero;
        _animator.applyRootMotion = true;
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
            //Debug.Log("HIT" + amount * direction);
            this.m_PlayerMovement.DisableRotation();
            _animator.SetTrigger("KnockdownTrigger");
            StartCoroutine(ImpulseWithTimer(direction, amount, duration));
        }
    }

    public void TriggerParry(GameObject attacker, float damage)
    {
        parryEffects.PlayParry();
        _animator.SetTrigger("Parrying");
        if (attacker.GetComponent<AISystem>().enemyType != EnemyType.BOSS) hitstopController.SlowTime(.5f, 1);
        if(attacker != null)
        {
            // TODO: Fix with damage later
            attacker.GetComponent<EDamageController>().OnParried(5); //Damage attacker's guard meter

        }
        //GameManager.instance.mainCamera.gameObject.GetComponent<CameraShakeController>().ShakeCamera(.7f);
        //Debug.LogWarning("Parried " + attacker.name);

    }
    public void TriggerBlock(GameObject attacker)
    {
        //rotate to face attacker
        parryEffects.PlayBlock();
        //GameManager.instance.mainCamera.gameObject.GetComponent<CameraShakeController>().ShakeCamera(1);
        bIsBlocking = false;
        _animator.SetTrigger("GuardBreak");
        //Debug.LogWarning("Guard broken!");
        _IKPuppet.DisableIK();
    }

    public void KillPlayer()
    {
        if (!bIsDead)
        {
            // play anim
            _animator.SetTrigger("Death");
            _animator.SetBool("isDead", true);
            
            // trigger rewind
            bIsDead = true;
            
            // Activate input switch to rewind
            IInputManager _InputManager = GameManager.instance.InputManager;
            _InputManager.SwitchToRewindControls();
            
            //Debug.LogError("Player killed!");
            //GameManager.instance.mainCamera.gameObject.GetComponent<CameraShakeController>().ShakeCamera(1);
            //GameManager.instance.gameObject.GetComponent<HitstopController>().Hitstop(.3f);
            this.LockOnSystem.EndLockOn();
        }
    }

    public void DisableBlock()
    {
        bCanBlock = false;
        //Debug.LogWarning("off");

        _IKPuppet.DisableIK();
    }

    public void EnableBlock()
    {
        bCanBlock = true;
        //Debug.LogWarning("on");
    }
 
    public void SnapToEnemy()
    {
        //Vector3 CenterPos = GetMousePosition(screenCenter.position, Camera.main);
        //Vector3 attackDir = 
    }
}
