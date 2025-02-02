using System;
using Player.Animation;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

public class PlayerLockOnMovement : BasePlayerMovementState
{

    #region - - - - - - Fields - - - - - -

    // Required Components
    private readonly IPlayerAttackHandler m_AttackHandler;
    private readonly PlayerAttackState m_AttackState;
    private readonly PlayerTargetTrackingState m_TargetTrackingState;
    private readonly MonoBehaviour m_RootReferenceMonoBehaviour; // Required for Coroutine
    
    // Runtime Fields
    private readonly float m_DodgeForce = 10f;
    private readonly float m_RotationSpeed = 4f;
    
    private Quaternion m_CurrentRotation;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public PlayerLockOnMovement(
        IPlayerAttackHandler attackHandler,
        PlayerAttackState attackState,
        PlayerAnimationComponent playerAnimationComponent, 
        Transform playerTransform, 
        PlayerMovementDataContainer movementDataContainer, 
        PlayerTargetTrackingState targetTrackingState,
        MonoBehaviour refMonoBehaviour)
        : base(playerAnimationComponent, playerTransform, movementDataContainer)
    {
        this.m_AttackHandler = attackHandler ?? throw new ArgumentNullException(nameof(attackHandler));
        this.m_AttackState = attackState ?? throw new ArgumentNullException(nameof(attackState));
        this.m_RootReferenceMonoBehaviour =
            refMonoBehaviour ?? throw new ArgumentNullException(nameof(refMonoBehaviour));
        this.m_TargetTrackingState =
            targetTrackingState ?? throw new ArgumentNullException(nameof(targetTrackingState));
    }

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    public override void CalculateMovement()
    {
        this.MMovementDataContainer.MoveDirection = 
            this.m_TargetTrackingState.AttackTarget.position - this.m_PlayerTransform.position;
        this.m_CurrentRotation = Quaternion.Slerp(
            this.m_PlayerTransform.rotation,
            Quaternion.LookRotation(this.MMovementDataContainer.MoveDirection),
            this.m_RotationSpeed);
    }

    public override void ApplyMovement()
    {
        base.ApplyMovement();
        
        // Apply rotation
        this.m_PlayerTransform.rotation = Quaternion.Euler(0, this.m_CurrentRotation.eulerAngles.y, 0);
    }
    
    public override PlayerMovementStates GetState()
        => PlayerMovementStates.LockOn;

    public override void PerformDodge()
    {
        if (!this.MMovementDataContainer.CanDodge) return;
        
        this.m_PlayerAnimationComponent.TriggerDodge();
        this.m_PlayerAnimationComponent.ResetLightAttack();
    
        if (this.m_AttackState.ParryStunned)
            this.m_AttackHandler.EndParryAction();
        
        this.m_RootReferenceMonoBehaviour.StartCoroutine(
            this.ApplyDodgeTranslation(
                new Vector3(this.MMovementDataContainer.MoveDirection.x, 0, this.MMovementDataContainer.MoveDirection.y),
                this.m_DodgeForce));
        this.m_AttackHandler.ResetAttack();
    }

    public override void SetInputDirection(Vector2 inputDirection)
        => this.m_InputDirection = inputDirection;

    #endregion Methods

}
