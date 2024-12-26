using System;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

public class PlayerLockOnMovement : BasePlayerMovementState
{

    #region - - - - - - Fields - - - - - -

    private readonly IPlayerAttackHandler m_AttackHandler;
    private readonly PlayerAttackState m_AttackState;
    private readonly MonoBehaviour m_RootReferenceMonoBehaviour; // Required for Coroutine
    
    private float m_CurrentAngleSmoothVelocity;
    private Quaternion m_CurrentRotation;
    private float m_DodgeForce = 10f;
    private float m_RotationSpeed = 4f;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public PlayerLockOnMovement(
        IPlayerAttackHandler attackHandler,
        PlayerAttackState attackState,
        Animator playerAnimator, 
        Transform playerTransform, 
        PlayerMovementState movementState, 
        MonoBehaviour refMonoBehaviour) 
        : base(playerAnimator, playerTransform, movementState)
    {
        this.m_AttackHandler = attackHandler ?? throw new ArgumentNullException(nameof(attackHandler));
        this.m_AttackState = attackState ?? throw new ArgumentNullException(nameof(attackState));
        this.m_RootReferenceMonoBehaviour =
            refMonoBehaviour ?? throw new ArgumentNullException(nameof(refMonoBehaviour));
    }

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    public override void CalculateMovement()
    {
        this.m_MovementState.MoveDirection = 
            new Vector3(this.m_InputDirection.x, 0, this.m_InputDirection.y).normalized;
        this.m_CurrentRotation = Quaternion.Slerp(
            this.m_PlayerTransform.rotation,
            Quaternion.LookRotation(this.m_MovementState.MoveDirection),
            this.m_RotationSpeed);
    }

    public override void ApplyMovement()
    {
        base.ApplyMovement();
    
        // Apply rotation
        this.m_PlayerTransform.rotation = Quaternion.Euler(0, this.m_CurrentRotation.y, 0);
    }

    public override void PerformDodge()
    {
        if (!this.m_MovementState.CanDodge) return;
        
        this.m_PlayerAnimator.SetTrigger("Dodge");
        this.m_PlayerAnimator.ResetTrigger("AttackLight");
    
        if (this.m_AttackState.HasBeenParried)
            this.m_AttackHandler.EndParryAction();
        
        this.m_RootReferenceMonoBehaviour.StartCoroutine(
            this.ApplyDodgeTranslation(
                new Vector3(this.m_MovementState.MoveDirection.x, 0, this.m_MovementState.MoveDirection.y),
                this.m_DodgeForce));
        this.m_AttackHandler.ResetAttack();
    }

    public override void SetInputDirection(Vector2 inputDirection)
        => this.m_InputDirection = inputDirection;

    #endregion Methods

}
