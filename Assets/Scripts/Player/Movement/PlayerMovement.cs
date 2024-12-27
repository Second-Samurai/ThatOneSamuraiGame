using Player.Animation;
using ThatOneSamuraiGame.Legacy;
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
        public CameraController CameraController;
        
        private FinishingMoveController m_FinishingMoveController;
        
        private PlayerAnimationComponent m_PlayerAnimationComponent;
        private ICameraController m_CameraController;
        private IControlledCameraState m_CameraState;
        private FinishingMoveController m_FinishingMoveController;
        
        // Player data containers
        private PlayerAttackState m_PlayerAttackState;
        private PlayerMovementState m_PlayerMovementState;
        private PlayerTargetTrackingState m_PlayerTargetTrackingState;
        
        // Player states
        private IPlayerMovementState m_CurrentMovementState;
        private PlayerNormalMovement m_NormalMovement;
        private PlayerLockOnMovement m_LockOnMovement;
        
        private float m_CurrentAngleSmoothVelocity;
        private Vector2 m_InputDirection = Vector2.zero;
        private bool m_IsMovementEnabled = true;
        private bool m_IsRotationEnabled = true;
        private bool m_IsSprinting = false;
        private float m_MovementSpeed = 0.5f;
        private float m_SprintMultiplier = 2.0f;
        private float m_SprintDuration = 0.0f;
        private float m_RotationSpeed = 4f;
        
        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PlayerAnimationComponent = this.GetComponent<PlayerAnimationComponent>();
            this.m_CameraController = this.GetComponent<ICameraController>();
            this.m_CameraState = this.GetComponent<IControlledCameraState>();
            this.m_FinishingMoveController = this.GetComponentInChildren<FinishingMoveController>();
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
            
            this.m_CurrentMovementState = this.m_NormalMovement;
            
            ((IPlayerDodgeMovement)this).EnableDodge();
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

            if (IsSprinting())
                TickSprintDuration();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -
        
        public void Dodge()
            => this.m_CurrentMovementState.PerformDodge();

        void IPlayerDodgeMovement.EnableDodge()
            => this.m_PlayerMovementState.CanDodge = true;

        void IPlayerDodgeMovement.DisableDodge()
            => this.m_PlayerMovementState.CanDodge = false;

        public bool IsSprinting() => m_IsSprinting;

        public float GetSprintDuration() => m_SprintDuration;

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
            
            if (moveDirection == Vector2.zero)
                m_PlayerAnimationComponent.SetSprinting(false);
            
            this.m_InputDirection = moveDirection;
        }

        void IPlayerMovement.PrepareSprint(bool isSprinting)
        {
            this.m_IsSprinting = isSprinting;

            // Toggle sprinting animation
            if (this.m_InputDirection == Vector2.zero || !this.m_IsSprinting)
            {
                m_SprintDuration = 0.0f;
                m_PlayerAnimationComponent.SetSprinting(false);
            }
            else
            {
                m_SprintDuration = 0.0f;
                m_PlayerAnimationComponent.SetSprinting(true);
            }
        }

        private void TickSprintDuration()
        {
            m_SprintDuration += Time.deltaTime;
        }
        
        void IPlayerMovement.SetState(PlayerMovementStates state)
        {
            if (state == PlayerMovementStates.Normal)
                this.m_CurrentMovementState = this.m_NormalMovement;
            else if (state == PlayerMovementStates.LockOn)
                this.m_CurrentMovementState = this.m_LockOnMovement;
            
            Debug.Log("Movement State is: " + state);
        }

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

        private void RotatePlayerToMovementDirection()
        {
            if (this.m_InputDirection == Vector2.zero
                 || this.m_CameraState.IsCameraViewTargetLocked
                 || !this.m_IsRotationEnabled
                 || this.m_PlayerAttackState.IsWeaponSheathed)
                return;

            float _TargetAngle = Mathf.Atan2(this.m_PlayerMovementState.MoveDirection.x, this.m_PlayerMovementState.MoveDirection.z)
                                 * Mathf.Rad2Deg + this.m_CameraState.CurrentEulerAngles.y;
            float _NextAngleRotation = Mathf.SmoothDampAngle(
                this.transform.eulerAngles.y,
                _TargetAngle,
                ref this.m_CurrentAngleSmoothVelocity,
                this.m_MovementSmoothingDampingTime);

            this.transform.rotation = Quaternion.Euler(0f, _NextAngleRotation, 0f);
        }
        
        private float sprintModifier => m_IsSprinting == true ? m_SprintMultiplier : 1f;
        private void PerformMovement()
        {
            if (this.m_PlayerMovementState.IsMovementLocked)
                return;
            
            // Invokes player movement through the physically based animation movements (Root Motion)
            m_PlayerAnimationComponent.SetInputDirection(m_InputDirection, m_MovementSmoothingDampingTime);
            m_PlayerAnimationComponent.SetInputSpeed(m_InputDirection.magnitude * m_MovementSpeed * sprintModifier, m_MovementSmoothingDampingTime);
        }

        #endregion Methods
        
    }
    
}
