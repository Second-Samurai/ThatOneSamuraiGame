using System.Security.Cryptography.X509Certificates;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.UI.Pause;
using UnityEngine;
using UnityTemplateProjects;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    public class PlayerMovement : ActionMonoBehaviour, IPlayerMovement
    {
        
        #region - - - - - - Fields - - - - - -

        private Animator m_Animator;
        private IControlledCameraState m_CameraState;
        private FinishingMoveController m_FinishingMoveController; // This needs to be decoupled. Use interfaces
        private IPlayerAttackState m_PlayerAttackState;

        private bool m_IsMovementEnabled = true;
        private bool m_IsRotationEnabled = true;
        private Vector2 m_MoveDirection = Vector2.zero;
        private float m_CurrentAngleSmoothVelocity;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        Vector2 IPlayerMovement.MoveDirection
            => this.m_MoveDirection;


        #endregion Properties
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_CameraState = this.GetComponent<IControlledCameraState>();
            this.m_FinishingMoveController = this.GetComponentInChildren<FinishingMoveController>();
            this.m_PlayerAttackState = this.GetComponent<IPlayerAttackState>();
        }

        private void Update()
        {
            if (this.IsPaused) return;
            
            this.MovePlayer();
            this.SprintPlayer();
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
            if (!this.m_FinishingMoveController.bIsFinishing)
                return;
            
            if (moveDirection == Vector2.zero)
                this.m_Animator.SetBool("IsSprinting", false);

            this.m_MoveDirection = moveDirection;
        }

        void IPlayerMovement.Sprint(bool isSprinting)
        {
            
        }

        private void MovePlayer()
        {
            if (!this.m_IsMovementEnabled && this.m_FinishingMoveController.bIsFinishing) 
                return;

            
            // Rotate the player to the target direction
            Vector3 _Direction = new Vector3(this.m_MoveDirection.x, 0, this.m_MoveDirection.y).normalized;
            if (_Direction != Vector3.zero
                && !this.m_CameraState.IsCameraViewTargetLocked
                && this.m_IsRotationEnabled
                && !this.m_PlayerAttackState.IsWeaponSheathed)
            {
                float _TargetAngle = Mathf.Atan2(_Direction.x, _Direction.z) * Mathf.Rad2Deg +
                                     this.m_CameraState.CurrentEulerAngles.y;
                float _NextAngleRotation = Mathf.SmoothDampAngle(
                                            this.m_CameraState.CurrentEulerAngles.y,
                                            _TargetAngle,
                                            ref this.m_CurrentAngleSmoothVelocity,
                                            .1f);

                this.transform.rotation = Quaternion.Euler(0f, _NextAngleRotation, 0f);
            }
        }

        private void SprintPlayer()
        {
            
        }

        #endregion Methods
        
    }
    
}
