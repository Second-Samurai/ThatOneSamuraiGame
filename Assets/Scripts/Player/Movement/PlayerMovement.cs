using System;
using Player.Animation;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{

    public class PlayerMovementInitializerData
    {

        #region - - - - - - Properties - - - - - -

        public ICameraController CameraController { get; private set; }
        
        public ILockOnObserver LockOnObserver { get; private set; }

        #endregion Properties

        #region - - - - - - Constructors - - - - - -

        public PlayerMovementInitializerData(ICameraController cameraController, ILockOnObserver lockOnObserver)
        {
            this.CameraController = cameraController;
            this.LockOnObserver = lockOnObserver;
        }

        #endregion Constructors
  
    }
    
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : 
        PausableMonoBehaviour, 
        IInitialize<PlayerMovementInitializerData>,
        IPlayerMovement, 
        IPlayerDodgeMovement
    {
        
        #region - - - - - - Fields - - - - - -
        
        [RequiredField]
        [SerializeField]
        private PlayerAnimationComponent m_PlayerAnimationComponent;
        private Rigidbody m_Rigidbody;
        
        private ICameraController m_CameraController;
        private IDamageable m_PlayerDamage;
        private IPlayerAttackHandler m_PlayerAttackHandler;
        private BlockingAttackHandler m_BlockingAttackHandler;
        
        // Player data containers
        private PlayerAttackState m_PlayerAttackState;
        private PlayerMovementDataContainer m_PlayerMovementDataContainer;
        private PlayerTargetTrackingState m_PlayerTargetTrackingState;
        private PlayerSpecialActionState m_PlayerSpecialActionState;
        
        // Player states
        private IPlayerMovementState m_CurrentMovementState;
        private IPlayerMovementState m_NormalMovement;
        private IPlayerMovementState m_LockOnMovement;
        private IPlayerMovementState m_FinisherMovement;
        
        private bool m_IsMovementEnabled = true;
        private bool m_IsRotationEnabled = true;
        private bool m_IsSprinting = false;
        private float m_SprintDuration = 0.0f;
        private float m_RotationSpeed = 4f;
        
        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public bool IsSprinting => this.m_IsSprinting;

        #endregion Properties
  
        #region - - - - - - Initializers - - - - - -

        public void Initialize(PlayerMovementInitializerData initializerData)
        {
            this.m_CameraController = 
                initializerData.CameraController 
                ?? throw new ArgumentNullException(nameof(initializerData.CameraController));
            this.m_Rigidbody = this.GetComponent<Rigidbody>() ??
                               throw new ArgumentNullException(nameof(this.m_Rigidbody));
            
            // Bind to observers
            initializerData.LockOnObserver.OnLockOnDisable.AddListener(() => 
                ((IPlayerMovement)this).SetState(PlayerMovementStates.Normal));
        }

        #endregion Initializers
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PlayerAnimationComponent = this.GetComponent<PlayerAnimationComponent>();
            this.m_PlayerTargetTrackingState = this.GetComponent<IPlayerState>().PlayerTargetTrackingState;
            this.m_PlayerAttackHandler = this.GetComponent<IPlayerAttackHandler>();
            this.m_PlayerDamage = this.GetComponent<IDamageable>();
            this.m_BlockingAttackHandler = this.GetComponent<BlockingAttackHandler>();

            IMovementAnimationEvents _AnimationEvents = this.GetComponent<IMovementAnimationEvents>();
            _AnimationEvents.OnEnableMovement.AddListener(((IPlayerMovement)this).EnableMovement);
            _AnimationEvents.OnDisableMovement.AddListener(((IPlayerMovement)this).DisableMovement);
            _AnimationEvents.OnEnableRotation.AddListener(((IPlayerMovement)this).EnableRotation);
            _AnimationEvents.OnDisableRotation.AddListener(((IPlayerMovement)this).DisableRotation);
            _AnimationEvents.OnStartDodge.AddListener(this.StartDodging);
            _AnimationEvents.OnEndDodge.AddListener(this.EndDodging);
            _AnimationEvents.OnBlockDodge.AddListener(this.BlockDodge);
            _AnimationEvents.OnResetDodge.AddListener(this.ResetDodge);
            _AnimationEvents.OnLockMoveInput.AddListener(this.LockMoveInput);
            _AnimationEvents.OnUnlockMoveInput.AddListener(this.UnlockMoveInput);

            IPlayerState _PlayerState = this.GetComponent<IPlayerState>();
            this.m_PlayerAttackState = _PlayerState.PlayerAttackState;
            this.m_PlayerMovementDataContainer = _PlayerState.PlayerMovementDataContainer;
            this.m_PlayerSpecialActionState = _PlayerState.PlayerSpecialActionState;

            // Initialize Movement States
            this.m_NormalMovement = new PlayerNormalMovement(
                this.GetComponent<IPlayerAttackHandler>(),
                this.m_PlayerAttackState,
                this.m_CameraController,
                this.m_PlayerMovementDataContainer,
                this.m_PlayerAnimationComponent, 
                this.transform,
                this);
            this.m_LockOnMovement = new PlayerLockOnMovement(
                this.GetComponent<IPlayerAttackHandler>(),
                this.m_PlayerAttackState,
                this.m_PlayerAnimationComponent, 
                this.transform,
                this.m_PlayerMovementDataContainer,
                this.m_PlayerTargetTrackingState,
                this);
            this.m_FinisherMovement = new PlayerFinishMovement(
                this.m_PlayerMovementDataContainer,
                this.m_PlayerAnimationComponent,
                this.transform);
            
            this.m_CurrentMovementState = this.m_NormalMovement;
            
            ((IPlayerDodgeMovement)this).EnableDodge();
        }

        private void Update()
        {
            if (this.IsPaused || !this.m_IsMovementEnabled)
                return;

            this.m_CurrentMovementState.CalculateMovement();
            this.m_CurrentMovementState.ApplyMovement();
            
            if (IsSprinting)
                TickSprintDuration();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -
        
        // --------------------------------
        // Dodge
        // --------------------------------
        
        public void Dodge()
            => this.m_CurrentMovementState.PerformDodge();

        void IPlayerDodgeMovement.EnableDodge()
            => this.m_PlayerMovementDataContainer.CanDodge = true;

        void IPlayerDodgeMovement.DisableDodge()
            => this.m_PlayerMovementDataContainer.CanDodge = false;
        
        private void BlockDodge() 
            => this.m_PlayerSpecialActionState.CanDodge = false;
        
        private void ResetDodge()
        {
            this.m_PlayerSpecialActionState.CanDodge = true;
            this.m_PlayerSpecialActionState.IsDodging = false;
        }

        private void StartDodging()
        {
            this.m_PlayerAttackState.CanAttack = false;
            this.m_PlayerSpecialActionState.IsDodging = true;
            this.m_PlayerAttackState.IsHeavyAttackCharging = false;
            this.m_PlayerAttackState.IsWeaponSheathed = false;
            
            this.m_PlayerDamage.DisableDamage();
            this.m_BlockingAttackHandler.DisableBlock();
            this.m_PlayerAttackHandler.ResetAttack();
        }

        private void EndDodging()
        {
            this.m_PlayerAttackState.CanAttack = true;
            // this.m_PlayerSpecialActionState.IsDodging = false;
            
            this.m_PlayerDamage.EnableDamage();
            this.m_BlockingAttackHandler.EnableBlock();
            this.m_PlayerAttackHandler.ResetAttack();
        }
        
        // --------------------------------
        // Movement
        // --------------------------------

        public float GetSprintDuration() => m_SprintDuration;
        
        public void CancelMove()
        {
            StopAllCoroutines(); 
            ((IPlayerMovement)this).EnableMovement();
            ((IPlayerMovement)this).EnableRotation();
            this.m_Rigidbody.linearVelocity = Vector3.zero;
            this.m_PlayerAnimationComponent.SetRootMotion(true);
        }

        void IPlayerMovement.DisableMovement()
            => this.m_IsMovementEnabled = false;

        void IPlayerMovement.EnableMovement()
            => this.m_IsMovementEnabled = true;

        void IPlayerMovement.PreparePlayerMovement(Vector2 moveDirection)
        {
            if (moveDirection == Vector2.zero)
            {
                this.m_CurrentMovementState.PerformSprint(false);
            }
            
            this.m_CurrentMovementState.SetInputDirection(moveDirection);
        }

        void IPlayerMovement.PrepareSprint(bool isSprinting)
        {
            this.m_CurrentMovementState.PerformSprint(isSprinting);
            this.m_CameraController.SelectCamera(isSprinting 
                ? SceneCameras.FollowSprintPlayer 
                : SceneCameras.FollowPlayer);

            this.m_IsSprinting = isSprinting;

            // // Toggle sprinting animation
            // if (this.m_InputDirection == Vector2.zero || !this.m_IsSprinting)
            // {
            //     m_SprintDuration = 0.0f;
            //     m_PlayerAnimationComponent.SetSprinting(false);
            // }
            // else
            // {
            //     m_SprintDuration = 0.0f;
            //     m_PlayerAnimationComponent.SetSprinting(true);
            // }
        }
        
        private void TickSprintDuration()
        {
            m_SprintDuration += Time.deltaTime;
        }
        
        private void LockMoveInput() // This is not being used anywhere
        { 
            if (this.m_PlayerMovementDataContainer.IsMovementLocked)
                return;

            this.m_PlayerMovementDataContainer.IsMovementLocked = true;
            this.StartDodging(); // Note: I find it unusual that Dodging is invoked when not moving the character.
        }
        
        private void UnlockMoveInput()
        {
            if (!this.m_PlayerMovementDataContainer.IsMovementLocked)
                return;

            this.m_PlayerMovementDataContainer.IsMovementLocked = false;
            this.EndDodging();
        }
        
        // --------------------------------
        // Rotation
        // --------------------------------

        void IPlayerMovement.DisableRotation()
            => this.m_IsRotationEnabled = false;

        void IPlayerMovement.EnableRotation()
            => this.m_IsRotationEnabled = true;
        
        // --------------------------------
        // State Management
        // --------------------------------
        
        void IPlayerMovement.SetState(PlayerMovementStates state)
        {
            // A check is performed as the swordsman implementation might cause a stackoverflow,
            //  by running this operation multiple times upon its death.
            if (this.m_CurrentMovementState != null && this.m_CurrentMovementState.GetState() == state) return;
            
            if (state == PlayerMovementStates.Normal)
                this.m_CurrentMovementState = this.m_NormalMovement;
            else if (state == PlayerMovementStates.LockOn)
                this.m_CurrentMovementState = this.m_LockOnMovement;
            else if (state == PlayerMovementStates.Finisher)
                this.m_CurrentMovementState = this.m_FinisherMovement;
            
            Debug.Log("Movement State is: " + state);
        }

        #endregion Methods
        
    }
    
}
