using System;
using Player.Animation;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using UnityEngine;
using Object = UnityEngine.Object;

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

        private bool m_CanBlock = true;
        private float m_HeavyAttackRemainingChargeTime;
        private float m_HeavyAttackRequiredChargeTime = 1.5f;

        private bool m_GleamTimerFinished = false;
        private float m_GleamTimer;
        private float m_GleamPrecedeTime = 0.4f;
        //private bool m_HasPerformedAttack;

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
            this.m_CameraController = this.GetComponent<ICameraController>();
            this.m_CombatController = this.GetComponent<ICombatController>();
            this.m_HitstopController = Object.FindFirstObjectByType<HitstopController>();
            this.m_PlayerAttackState = this.GetComponent<IPlayerState>().PlayerAttackState;
            this.m_AnimationDispatcher = this.GetComponent<IPlayerAnimationDispatcher>();
            this.m_WeaponSystem = this.GetComponent<IWeaponSystem>();
            this.m_BlockingAttackHandler = this.GetComponent<BlockingAttackHandler>();

            this.m_HeavyAttackRemainingChargeTime = this.m_HeavyAttackRequiredChargeTime;
            m_GleamTimer = m_HeavyAttackRequiredChargeTime - m_GleamPrecedeTime;
        }

        private void Update()
        {
            if (this.IsPaused)
                return;
            
            if (this.m_PlayerAttackState.IsHeavyAttackCharging) 
                this.TickHeavyTimer();
            else if (Mathf.Approximately(this.m_HeavyAttackRemainingChargeTime, this.m_HeavyAttackRequiredChargeTime))
                this.m_HeavyAttackRemainingChargeTime = this.m_HeavyAttackRequiredChargeTime;
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -
        
        // NOTE: IPlayerAttackHandler.Attack() is a release input option (e.g. OnMouseUp)
        void IPlayerAttackHandler.Attack()
        {
            if (!this.m_WeaponSystem.IsWeaponEquipped()) return;
            
            if (this.m_PlayerAttackState.IsHeavyAttackCharging) // HEAVY ATTACK
            {
                Invoke("PerformHeavyAttack", m_HeavyAttackRemainingChargeTime);
            }
            else if(this.m_PlayerAttackState.CanAttack) // LIGHT ATTACK
            {
                this.m_CombatController.AttemptLightAttack();
                //this.m_HasPerformedAttack = true;
                
                if (this.m_HitstopController.bIsSlowing)
                    this.m_HitstopController.CancelEffects();
            }
            
            //this.m_HasPerformedAttack = false;
        }

        void IPlayerAttackHandler.DrawSword() 
            => this.m_CombatController?.DrawSword();

        // Tech-Debt: #35 - PlayerFunctions will be refactored to mitigate large class bloat.
        void IPlayerAttackHandler.EndBlock()
            => this.m_BlockingAttackHandler.EndBlock();

        void IPlayerAttackHandler.EndParryAction()
        {
            this.m_PlayerAttackState.ParryStunned = false;
            this.m_HitstopController.CancelEffects();
        }
        
        void IPlayerAttackHandler.StartHeavy()
        {
            if (!this.m_WeaponSystem.IsWeaponEquipped()) return;
            
            this.m_HeavyAttackRemainingChargeTime = this.m_HeavyAttackRequiredChargeTime;
            this.StartHeavyAttack();
        }

        // Note: This behaviour is not implemented, but will be open for future use.
        void IPlayerAttackHandler.StartHeavyAlternative()
            => throw new NotImplementedException();
        
        private bool CanBlock() => m_CombatController.IsSwordDrawn && this.m_CanBlock;
        
        void IPlayerAttackHandler.StartBlock()
        {
            if (!CanBlock())
                return;

            if (this.m_PlayerAttackState.ParryStunned)
                ((IPlayerAttackHandler)this).EndParryAction();
            
            this.m_BlockingAttackHandler.StartBlock();
        }
        
        void IPlayerAttackHandler.ResetAttack()
        {
            this.m_PlayerAttackState.CanAttack = true;
            //this.m_HasPerformedAttack = false;
            
            this.m_CombatController.EndAttack();
            this.m_CombatController.ResetAttackCombo();
        }
        
        private void PerformHeavyAttack()
        {
            this.m_ShowHeavyTutorialEvent.Raise();
            this.m_EndHeavyTelegraphEvent.Raise();

            this.m_PlayerAttackState.IsHeavyAttackCharging = false;
            this.m_PlayerAttackState.IsWeaponSheathed = false;
            this.m_GleamTimerFinished = false;
            this.m_HeavyAttackRemainingChargeTime = this.m_HeavyAttackRequiredChargeTime;
            
            // Commenting line below from merge conflict with camera rework
            this.m_PlayerAnimationComponent.TriggerHeavyAttack();
            
            // Create gleam effect
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
        
        private void TickHeavyTimer()
        {
            CountdownHeavyTimer();
            
            if(!m_GleamTimerFinished)
                CountdownGleamTimer();
        }
        
        private void StartHeavyAttack()
        {
            // Commenting line below from merge conflict with camera rework
            //if (!this.m_PlayerAttackState.CanAttack /*&& this.m_Animator.GetBool("HeavyAttackHeld")*/)
            if (!this.m_PlayerAttackState.CanAttack 
                && this.m_AnimationDispatcher.Check(PlayerAnimationCheckState.HeavyAttackHeld))
                return;
            
            this.m_ShowHeavyTelegraphEvent.Raise();
            
            this.m_PlayerAttackState.IsWeaponSheathed = true;
            this.m_PlayerAttackState.IsHeavyAttackCharging = true;
              
            // Commenting line below from merge conflict with camera rework
            this.m_PlayerAnimationComponent.ResetAttackParameters();
            this.m_PlayerAnimationComponent.ChargeHeavyAttack(true);
            
            this.m_AnimationDispatcher.Dispatch(PlayerAnimationEventStates.StartHeavyAttackHeld);
            
            // Ends the camera roll
            // TODO: Fix the error from the line below
            this.m_CameraController.EndCameraAction();
        }

        // Tech-Debt: #36 - Create a simple universal timer to keep timer behaviour consistent.
        private void CountdownHeavyTimer()
        {
            this.m_HeavyAttackRemainingChargeTime -= Time.deltaTime;
            
            if (!(this.m_HeavyAttackRemainingChargeTime <= 0)) return;
            
            //this.m_HasPerformedAttack = true;
        }
        
        private void CountdownGleamTimer()
        {
            m_GleamTimer -= Time.deltaTime;
            
            if (!(m_GleamTimer <= 0)) return;
            
            m_GleamTimerFinished = true;
            m_GleamTimer = m_HeavyAttackRequiredChargeTime - m_GleamPrecedeTime;
            m_BlockingAttackHandler.m_BlockingEffects.PlayGleam(); // TODO: Change this to use interface instead.
        }
        
        #endregion Methods
        
    }
    
}
