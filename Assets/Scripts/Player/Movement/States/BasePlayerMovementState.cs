using System;
using System.Collections;
using Player.Animation;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

public class BasePlayerMovementState : IPlayerMovementState
{

    #region - - - - - - Fields - - - - - -

    protected const float MOVEMENT_SMOOTHING_DAMPING_TIME = .1f;
    protected const float DODGE_TIME_LIMIT = .15f;
    
    protected readonly float m_DeltaTime;
    protected Vector2 m_InputDirection;
    protected readonly PlayerMovementState m_MovementState;
    protected readonly PlayerAnimationComponent m_PlayerAnimationComponent;
    protected readonly Transform m_PlayerTransform;

    // private bool m_IsSprinting;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public BasePlayerMovementState(
        PlayerAnimationComponent playerAnimationComponent, 
        Transform playerTransform, 
        PlayerMovementState movementState)
    {
        this.m_PlayerAnimationComponent = playerAnimationComponent ?? throw new ArgumentNullException(nameof(playerAnimationComponent));
        this.m_PlayerTransform = playerTransform ?? throw new ArgumentNullException(nameof(playerTransform));
        this.m_MovementState = movementState ?? throw new ArgumentNullException(nameof(movementState));
        
        this.m_DeltaTime = Time.deltaTime;
    }

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    public virtual void CalculateMovement() {}

    public virtual void ApplyMovement()
    {
        // Invokes player movement through the physically based animation movements
        this.m_PlayerAnimationComponent.SetInputDirection(
            new Vector2(this.m_InputDirection.x, this.m_InputDirection.y),
            MOVEMENT_SMOOTHING_DAMPING_TIME);
        
        this.m_PlayerAnimationComponent.SetInputSpeed(
            this.m_InputDirection.magnitude,
            MOVEMENT_SMOOTHING_DAMPING_TIME);
    }

    public virtual void PerformDodge() { }

    public virtual void SetInputDirection(Vector2 inputDirection) { }
    
    public virtual PlayerMovementStates GetState()
        => PlayerMovementStates.Normal; // Returns normal movement by default. Just to satisfy return value.

    void IPlayerMovementState.PerformSprint(bool isSprinting)
    {
        if (this.m_InputDirection == Vector2.zero || !isSprinting)
            this.m_PlayerAnimator.SetBool("IsSprinting", false);
        else
            this.m_PlayerAnimator.SetBool("IsSprinting", true);
    }
    
    protected IEnumerator ApplyDodgeTranslation(Vector3 lastDirection, float force)
    {
        this.m_MovementState.CanDodge = false;
        
        float _DodgeTimer = DODGE_TIME_LIMIT;
        while (_DodgeTimer > 0f)
        {
            this.m_PlayerTransform.Translate(lastDirection.normalized * (force * this.m_DeltaTime)); 
            _DodgeTimer -= this.m_DeltaTime;
        
            yield return null;
        }

        this.m_MovementState.CanDodge = true;
    }

    #endregion Methods
  
}
