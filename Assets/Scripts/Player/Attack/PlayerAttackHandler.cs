using System;
using Player.Animation;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Attack
{

    public class PlayerAttackInitializerData
    {

        #region - - - - - - Properties - - - - - -

        public ICameraController CameraController { get; private set; }

        #endregion Properties

        #region - - - - - - Constructors - - - - - -

        public PlayerAttackInitializerData(ICameraController cameraController) 
            => this.CameraController = cameraController;

        #endregion Constructors
  
    }
    
    public class PlayerAttackHandler : 
        PausableMonoBehaviour, 
        IInitialize<PlayerAttackInitializerData>,
        IPlayerAttackHandler
    {

        #region - - - - - - Fields - - - - - -
        
        // Component Fields
        private PlayerAnimationComponent m_PlayerAnimationComponent;
        private ICameraController m_CameraController;
        private ICombatController m_CombatController;
        private HitstopController m_HitstopController;
        private PlayerAttackState m_PlayerAttackState;
        private IPlayerAnimationDispatcher m_AnimationDispatcher;
        private BlockingAttackHandler m_BlockingAttackHandler;
        private IWeaponSystem m_WeaponSystem;

        [SerializeField] 
        private GameEvent m_ShowHeavyTutorialEvent; // This event feels out of place for this component.
        [SerializeField] 
        private GameEvent m_ShowHeavyTelegraphEvent; // This event feels out of place for this component.
        [SerializeField] 
        private GameEvent m_EndHeavyTelegraphEvent; // This event feels out of place for this component. 

        // private float m_HeavyAttackRemainingChargeTime;
        private readonly float m_HeavyAttackChargeTime = 1.5f;
        private EventTimer m_HeavyAttackTimer;

        // Gleam Effect Fields
        // private bool m_GleamTimerFinished;
        // private float m_GleamTimer;
        // private readonly float m_GleamPrecedeTime = 0.4f;

        #endregion Fields

        #region - - - - - - Initializers - - - - - -

        public void Initialize(PlayerAttackInitializerData initializerData)
        {
            this.m_CameraController = 
                initializerData.CameraController 
                    ?? throw new ArgumentNullException(nameof(initializerData.CameraController));
        }

        #endregion Initializers
  
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PlayerAnimationComponent = this.GetComponent<PlayerAnimationComponent>();
            this.m_CombatController = this.GetComponent<ICombatController>();
            this.m_HitstopController = FindFirstObjectByType<HitstopController>();
            this.m_PlayerAttackState = this.GetComponent<IPlayerState>().PlayerAttackState;
            this.m_AnimationDispatcher = this.GetComponent<IPlayerAnimationDispatcher>();
            this.m_WeaponSystem = this.GetComponent<IWeaponSystem>();
            this.m_BlockingAttackHandler = this.GetComponent<BlockingAttackHandler>();

            // this.m_HeavyAttackRemainingChargeTime = this.m_HeavyAttackChargeTime;
            this.m_HeavyAttackTimer = new EventTimer(
                this.m_HeavyAttackChargeTime,
                Time.deltaTime,
                this.m_BlockingAttackHandler.m_BlockingEffects.PlayGleam,
                false,
                false);
            // m_GleamTimer = m_HeavyAttackChargeTime - m_GleamPrecedeTime;

            IAttackAnimationEvents _AnimationEvents = this.GetComponent<IAttackAnimationEvents>();
            _AnimationEvents.OnParryStunStateStart.AddListener(() => this.m_PlayerAttackState.ParryStunned = true);
            _AnimationEvents.OnParryStunStateEnd.AddListener(() => this.m_PlayerAttackState.ParryStunned = false);
        }

        private void Update()
        {
            if (this.IsPaused)
                return;
            
            if (this.m_PlayerAttackState.IsHeavyAttackCharging) 
                this.m_HeavyAttackTimer.TickTimer();
            // else if (Mathf.Approximately(this.m_HeavyAttackRemainingChargeTime, this.m_HeavyAttackChargeTime))
            //     this.m_HeavyAttackRemainingChargeTime = this.m_HeavyAttackChargeTime;
        }

        #endregion Lifecycle Methods

        #region - - - - - - General Methods - - - - - -

        // NOTE: IPlayerAttackHandler.Attack() is a release input option (e.g. OnMouseUp)
        void IPlayerAttackHandler.Attack()
        {
            if (!this.m_WeaponSystem.IsWeaponEquipped()) return;
            
            if (this.m_PlayerAttackState.IsHeavyAttackCharging) // HEAVY ATTACK
                this.PerformHeavyAttack();
                // Invoke("PerformHeavyAttack", m_HeavyAttackRemainingChargeTime);
            else if(this.m_PlayerAttackState.CanAttack) // LIGHT ATTACK
            {
                this.m_CombatController.AttemptLightAttack();
                
                if (this.m_HitstopController.bIsSlowing)
                    this.m_HitstopController.CancelEffects();
            }
        }

        // TODO: Belongs to weapon system
        void IPlayerAttackHandler.DrawSword() 
            => this.m_CombatController?.DrawSword();
        
        void IPlayerAttackHandler.ResetAttack()
        {
            this.m_PlayerAttackState.CanAttack = true;
            //this.m_HasPerformedAttack = false;
            
            this.m_CombatController.EndAttack();
            this.m_CombatController.ResetAttackCombo();
        }

        #endregion General Methods

        #region - - - - - - Parry and Block Methods - - - - - -

        // Tech-Debt: #35 - PlayerFunctions will be refactored to mitigate large class bloat.
        void IPlayerAttackHandler.EndBlock()
            => this.m_BlockingAttackHandler.EndBlock();
        
        void IPlayerAttackHandler.StartBlock() 
            => this.m_BlockingAttackHandler.StartBlock();

        void IPlayerAttackHandler.EndParryAction()
        {
            this.m_PlayerAttackState.ParryStunned = false;
            this.m_HitstopController.CancelEffects();
        }

        #endregion Parry and Block Methods
  
        #region - - - - - - Heavy Attack Methods - - - - - -

        void IPlayerAttackHandler.StartHeavy()
        {
            if (!this.m_WeaponSystem.IsWeaponEquipped()) return;
            
            // Commenting line below from merge conflict with camera rework
            //if (!this.m_PlayerAttackState.CanAttack /*&& this.m_Animator.GetBool("HeavyAttackHeld")*/)
            if (!this.m_PlayerAttackState.CanAttack 
                && this.m_AnimationDispatcher.Check(PlayerAnimationCheckState.HeavyAttackHeld))
                return;
            
            this.m_HeavyAttackTimer.StartTimer();
            
            this.m_ShowHeavyTelegraphEvent.Raise();
            
            this.m_PlayerAttackState.IsWeaponSheathed = true;
            this.m_PlayerAttackState.IsHeavyAttackCharging = true;
              
            // Commenting line below from merge conflict with camera rework
            this.m_PlayerAnimationComponent.ResetAttackParameters();
            this.m_PlayerAnimationComponent.ChargeHeavyAttack(true);
            
            this.m_AnimationDispatcher.Dispatch(PlayerAnimationEventStates.StartHeavyAttackHeld);
            
            // Ends the camera roll
            // TODO: Fix the error from the line below
            GameLogger.Log(this.m_CameraController.ToString());
            this.m_CameraController.EndCameraAction();
        }

        // Note: This behaviour is not implemented, but will be open for future use.
        void IPlayerAttackHandler.StartHeavyAlternative()
            => throw new NotImplementedException();
        
        private void PerformHeavyAttack()
        {
            this.m_ShowHeavyTutorialEvent.Raise();
            this.m_EndHeavyTelegraphEvent.Raise();

            this.m_PlayerAttackState.IsHeavyAttackCharging = false;
            this.m_PlayerAttackState.IsWeaponSheathed = false;
            
            this.m_HeavyAttackTimer.StopTimer();
            this.m_HeavyAttackTimer.ResetTimer();
            
            this.m_PlayerAnimationComponent.TriggerHeavyAttack();
            
            //this.m_PlayerFunctions.parryEffects.PlayGleam();
            
            this.m_AnimationDispatcher.Dispatch(PlayerAnimationEventStates.EndHeavyAttachHeld);
            
            // Rolls the camera
            // TODO: Fix the error from the code below
            IFreelookCameraController _FreeLookCamera = this.m_CameraController
                .GetCamera(SceneCameras.FreeLook)
                .GetComponent<IFreelookCameraController>();
            CameraRollAction _CameraRoll = new CameraRollAction(_FreeLookCamera, this);
            this.m_CameraController.SetCameraAction(_CameraRoll);
        }
        
        // Tech-Debt: #36 - Create a simple universal timer to keep timer behaviour consistent.
        // private void CountdownHeavyTimer()
        // {
        //     this.m_HeavyAttackRemainingChargeTime -= Time.deltaTime;
        //     
        //     if (!(this.m_HeavyAttackRemainingChargeTime <= 0)) return;
        // }

        // private void TickHeavyTimer()
        // {
        //     this.CountdownHeavyTimer();
        //     
        //     if(!m_GleamTimerFinished)
        //         this.CountdownGleamTimer();
        // }
        
        // private void CountdownGleamTimer()
        // {
        //     this.m_GleamTimer -= Time.deltaTime;
        //     
        //     if (!(this.m_GleamTimer <= 0)) return;
        //     
        //     this.m_GleamTimerFinished = true;
        //     this.m_GleamTimer = m_HeavyAttackChargeTime - this.m_GleamPrecedeTime;
        //     this.m_BlockingAttackHandler.m_BlockingEffects.PlayGleam(); // TODO: Change this to use interface instead.
        // }
        
        #endregion Heavy Attack Methods
        
    }
    
}
