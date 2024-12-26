using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{

    public interface IPlayerDodgeMovement
    {

        #region - - - - - - Methods - - - - - -

        void EnableDodge();

        void DisableDodge();

        #endregion Methods

    }
    
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : PausableMonoBehaviour, IPlayerMovement, IPlayerDodgeMovement
    {
        
        #region - - - - - - Fields - - - - - -
        
        // Make these injected
        public CameraController CameraController;

        private Animator m_Animator;
        private FinishingMoveController m_FinishingMoveController;
        
        // Player data containers
        private PlayerAttackState m_PlayerAttackState;
        private PlayerMovementState m_PlayerMovementState;
        private PlayerTargetTrackingState m_PlayerTargetTrackingState;
        
        // Player states
        private PlayerNormalMovement m_NormalMovement;
        private PlayerLockOnMovement m_LockOnMovement;

        private IPlayerMovementState m_CurrentMovementState;
        
        private float m_CurrentAngleSmoothVelocity;
        private bool m_IsMovementEnabled = true;
        private bool m_IsRotationEnabled = true;
        private bool m_IsSprinting = false;
        private Vector2 m_InputDirection = Vector2.zero;
        private float m_RotationSpeed = 4f;
        private float m_MovementSmoothingDampingTime = .1f;

        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_Animator = this.GetComponent<Animator>();
            this.m_FinishingMoveController = this.GetComponentInChildren<FinishingMoveController>();
            this.m_PlayerTargetTrackingState = this.GetComponent<IPlayerState>().PlayerTargetTrackingState;

            IPlayerState _PlayerState = this.GetComponent<IPlayerState>();
            this.m_PlayerAttackState = _PlayerState.PlayerAttackState;
            this.m_PlayerMovementState = _PlayerState.PlayerMovementState;

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
                this.m_PlayerTargetTrackingState,
                this);
            
            this.m_CurrentMovementState = this.m_NormalMovement;
            
            this.EnableDodge();
        }

        private void Update()
        {
            if (this.IsPaused || !this.m_IsMovementEnabled || this.m_FinishingMoveController.bIsFinishing) 
                return;

            this.m_CurrentMovementState.SetInputDirection(this.m_InputDirection);
            this.m_CurrentMovementState.CalculateMovement();
            this.m_CurrentMovementState.ApplyMovement();
            
            // Perform specific movement behavior
            this.LockPlayerRotationToAttackTarget();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -
        
        // TODO: Convert to interface
        public void SetState(PlayerMovementStates state)
        {
            if (state == PlayerMovementStates.Normal)
                this.m_CurrentMovementState = this.m_NormalMovement;
            else if (state == PlayerMovementStates.LockOn)
                this.m_CurrentMovementState = this.m_LockOnMovement;
            
            Debug.Log("Movement State is: " + state);
        }

        public void EnableDodge()
            => this.m_PlayerMovementState.CanDodge = true;

        void IPlayerDodgeMovement.DisableDodge()
            => this.m_PlayerMovementState.CanDodge = false;

        void IPlayerMovement.DisableMovement()
            => this.m_IsMovementEnabled = false;

        void IPlayerMovement.DisableRotation()
            => this.m_IsRotationEnabled = false;

        void IPlayerMovement.EnableMovement()
            => this.m_IsMovementEnabled = true;

        void IPlayerMovement.EnableRotation()
            => this.m_IsRotationEnabled = true;

        void IPlayerMovement.PreparePlayerMovement(Vector2 moveDirection)
        {
            if (this.m_FinishingMoveController.bIsFinishing)
                return;
            
            // Ticket: #34 - Behaviour pertaining to animation will be moved to separate player animator component.
            if (moveDirection == Vector2.zero)
                this.m_Animator.SetBool("IsSprinting", false);

            this.m_InputDirection = moveDirection;
        }

        void IPlayerMovement.PrepareSprint(bool isSprinting)
        {
            this.m_IsSprinting = isSprinting;

            // Toggle sprinting animation
            // Ticket: #34 - Behaviour pertaining to animation will be moved to separate player animator component.
            if (this.m_InputDirection == Vector2.zero || !this.m_IsSprinting)
                this.m_Animator.SetBool("IsSprinting", false);
            else
                this.m_Animator.SetBool("IsSprinting", true);
        }

        public void Dodge()
            => this.m_CurrentMovementState.PerformDodge();

        private void LockPlayerRotationToAttackTarget()
        {
            if (this.m_PlayerTargetTrackingState.AttackTarget == null) return;
            
            Vector3 _NewLookDirection = this.m_PlayerTargetTrackingState.AttackTarget.position - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(
                this.transform.rotation,
                Quaternion.LookRotation(_NewLookDirection),
                this.m_RotationSpeed);
            
            this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
        }

        #endregion Methods
        
    }
    
}
