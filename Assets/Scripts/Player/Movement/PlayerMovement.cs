using Player.Animation;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    public class PlayerMovement : TOSGMonoBehaviourBase, IPlayerMovement
    {
        
        #region - - - - - - Fields - - - - - -
        
        private PlayerAnimationComponent m_PlayerAnimationComponent;
        private ICameraController m_CameraController;
        private IControlledCameraState m_CameraState;
        private FinishingMoveController m_FinishingMoveController;
        
        // Player states
        private PlayerAttackState m_PlayerAttackState;
        private PlayerMovementState m_PlayerMovementState;
        private PlayerTargetTrackingState m_PlayerTargetTrackingState;
        
        private float m_CurrentAngleSmoothVelocity;
        private bool m_IsMovementEnabled = true;
        private bool m_IsRotationEnabled = true;
        private bool m_IsSprinting = false;
        private Vector2 m_InputDirection = Vector2.zero;
        private float m_MovementSpeed = 0.5f;
        private float m_SprintMultiplier = 2.0f;
        private float m_RotationSpeed = 4f;
        private float m_MovementSmoothingDampingTime = .1f;

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
        }

        private void Update()
        {
            if (this.IsPaused || !this.m_IsMovementEnabled || this.m_FinishingMoveController.bIsFinishing) 
                return;

            // Move the player
            this.CalculateMovementDirection();
            this.RotatePlayerToMovementDirection();
            this.PerformMovement();
            
            // Perform specific movement behavior
            this.LockPlayerRotationToAttackTarget();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -

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

            if (this.m_CameraState.IsCameraViewTargetLocked) 
                return;
            
            this.m_CameraController.ToggleSprintCameraState(this.m_IsSprinting);
            
            // Toggle sprinting animation
            if (this.m_InputDirection == Vector2.zero || !this.m_IsSprinting)
                m_PlayerAnimationComponent.SetSprinting(false);
            else
                m_PlayerAnimationComponent.SetSprinting(true);
        }

        private Vector3 CalculateMovementDirection()
        {
            this.m_PlayerMovementState.MoveDirection = new Vector3(this.m_InputDirection.x, 0, this.m_InputDirection.y).normalized;
            return this.m_PlayerMovementState.MoveDirection;
        }        

        private void LockPlayerRotationToAttackTarget()
        {
            if (!this.m_CameraState.IsCameraViewTargetLocked)
                return;
            
            Vector3 _NewLookDirection = this.m_PlayerTargetTrackingState.AttackTarget.transform.position - this.transform.position;
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
