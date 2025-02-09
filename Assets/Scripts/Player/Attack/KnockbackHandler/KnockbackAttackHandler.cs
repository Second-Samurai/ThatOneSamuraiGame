using System.Collections;
using Enemies;
using Player.Animation;
using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

namespace ThatOneSamuraiGame
{

    public class KnockbackAttackHandler : MonoBehaviour
    {
        [Header("Block Variables")]
        public bool bIsBlocking = false;
        public bool bInputtingBlock = false;

        [Header("Parry Variables")]
        public bool bIsParrying = false;
        public float parryTimer = 0f; 
        public float parryTimerTarget;
        bool _bDontCheckParry = false;
        
        private Rigidbody rb;
        public ParryEffects parryEffects;
        public ILockOnSystem LockOnSystem;
        private IPlayerMovement m_PlayerMovement;
        [SerializeField] private BlockingAttackHandler m_BlockAttackHandler;
        [SerializeField] private LayerMask enemyMask;
        private PlayerAnimationComponent m_PlayerAnimationComponent;
        private PDamageController _pDamageController;
        private HitstopController hitstopController;
        private PlayerSFX playerSFX;
        private  RaycastHit sprintAttackTarget;
        
        public bool bIsDead = false;
        bool bIsSprintAttacking = false;
        public bool bAllowDeathMoveReset = true;

        // Player States
        private PlayerMovementDataContainer _mPlayerMovementDataContainer;
        private PlayerSpecialActionState m_PlayerSpecialActionState;
        
        private void Start()
        {
            playerSFX = gameObject.GetComponent<PlayerSFX>();
            rb = GetComponent<Rigidbody>();
            _pDamageController = GetComponent<PDamageController>();
            m_PlayerAnimationComponent = GetComponent<PlayerAnimationComponent>();
            hitstopController = GameManager.instance.GetComponent<HitstopController>();
            enemyMask = LayerMask.GetMask("Enemy");
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
            this.LockOnSystem = SceneManager.Instance.LockOnSystem;

            IPlayerState _PlayerState = this.GetComponent<IPlayerState>();
            this._mPlayerMovementDataContainer = _PlayerState.PlayerMovementDataContainer;
            this.m_PlayerSpecialActionState = _PlayerState.PlayerSpecialActionState;
        }
        
        private void Update()
        {
            CheckParry();

            if (bAllowDeathMoveReset)
            {
                if (bIsDead && this._mPlayerMovementDataContainer.IsMovementEnabled)
                    m_PlayerMovement.DisableMovement();
                else if (!bIsDead && !this._mPlayerMovementDataContainer.IsMovementEnabled)
                    m_PlayerMovement.EnableMovement();
            }
        }
        
        public IEnumerator ImpulseWithTimer(Vector3 lastDir, float force, float timer)
        {
            float dodgeTimer = timer;
            while (dodgeTimer > 0f)
            {
                // if(bLockedOn)
                //transform.Translate(lastDir.normalized * force * Time.deltaTime);
                m_PlayerAnimationComponent.SetRootMotion(false);
                if (bIsSprintAttacking) CorrectAttackAngle(ref lastDir);
                rb.linearVelocity = lastDir.normalized * force ;
                // rb.MovePosition(transform.position + lastDir.normalized * force * Time.deltaTime);
                //else
                //    transform.position += lastDir.normalized * force * Time.deltaTime;
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
        
        // Should belong to health system
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
                        this.m_BlockAttackHandler.TriggerBlock(attacker);
                    }
                    else
                    {

                        KillPlayer();
                    }
                }
                else KillPlayer();
            }
        }
        
        // Should belong to movement system
        public void CancelMove()
        {
            StopAllCoroutines(); 
            this.m_PlayerMovement.EnableMovement();
            this.m_PlayerMovement.EnableRotation();
            rb.linearVelocity = Vector3.zero;
            m_PlayerAnimationComponent.SetRootMotion(true);
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
                m_PlayerAnimationComponent.TriggerKnockdown();
                StartCoroutine(ImpulseWithTimer(direction, amount, duration));
            }
        }
        
        // Belongs to player health system
        public void KillPlayer()
        {
            if (!bIsDead)
            {
                // play anim
                m_PlayerAnimationComponent.SetDead(true);
            
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
                    if (!bInputtingBlock && bIsBlocking) this.m_BlockAttackHandler.EndBlock();
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
        
    }

}