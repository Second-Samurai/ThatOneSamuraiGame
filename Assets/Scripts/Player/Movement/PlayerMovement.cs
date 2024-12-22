using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    public class PlayerMovement : PausableMonoBehaviour, IPlayerMovement
    {
        
        #region - - - - - - Fields - - - - - -

        // Make these injected
        public CameraController CameraController;

        private Animator m_Animator;
        // private Legacy.ICameraController m_CameraController;
        // private IControlledCameraState m_CameraState;
        private FinishingMoveController m_FinishingMoveController;
        
        // Player data containers
        private PlayerAttackState m_PlayerAttackState;
        private PlayerMovementState m_PlayerMovementState;
        private PlayerTargetTrackingState m_PlayerTargetTrackingState;
        
        // Player states
        private PlayerNormalMovement m_NormalMovement;

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
            // this.m_CameraController = this.GetComponent<Legacy.ICameraController>();
            // this.m_CameraState = this.GetComponent<IControlledCameraState>();
            this.m_FinishingMoveController = this.GetComponentInChildren<FinishingMoveController>();
            this.m_PlayerTargetTrackingState = this.GetComponent<IPlayerState>().PlayerTargetTrackingState;

            IPlayerState _PlayerState = this.GetComponent<IPlayerState>();
            this.m_PlayerAttackState = _PlayerState.PlayerAttackState;
            this.m_PlayerMovementState = _PlayerState.PlayerMovementState;

            this.m_NormalMovement = new PlayerNormalMovement(
                this.CameraController, 
                this.m_PlayerMovementState,
                this.m_Animator, 
                this.transform);
            
            this.m_CurrentMovementState = this.m_NormalMovement;
        }

        private void Update()
        {
            if (this.IsPaused || !this.m_IsMovementEnabled || this.m_FinishingMoveController.bIsFinishing) 
                return;

            // Move the player
            // this.CalculateMovementDirection();
            // this.RotatePlayerToMovementDirection();
            // this.PerformMovement();
            
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
        }

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

            // if (this.m_CameraState.IsCameraViewTargetLocked) 
            //     return;
            //
            // this.m_CameraController.ToggleSprintCameraState(this.m_IsSprinting);
            
            // Toggle sprinting animation
            // Ticket: #34 - Behaviour pertaining to animation will be moved to separate player animator component.
            if (this.m_InputDirection == Vector2.zero || !this.m_IsSprinting)
                this.m_Animator.SetBool("IsSprinting", false);
            else
                this.m_Animator.SetBool("IsSprinting", true);
        }

        // private Vector3 CalculateMovementDirection()
        // {
        //     this.m_PlayerMovementState.MoveDirection = new Vector3(this.m_InputDirection.x, 0, this.m_InputDirection.y).normalized;
        //     return this.m_PlayerMovementState.MoveDirection;
        // }        

        private void LockPlayerRotationToAttackTarget()
        {
            // if (!this.m_CameraState.IsCameraViewTargetLocked)
            //     return;

            if (this.m_PlayerTargetTrackingState.AttackTarget == null) return;
            
            Vector3 _NewLookDirection = this.m_PlayerTargetTrackingState.AttackTarget.position - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(
                this.transform.rotation,
                Quaternion.LookRotation(_NewLookDirection),
                this.m_RotationSpeed);
            
            this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);
        }

        // private void RotatePlayerToMovementDirection()
        // {
        //     if (this.m_InputDirection == Vector2.zero
        //          // || this.CameraController.IsCameraViewTargetLocked // TODO: This should come from a target manager or controller. Not from the camera
        //          || !this.m_IsRotationEnabled
        //          || this.m_PlayerAttackState.IsWeaponSheathed)
        //         return;

            // float _TargetAngle = Mathf.Atan2(this.m_PlayerMovementState.MoveDirection.x, this.m_PlayerMovementState.MoveDirection.z)
            //                      * Mathf.Rad2Deg + this.CameraController.GetCameraEulerAngles().y; // TODO: get from the camera controller
            // float _NextAngleRotation = Mathf.SmoothDampAngle(
            //     this.transform.eulerAngles.y,
            //     _TargetAngle,
            //     ref this.m_CurrentAngleSmoothVelocity,
            //     this.m_MovementSmoothingDampingTime);
            //
            // this.transform.rotation = Quaternion.Euler(0f, _NextAngleRotation, 0f);
        // }

        // Ticket: #34 - Behaviour pertaining to animation will be moved to separate player animator component.
        // private void PerformMovement()
        // {
        //     if (this.m_PlayerMovementState.IsMovementLocked)
        //         return;
        //
        //     // Invokes player movement through the physically based animation movements
        //     this.m_Animator.SetFloat(
        //         "XInput", 
        //         this.m_InputDirection.x, 
        //         this.m_MovementSmoothingDampingTime, 
        //         Time.deltaTime);
        //     
        //     this.m_Animator.SetFloat(
        //         "YInput",
        //         this.m_InputDirection.y,
        //         this.m_MovementSmoothingDampingTime,
        //         Time.deltaTime);
        //     
        //     this.m_Animator.SetFloat(
        //         "InputSpeed",
        //         this.m_InputDirection.magnitude,
        //         this.m_MovementSmoothingDampingTime,
        //         Time.deltaTime);
        // }

        #endregion Methods
        
    }
    
}
