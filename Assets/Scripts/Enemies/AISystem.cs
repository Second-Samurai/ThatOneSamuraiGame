using System.Collections;
using System.Diagnostics;
using Enemies.Enemy_States;
using Enemy_Scripts;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using UnityEngine.InputSystem;

public enum EnemyType
{
    SWORDSMAN,
    ARCHER,
    GLAIVEWIELDER,
    BOSS,
    MINIBOSS,
    TUTORIALENEMY
}

namespace Enemies
{
    // AI SYSTEM INFO
    // AISystem is responsible for receiving calls to tell the enemy what to perform. It should also
    // Be responsible for storing enemy data (i.e. Guard meter, remaining guard etc.) BUT
    // any enemy behaviours should be handled through the state machine
    public class AISystem : EnemyStateMachine
    {

        #region Fields and Properties 
        //ENEMY TYPE, SET IN PREFAB INSPECTOR
        public EnemyType enemyType;
        //WEAPON COLLIDER, SET IN PREFAB INSPECTOR
        public CapsuleCollider meleeCollider;

        //ENEMY SETTINGS [See EntityStatData for list of stats]
        public EnemySettings enemySettings; // Taken from EnemySettings Scriptable object in start
        public StatHandler statHandler;
        public EnemyTracker enemyTracker;
        public EnemySpawnCheck spawnCheck;
        
        //ANIMATOR
        public Animator animator;
        public bool bPlayerFound = false;
        
        //NAVMESH
        public NavMeshAgent navMeshAgent;
        public bool bIsIdle = true;
        
        //DAMAGE CONTROLS
        public CapsuleCollider col; //SET IN PREFAB INSPECTOR
        public EDamageController eDamageController;
        public bool bIsDead = false;
        public bool bIsUnblockable = false;
        public KnockbackAttack kbController;
        public ArmourManager armourManager;
        public bool bHasArmour;
        public TriggerImpulse camImpulse;
        public bool bIsQuickBlocking = false;
        public bool bFinish = false;

        //NOTE: isStunned is handled in Guarding script, inside the eDamageController script

        //Float offset added to the target location so the enemy doesn't clip into the floor 
        //because the player's origin point is on the floor
        public Vector3 floatOffset = Vector3.up * 2.0f;

        //PARTICLES
        public ParryEffects parryEffects;
        public WSwordEffect swordEffects;
        public AttackIndicator attackIndicator;
        public HitstopController hitstopController;

        //PHYSICS
        public Rigidbody rb;

        //BOSS VARS
        [Header("BOSS VARIABLES")]
        public int bossAttackSelector = 10;
        public bool bCanBeStunned = true;
        public BoxCollider slamCol;
        public bool bHasBowDrawn = false;
        public int shotCount = 4;
        public Transform firePoint;
        public MeshRenderer glaiveMesh;
        public MeshRenderer bowMesh;
        public WeaponSwitcher weaponSwitcher;
        public SwordColliderOverride colliderOverride;
        public GameObject teleportParticle, dustParticle;
        public float shotTimer = 1;
        public EnemyAudio enemyAudio;
        public GameEvent bossEvent;
        public GameEvent bossAggro;
        public GameEvent bossHalfway;
        public bool bisHalfway = false;

        //ATTACK SPEED VARIABLES
        public float previousAttackSpeed;
        public float attackSpeed;
        
        //CIRCLE TRACKING (used for the enemy tracker) and CloseDistanceTracking (used of heavyAttack)
        public bool bIsCircling = false;
        public bool bIsClosingDistance = false;
        
        #endregion
        
        #region Unity Monobehaviour Functions

        private void Start()
        {


            hitstopController = GameManager.instance.gameObject.GetComponent<HitstopController>();
            
            // Grab the enemy settings from the Game Manager > Game Settings > Enemy Settings
            enemySettings = GameManager.instance.gameSettings.enemySettings;
            
            // Get the enemy tracker
            enemyTracker = GameManager.instance.enemyTracker;

            // Set up animator
            animator = GetComponent<Animator>();

            // Set up nav mesh parameters
            navMeshAgent = GetComponent<NavMeshAgent>();
            
            // Set up Damage Controller
            eDamageController = GetComponent<EDamageController>();
            statHandler = new StatHandler(); // Stat handler = stats that can be modified
            
            // Assign stats based on the enemy type
            SetupEnemyType();
            
            // Set up damage controller continues
            eDamageController.Init(statHandler);
            eDamageController.EnableDamage();
            
            // Set up the attack speed variables
            attackSpeed = animator.GetFloat("AttackSpeedMultiplier");
            previousAttackSpeed = attackSpeed;

            // Start the enemy in an idle state
            OnIdle();

            if (!attackIndicator) attackIndicator = GetComponentInChildren<AttackIndicator>();

            rb = GetComponent<Rigidbody>();

            if (enemyType == EnemyType.BOSS) weaponSwitcher = GetComponent<WeaponSwitcher>();
        }

        private void Update()
        {
            spawnCheck.bSpawnMe = !bIsDead;
            //if (enemyType == EnemyType.BOSS && Keyboard.current.oKey.wasPressedThisFrame) OnBossArrowMove();
            //if (enemyType == EnemyType.BOSS && Keyboard.current.iKey.wasPressedThisFrame) OnBossTaunt();

            //if (enemyType == EnemyType.BOSS && Keyboard.current.lKey.wasPressedThisFrame) OnEnemyDeath();
        }

        #endregion

        #region Enemy Utility Funcitons

        // An override that is performed for every state change
        public override void SetState(EnemyState newEnemyState)
        {
            if(!eDamageController.enemyGuard.canParry) swordEffects.EndBlockEffect();
            if(!bIsUnblockable) swordEffects.EndUnblockableEffect();
            eDamageController.enemyGuard.bSuperArmour = false;

            if (bIsQuickBlocking) bIsQuickBlocking = false;
            if (bIsClosingDistance) bIsClosingDistance = false;

            if (enemyType != EnemyType.ARCHER)
            {
                meleeCollider.enabled = false;
            }

            if (enemyType == EnemyType.BOSS)
            {
                meleeCollider.enabled = false;
                if(!bHasBowDrawn) 
                    animator.SetLayerWeight(1, 0);
                if(!eDamageController.enemyGuard.isStunned) eDamageController.enemyGuard.canGuard = true;
                KBColOff();
                
            }
            
            ResetAnimationVariables();
            
            //Debug.LogWarning(newEnemyState.GetType().Name);
            base.SetState(newEnemyState);
        }
        
        // Assign stats based on the enemy type
        private void SetupEnemyType()
        {
            // enemySettings.enemyData = initial scriptable objects values
            
            switch (enemyType)
            {
                case EnemyType.SWORDSMAN:
                    statHandler.Init(enemySettings.swordsmanStats.enemyData); 
                    animator.SetFloat("ApproachSpeedMultiplier", enemySettings.swordsmanStats.enemyData.moveSpeed);
                    animator.SetFloat("CircleSpeedMultiplier", enemySettings.swordsmanStats.circleSpeed);
                    break;
                case EnemyType.ARCHER:
                    break;
                case EnemyType.GLAIVEWIELDER:
                    statHandler.Init(enemySettings.glaiveWielderStats.enemyData);
                    animator.SetFloat("ApproachSpeedMultiplier", enemySettings.glaiveWielderStats.enemyData.moveSpeed);
                    animator.SetFloat("CircleSpeedMultiplier", enemySettings.glaiveWielderStats.circleSpeed); 
                    break;
                case EnemyType.TUTORIALENEMY:
                    statHandler.Init(enemySettings.swordsmanStats.enemyData);
                    animator.SetFloat("ApproachSpeedMultiplier", enemySettings.swordsmanStats.enemyData.moveSpeed);
                    animator.SetFloat("CircleSpeedMultiplier", enemySettings.swordsmanStats.circleSpeed);
                    break;
                case EnemyType.BOSS:
                    statHandler.Init(enemySettings.bossStats.enemyData); 
                    animator.SetFloat("ApproachSpeedMultiplier", enemySettings.bossStats.enemyData.moveSpeed);
                    animator.SetFloat("CircleSpeedMultiplier", enemySettings.bossStats.circleSpeed);
                    break;
                case EnemyType.MINIBOSS:
                    statHandler.Init(enemySettings.bossStats.enemyData);
                    animator.SetFloat("ApproachSpeedMultiplier", enemySettings.bossStats.enemyData.moveSpeed);
                    animator.SetFloat("CircleSpeedMultiplier", enemySettings.bossStats.circleSpeed);
                    break;
                default:
                    Debug.LogError("Error: Could not find suitable enemy type");
                    break;
            }
        }

        public void ApplyHit(GameObject attacker)
        {
            if (attacker.GetComponent<AISystem>())
            {
                Debug.Log("Friendly Fire hit");
            }
            else if (attacker.GetComponent<PlayerController>())
            {
                if (bHasArmour)
                {
                    if (armourManager.DestroyPiece())
                    {
                        //hitstopController.Hitstop(.15f);
                        camImpulse.FireImpulse();
                        if (enemyType == EnemyType.BOSS)
                        {
                            IncreaseAttackSpeed(.05f);
                            EndState();
                            OnDodge();
                            CheckArmourLevel();
                            eDamageController.enemyGuard.ResetGuard();
                        }
                        //EndState();
                        //OnDodge(); 
                    }
                    else
                    {
                        hitstopController.Hitstop(.15f);
                        camImpulse.FireImpulse();
                        if (enemyType != EnemyType.BOSS) OnEnemyDeath();
                        else OnBossDeath();
                    }
                }
                else
                {
                    hitstopController.Hitstop(.15f);
                    camImpulse.FireImpulse();
                    OnEnemyDeath();
                }
            }
            else
            {
                Debug.LogWarning("Unknown attacker");
            }
        }

        public void ApplyImpulseForce(float f)
        {
            rb.AddForce(transform.forward * f, ForceMode.Impulse);
        }

        // Called from dodgestate
        public void DodgeImpulse(Vector3 lastDir, float force)
        {
            StopAllCoroutines();
            StartCoroutine(DodgeImpulseCoroutine(lastDir, force));
        }

        // Called from an animation event in lightattackstate
        public void DodgeImpulseAnimationEvent()
        {
            StopAllCoroutines();
            if (Vector3.Distance(transform.position, enemySettings.GetTarget().position) > enemySettings.shortRange)
            {
                StartCoroutine(DodgeImpulseCoroutine(transform.parent.forward, enemySettings.GetEnemyStatType(enemyType).dodgeForce));
            }
        }

        public void ForwardImpulseAnimEvent(float time)
        {
            StartCoroutine(DodgeImpulseCoroutine(Vector3.forward, 10f, time));
        }

        public void JumpImpulseAnimEvent(float time)
        {
            navMeshAgent.enabled = false; 
            StartCoroutine(JumpImpulseCoroutine(Vector3.forward, 20f, time));
        }
        public void PreJumpImpulseAnimEvent(float time)
        {
            navMeshAgent.enabled = false;
            StartCoroutine(DodgeImpulseCoroutine(new Vector3(0,1,1), 20f, time));
        }
        public void PreFlipImpulseAnimEvent(float time)
        {
            navMeshAgent.enabled = false;
            StartCoroutine(JumpImpulseCoroutine(new Vector3(0, 1, -1), 20f, time));
        }

        public void ImpulseWithDirection(float force, Vector3 dir)
        {
            StartCoroutine(DodgeImpulseCoroutine(dir, force, .7f));
        }
        public void ImpulseWithDirection(float force, Vector3 dir, float time)
        {
            if(PrintStates) Debug.Log("Knockback direction: " + dir);
            StartCoroutine(DodgeImpulseCoroutine(dir, force, time));
        }

        public void KBColOn()
        {
            kbController.KBColOn();
        }
        public void KBColOff()
        {
            kbController.KBColOff();
        }

        public void SlamColOn()
        {
            slamCol.enabled = true;
        }

        public void SlamColOff()
        {
            slamCol.enabled = false;
        }


        // Coroutines cannot exist in enemystate since it's not a monobehavior, so we handle it here
        private IEnumerator DodgeImpulseCoroutine(Vector3 lastDir, float force)
        {
            float dodgeTimer = .15f;
            while (dodgeTimer > 0f)
            {
                transform.Translate(lastDir.normalized * force * Time.deltaTime);
                
                dodgeTimer -= Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator DodgeImpulseCoroutine(Vector3 lastDir, float force, float timer)
        {
            float dodgeTimer = timer;
            while (dodgeTimer > 0f)
            {
                transform.Translate(lastDir.normalized * force * Time.deltaTime);

                dodgeTimer -= Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator JumpImpulseCoroutine(Vector3 lastDir, float force, float timer)
        {
            float dodgeTimer = timer;
            animator.applyRootMotion = false;
            while (dodgeTimer > 0f)
            {
                transform.Translate(lastDir.normalized * force * Time.deltaTime);
                if (Vector3.Distance(transform.position, enemySettings.GetTarget().position) <= enemySettings.veryShortRange)
                {
                    
                    break;
                }

                dodgeTimer -= Time.deltaTime;
                yield return null;
            }
            animator.applyRootMotion = true;
        }

        public void BeginUnblockable()
        {
            swordEffects.BeginUnblockableEffect();
            bIsUnblockable = true;
        }
        public void EndUnblockable()
        {
            swordEffects.EndUnblockableEffect();
            bIsUnblockable = false;
        }

        public void ShowIndicator()
        {
            attackIndicator.ShowIndicator();
        }
        
        public void ResetAnimationVariables()
        {
            // Set all movement variables to 0
            animator.SetFloat("MovementX", 0);
            //animator.SetFloat("MovementZ", 0);
            
            // Set all suitable animation bools to false
            animator.ResetTrigger("TriggerMovement");
            animator.ResetTrigger("TriggerGuardBreak");
            animator.ResetTrigger("TriggerDeath");
            animator.ResetTrigger("TriggerRecovery");
            animator.ResetTrigger("TriggerLightAttack");
            animator.ResetTrigger("TriggerCounterAttack");
            animator.ResetTrigger("TriggerDodge");
            animator.ResetTrigger("TriggerParryStun");
            animator.ResetTrigger("TriggerQuickBlock");
            animator.ResetTrigger("TriggerBlock");
            
            //Swordsman specific
            if (enemyType == EnemyType.BOSS)
            {
                animator.ResetTrigger("TriggerHeavyAttack");
                animator.ResetTrigger("TriggerJumpAttack");
                animator.ResetTrigger("TriggerCharge");
                animator.ResetTrigger("TriggerHeavyCombo");
                animator.ResetTrigger("TriggerArrowShot");
                animator.ResetTrigger("DrawBow");
                animator.ResetTrigger("SheathBow");
                animator.ResetTrigger("TriggerTaunt");
            }
            else if (enemyType == EnemyType.GLAIVEWIELDER)
            {
                animator.ResetTrigger("TriggerHeavyAttack");
                animator.ResetTrigger("TriggerJumpAttack");
                animator.ResetTrigger("TriggerCharge");
                animator.ResetTrigger("TriggerHeavyCombo");
            }
            else
            {
                animator.ResetTrigger("TriggerThrust");
            }
            
        }

        private bool EnemyDeathCheck()
        {
            if (bIsDead)
            {
                Debug.LogError(gameObject.name + " tried to switch states but is dead. State switch cancelled");
                return true;
            }

            return false;
        }
        
        // Called in parry enemy state
        public void IncreaseAttackSpeed(float increasedAmount)
        {
            if (attackSpeed + increasedAmount < 2f)
            {
                previousAttackSpeed = attackSpeed;
                attackSpeed += increasedAmount;
                animator.SetFloat("AttackSpeedMultiplier", attackSpeed);
            }
        }
        
        public void ReturnPreviousAttackSpeed()
        {
            attackSpeed = previousAttackSpeed;
            animator.SetFloat("AttackSpeedMultiplier", attackSpeed);
        }
        
        // Used in CircleEnemyState and enemy tracker to move the enemy onto another action
        // DO NOT IMPLEMENT A START CIRCLING STATE. Instead you should switch to CircleEnemyState
        public void StopCircling()
        {
            // Reset animation variables
            animator.SetFloat("MovementX", 0.0f);
            
            // Reset circling variable
            bIsCircling = false;
        }
        
        // Used to avoid the player's heavy attack if closing the distance
        public void AvoidHeavyAttack()
        {
            if(enemyType == EnemyType.BOSS)
            {
                if (bHasBowDrawn)
                {
                    animator.SetFloat("MovementZ", -1.0f);
                    OnDodge();
                }

            }
            else 
            { 
                if (bIsClosingDistance)
                {
                    animator.SetFloat("MovementZ", -1.0f);
                    OnDodge();
                }
            }
             
        }
        
        #endregion

        #region Animation Called Events

        // BUG-FIX: BREAKING THE STATE MACHINE RULES
        // The end state animation event in swordsman light attack was sometimes performing EndState for other events
        // This is a precautionary method to stop that from happening

        // Called in animation events to return the enemy's guard option
        public void StartIntangibility()
        {
            eDamageController.DisableDamage();
        }

        // Called in animation events to return the enemy's guard option
        public void StopIntangibility()
        {
            eDamageController.EnableDamage();
        }
        public void EnableNav()
        {
            navMeshAgent.enabled = true;
        }

        public void EndState()
        {
            if(PrintStates) Debug.LogWarning("End state called by anim");
            EnemyState.EndState();
        }

        public void StopRotating()
        {
            EnemyState.StopRotating();
        }
        public void StartRotating()
        { 
            EnemyState.StartRotating();
        }

        public void TurnOffBodyCol()
        {
            col.enabled = false;
        }
        
        public void EndStateAttack()
        {
            if (EnemyState.GetType() == typeof(SwordAttackEnemyState) || EnemyState.GetType() == typeof(ParryEnemyState) || EnemyState.GetType() == typeof(JumpAttackEnemyState) || EnemyState.GetType() == typeof(GlaiveAttackEnemyState))
            {
                EnemyState.EndState();
            }
            else
            {
                Debug.LogWarning("Warning: "+EnemyState.GetType().Name+" tried to EndState the wrong state, EndState cancelled");
            }
        }

        #endregion
        
        // ENEMY STATE SWITCHING INFO
        // Any time an enemy gets a combat maneuver called, their state will switch
        // Upon switching states, they override the EnemyState Start() method to perform their action

        #region Enemy Combat Manuervers

        public void OnSwordAttack()
        {
            if (EnemyDeathCheck()) return;
            SetState(new SwordAttackEnemyState(this));
        }

        public void OnGlaiveAttack()
        {
            if (EnemyDeathCheck()) return;
            SetState(new GlaiveAttackEnemyState(this));
        }

        public void OnJumpAttack()
        {
            bHasBowDrawn = false;
            if (EnemyDeathCheck()) return;
            SetState(new JumpAttackEnemyState(this));
        }

        public void OnSpecialAttack()
        {
            if (EnemyDeathCheck()) return;
        }
        
        public void OnQuickBlock()
        {
            bHasBowDrawn = false;
            if (EnemyDeathCheck()) return;
            if(!bIsQuickBlocking) SetState(new QuickBlockEnemyState(this));
        }

        public void OnBlock()
        {
            bHasBowDrawn = false;
            if (EnemyDeathCheck()) return;
            SetState(new BlockEnemyState(this));
        }

        public void OnParry()
        {
            if (EnemyDeathCheck()) return;
            SetState(new ParryEnemyState(this));
        }

        public void OnDodge()
        {
            if (EnemyDeathCheck()) return;
            SetState(new DodgeEnemyState(this));
        }
    
        #endregion

        #region Enemy Movement

        public void OnIdle()
        {
            if (EnemyDeathCheck()) return;
            SetState(new IdleEnemyState(this));
        }

        public void OnPatrol()
        {
        
        }

        public void OnChargePlayer()
        {
            if (EnemyDeathCheck()) return;
            SetState(new ChargeEnemyState(this));
        }

        public void OnApproachPlayer()
        {
            if (EnemyDeathCheck()) return;
            SetState(new ApproachPlayerEnemyState(this));
        }

        public void OnCloseDistance()
        {
            if (EnemyDeathCheck()) return;
            SetState(new CloseDistanceEnemyState(this));
        }

        public void OnCirclePlayer()
        {
            if (EnemyDeathCheck()) return;
            SetState(new CircleEnemyState(this));
        }

        public void OnEnemyStun()
        {
            if (EnemyDeathCheck()) return;
            SetState(new StunEnemyState(this));
        }
        
        public void OnParryStun()
        {
            if (EnemyDeathCheck()) return;
            SetState(new ParryStunEnemyState(this));
        }

        public void OnEnemyRecovery()
        {
            if (enemyType != EnemyType.BOSS)
                SetState(new RecoveryEnemyState(this)); 
        }

        public void OnEnemyDeath()
        {
            if(enemyType != EnemyType.BOSS)
                SetState(new DeathEnemyState(this));
            else
            {
                if (armourManager.armourCount <= 0)
                {
                    animator.SetLayerWeight(1, 0);
                    SetState(new DeathEnemyState(this));
                    bossEvent.Raise();
                    col.enabled = true;
                }
                else
                {
                    eDamageController.enemyGuard.ResetGuard();
                    armourManager.DestroyPiece();
                    armourManager.DestroyPiece();
                    IncreaseAttackSpeed(.05f);
                    IncreaseAttackSpeed(.05f);
                    CheckArmourLevel();


                }
            }
        }



        public void OnBossDeath()
        {
            if (armourManager.armourCount > 0)
            {
                eDamageController.enemyGuard.ResetGuard();
                armourManager.DestroyPiece();
                armourManager.DestroyPiece();
                IncreaseAttackSpeed(.05f);
                IncreaseAttackSpeed(.05f);
                CheckArmourLevel();
            }
            else
            {
                animator.SetLayerWeight(1, 0);
                SetState(new DeathEnemyState(this));
                bossEvent.Raise();
                col.enabled = true;
                // statHandler.maxGuard = 5;
                // eDamageController.enemyGuard.ResetGuard();
                // CheckArmourLevel();
            }
        }

        public void OnEnemyRewind() 
        {
            SetState(new RewindEnemyState(this));
        }

        public void OnBossArrowMove()
        {
            SetState(new BossArrowMoveState(this));
        }

        public void OnBossArrowFire()
        {
            SetState(new BossArrowFireState(this));
        }

        public void OnBossTaunt()
        {
            SetState(new BossTauntEnemyState(this));
        }

        public void CheckArmourLevel()
        {
            if (armourManager.armourCount <= 3)
            {
                OnDodge();
                //OnGlaiveAttack();
                statHandler.maxGuard += 40;
            }
            else if (armourManager.armourCount <= 6)
            {
                OnDodge();
               // OnBossArrowMove();
                statHandler.maxGuard += 20;
            }
            else
            {
                OnDodge();
                statHandler.maxGuard += 20;
                //OnApproachPlayer();
            }

            if(armourManager.armourCount < 4 && !bisHalfway)
            {
                bisHalfway = true;
                bossHalfway.Raise();
            }


        }

        public void BossGlaiveColOn()
        {
            if (enemyType == EnemyType.BOSS) colliderOverride.ColOn(1);
        }
        public void BossGlaiveColOff()
        {
            if (enemyType == EnemyType.BOSS) colliderOverride.ColOff(1);
        }

        public void TauntTeleport()
        {
            Vector3 teleportVector = Camera.main.transform.forward * 8;
            teleportVector.y = 0;
            transform.position = (enemySettings.GetTarget().position - (teleportVector));
            OnJumpAttack();
        }

        public void DropSmoke()
        {
            Instantiate(teleportParticle, transform.position + (transform.forward*2), Quaternion.identity);
            enemyAudio.Smoke();

        }
        public void DropDirtExplosion()
        {
            //Instantiate(dustParticle, transform.position + (transform.forward * 2), Quaternion.identity);
        }

        #endregion

        private void OnDisable()
        { 
            GameManager.instance.enemyTracker.RemoveEnemy(rb.gameObject.transform);
        }

        private void OnDestroy()
        {
            GameManager.instance.enemyTracker.RemoveEnemy(rb.gameObject.transform);
        }
        
    }

}
