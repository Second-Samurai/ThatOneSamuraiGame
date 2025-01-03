using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{

    public class PlayerMovementInitializerData
    {

        #region - - - - - - Properties - - - - - -

        public ICameraController CameraController { get; private set; }

        #endregion Properties

        #region - - - - - - Constructors - - - - - -

        public PlayerMovementInitializerData(ICameraController cameraController) 
            => this.CameraController = cameraController;

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
        private Animator m_Animator;
        
        private ICameraController m_CameraController;
        
        // Player data containers
        private PlayerAttackState m_PlayerAttackState;
        private PlayerMovementState m_PlayerMovementState;
        private PlayerTargetTrackingState m_PlayerTargetTrackingState;
        
        // Player states
        private IPlayerMovementState m_CurrentMovementState;
        private IPlayerMovementState m_NormalMovement;
        private IPlayerMovementState m_LockOnMovement;
        private IPlayerMovementState m_FinisherMovement;
        
        private bool m_IsMovementEnabled = true;
        private bool m_IsRotationEnabled = true;

        #endregion Fields
        
        #region - - - - - - Initializers - - - - - -

        public void Initialize(PlayerMovementInitializerData initializerData)
        {
            this.m_CameraController = 
                initializerData.CameraController 
                ?? throw new ArgumentNullException(nameof(initializerData.CameraController));
        }

        #endregion Initializers
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PlayerTargetTrackingState = this.GetComponent<IPlayerState>().PlayerTargetTrackingState;

            IPlayerState _PlayerState = this.GetComponent<IPlayerState>();
            this.m_PlayerAttackState = _PlayerState.PlayerAttackState;
            this.m_PlayerMovementState = _PlayerState.PlayerMovementState;

            // Initialize Movement States
            this.m_NormalMovement = new PlayerNormalMovement(
                this.GetComponent<IPlayerAttackHandler>(),
                this.m_PlayerAttackState,
                this.m_CameraController, 
                this.m_PlayerMovementState,
                this.m_Animator, 
                this.transform,
                this);
            this.m_LockOnMovement = new PlayerLockOnMovement(
                this.GetComponent<IPlayerAttackHandler>(),
                this.m_PlayerAttackState,
                this.m_Animator, 
                this.transform,
                this.m_PlayerMovementState,
                this.m_PlayerTargetTrackingState,
                this);
            this.m_FinisherMovement = new PlayerFinishMovement(
                this.m_PlayerMovementState,
                this.m_Animator,
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
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -
        
        // --------------------------------
        // Dodge
        // --------------------------------
        
        public void Dodge()
            => this.m_CurrentMovementState.PerformDodge();

        void IPlayerDodgeMovement.EnableDodge()
            => this.m_PlayerMovementState.CanDodge = true;

        void IPlayerDodgeMovement.DisableDodge()
            => this.m_PlayerMovementState.CanDodge = false;
        
        // --------------------------------
        // Movement
        // --------------------------------

        void IPlayerMovement.DisableMovement()
            => this.m_IsMovementEnabled = false;

        void IPlayerMovement.EnableMovement()
            => this.m_IsMovementEnabled = true;

        void IPlayerMovement.PreparePlayerMovement(Vector2 moveDirection)
        {
            // Ticket: #34 - Behaviour pertaining to animation will be moved to separate player animator component.
            if (moveDirection == Vector2.zero)
                this.m_CurrentMovementState.PerformSprint(false);

            this.m_CurrentMovementState.SetInputDirection(moveDirection);
        }

        void IPlayerMovement.PrepareSprint(bool isSprinting)
            => this.m_CurrentMovementState.PerformSprint(isSprinting);
        
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
