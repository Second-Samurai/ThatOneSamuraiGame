using System;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{

    public class PlayerLockOnMovement : IPlayerMovementState
    {
        private const float MOVEMENT_SMOOTHING_DAMPING_TIME = .1f;
        private const float DODGE_TIME_LIMIT = .15f;
        
        private Vector2 m_InputDirection;
        private readonly Animator m_PlayerAnimator;
        private readonly Transform m_PlayerTransform;
        private PlayerMovementState m_PlayerMovementState;
        private PlayerTargetTrackingState m_PlayerTargetTrackingState;
        
        private float m_CurrentAngleSmoothVelocity;
        private Quaternion m_CurrentRotation;
        private float m_DeltaTime;
        private float m_DodgeForce = 10f;
        private float m_RotationSpeed = 4f;

        public PlayerLockOnMovement(
            Animator playerAnimator, 
            Transform playerTransform, 
            PlayerMovementState movementState, 
            PlayerTargetTrackingState targetTrackingState)
        {
            this.m_PlayerAnimator = playerAnimator ?? throw new ArgumentNullException(nameof(playerAnimator));
            this.m_PlayerTransform = playerTransform ?? throw new ArgumentNullException(nameof(playerTransform));
            this.m_PlayerMovementState = movementState ?? throw new ArgumentNullException(nameof(movementState));
            this.m_PlayerTargetTrackingState =
                targetTrackingState ?? throw new ArgumentNullException(nameof(targetTrackingState));
        }
        
        public void CalculateMovement()
        {
            this.m_CurrentRotation = Quaternion.Slerp(
                this.m_PlayerTransform.rotation,
                Quaternion.LookRotation(
                    this.m_PlayerTargetTrackingState.AttackTarget.position -
                    this.m_PlayerTransform.position),
                this.m_RotationSpeed);
        }

        public void ApplyMovement()
        {
            // Invokes player movement through the physically based animation movements
            this.m_PlayerAnimator.SetFloat(
                "XInput", 
                this.m_InputDirection.x, 
                MOVEMENT_SMOOTHING_DAMPING_TIME,
                this.m_DeltaTime);
            
            this.m_PlayerAnimator.SetFloat(
                "YInput",
                this.m_InputDirection.y,
                MOVEMENT_SMOOTHING_DAMPING_TIME,
                this.m_DeltaTime);
            
            this.m_PlayerAnimator.SetFloat(
                "InputSpeed",
                this.m_InputDirection.magnitude,
                MOVEMENT_SMOOTHING_DAMPING_TIME,
                this.m_DeltaTime);
        
            // Apply rotation
            this.m_PlayerTransform.rotation = Quaternion.Euler(0, this.m_CurrentRotation.y, 0);
        }

        public void PerformDodge()
        {
            throw new System.NotImplementedException();
        }

        public void SetInputDirection(Vector2 inputDirection)
            => this.m_InputDirection = inputDirection;
        
    }

}