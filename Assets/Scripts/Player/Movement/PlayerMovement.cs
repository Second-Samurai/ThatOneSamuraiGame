using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{

    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : PausableMonoBehaviour, IPlayerMovement, IPlayerDodgeMovement
    {
        
        #region - - - - - - Fields - - - - - -
        
        // Make these injected
        [RequiredField]
        [SerializeField]
        private CameraController CameraController;
        [RequiredField]
        [SerializeField]
        private LockOnSystem LockOnSystem;
        [RequiredField]
        [SerializeField]
        private Animator m_Animator;
        // private FinishingMoveController m_FinishingMoveController;
        
        // Player data containers
        private PlayerAttackState m_PlayerAttackState;
        private PlayerMovementState m_PlayerMovementState;
        private PlayerTargetTrackingState m_PlayerTargetTrackingState;
        
        // Player states
        private IPlayerMovementState m_CurrentMovementState;
        private IPlayerMovementState m_NormalMovement;
        private IPlayerMovementState m_LockOnMovement;
        private IPlayerMovementState m_FinisherMovement;
        
        // private float m_CurrentAngleSmoothVelocity;
        private bool m_IsMovementEnabled = true;
        private bool m_IsRotationEnabled = true;
        // private float m_RotationSpeed = 4f;

        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            // this.m_Animator = this.GetComponent<Animator>();
            // this.m_FinishingMoveController = this.GetComponentInChildren<FinishingMoveController>();
            this.m_PlayerTargetTrackingState = this.GetComponent<IPlayerState>().PlayerTargetTrackingState;

            IPlayerState _PlayerState = this.GetComponent<IPlayerState>();
            this.m_PlayerAttackState = _PlayerState.PlayerAttackState;
            this.m_PlayerMovementState = _PlayerState.PlayerMovementState;

            // Initialize Movement States
            this.m_NormalMovement = new PlayerNormalMovement(
                this.GetComponent<IPlayerAttackHandler>(),
                this.m_PlayerAttackState,
                this.CameraController, 
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
                this);
            // ILockOnSystem _LockOnSystem = this.GetComponentInChildren<ILockOnSystem>();
            // this.m_FinisherMovement = new PlayerFinishMovement(
            //     _LockOnSystem,
            //     this.m_PlayerMovementState,
            //     this.m_Animator,
            //     this.transform,
            //     this);
            
            this.m_CurrentMovementState = this.m_NormalMovement;
            
            ((IPlayerDodgeMovement)this).EnableDodge();
        }

        private void Update()
        {
            if (this.IsPaused 
                || !this.m_IsMovementEnabled 
                || this.m_CurrentMovementState.GetState() == PlayerMovementStates.Finisher) // This needs to be removed
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
