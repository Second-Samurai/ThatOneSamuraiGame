using System;
using Player.Animation;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Attack
{

    public class PlayerAttackSystem : 
        PausableMonoBehaviour, 
        IInitialize<PlayerAttackInitializerData>,
        IPlayerAttackHandler
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField] private SphereCollider m_AttackCollider;
        
        // Component Fields
        private PlayerAnimationComponent m_PlayerAnimationComponent;
        private ICameraController m_CameraController;
        private ICombatController m_CombatController;
        private HitstopController m_HitstopController;
        private PlayerAttackState m_PlayerAttackState;
        private CloseEnemyGuideControl m_NearEnemyMovementGuideControl;
        private IPlayerAnimationDispatcher m_AnimationDispatcher;
        private IWeaponSystem m_WeaponSystem;
        
        // Attack Component Fields
        private BlockingAttackHandler m_BlockingAttackHandler;
        private LightAttackHandler m_LightAttackHandler;
        
        [SerializeField] 
        private GameEvent m_ShowHeavyTutorialEvent; // This event feels out of place for this component.
        [SerializeField] 
        private GameEvent m_ShowHeavyTelegraphEvent; // This event feels out of place for this component.
        [SerializeField] 
        private GameEvent m_EndHeavyTelegraphEvent; // This event feels out of place for this component. 

        private readonly float m_HeavyAttackChargeTime = 1.5f;
        private EventTimer m_HeavyAttackTimer;

        private bool m_CanAttack;

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
            this.m_LightAttackHandler = this.GetComponent<LightAttackHandler>();

            this.m_HeavyAttackTimer = new EventTimer(
                this.m_HeavyAttackChargeTime,
                Time.deltaTime,
                this.m_BlockingAttackHandler.m_BlockingEffects.PlayGleam,
                false,
                false);

            IAttackAnimationEvents _AnimationEvents = this.GetComponent<IAttackAnimationEvents>();
            _AnimationEvents.OnParryStunStateStart.AddListener(() => this.m_PlayerAttackState.ParryStunned = true);
            _AnimationEvents.OnParryStunStateEnd.AddListener(() => this.m_PlayerAttackState.ParryStunned = false)
            _AnimationEvents.OnAttackStart.AddListener(this.PrepareAttack);
            _AnimationEvents.OnAttackEnd.AddListener(this.EndAttack);
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

        private void PrepareAttack()
        {
            this.m_BlockingAttackHandler.DisableBlock();
            this.m_AttackCollider.enabled = true;
            this.m_NearEnemyMovementGuideControl.MoveToNearestEnemy();
        }

        // NOTE: IPlayerAttackHandler.Attack() is a release input option (e.g. OnMouseUp)
        void IPlayerAttackHandler.Attack()
        {
            if (!this.m_WeaponSystem.IsWeaponEquipped() || !this.m_CanAttack) return;
            
            if (this.m_PlayerAttackState.IsHeavyAttackCharging) // HEAVY ATTACK
                this.PerformHeavyAttack();
            else
                this.m_LightAttackHandler.QueueLightAttack();
            
            if (this.m_HitstopController.bIsSlowing)
                this.m_HitstopController.CancelEffects();
        }

        private void EndAttack()
        {
            this.m_BlockingAttackHandler.EnableBlock();
            this.m_AttackCollider.enabled = false;
        }

        public void EnableAttack()
            => this.m_CanAttack = true;

        public void DisableAttack()
            => this.m_CanAttack = false;
        
        void IPlayerAttackHandler.ResetAttack()
        {
            this.m_PlayerAttackState.CanAttack = true;
            
            this.m_CombatController.EndAttack();
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
            this.m_AnimationDispatcher.Dispatch(PlayerAnimationEventStates.EndHeavyAttachHeld);
            
            // Rolls the camera
            // TODO: Fix the error from the code below
            IFreelookCameraController _FreeLookCamera = this.m_CameraController
                .GetCamera(SceneCameras.FreeLook)
                .GetComponent<IFreelookCameraController>();
            CameraRollAction _CameraRoll = new CameraRollAction(_FreeLookCamera, this);
            this.m_CameraController.SetCameraAction(_CameraRoll);
        }

        #endregion Heavy Attack Methods
        
    }
    
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

    
}
