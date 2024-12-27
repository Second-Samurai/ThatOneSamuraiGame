using System;
using Player.Animation;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

public class PlayerNormalMovement : BasePlayerMovementState
{

    #region - - - - - - Fields - - - - - -

    private readonly IPlayerAttackHandler m_AttackHandler;
    private readonly PlayerAttackState m_AttackState;
    private readonly CameraController m_CameraController;
    private readonly MonoBehaviour m_RootReferenceMonoBehaviour; // Required for Coroutine
    
    private float m_CurrentAngleSmoothVelocity;
    private float m_CurrentAngleRotation;
    private float m_TargetAngleRotation;
    private float m_DodgeForce = 10f;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public PlayerNormalMovement(
        IPlayerAttackHandler attackHandler,
        PlayerAttackState attackState,
        CameraController cameraController,
        PlayerMovementState movementState,
        PlayerAnimationComponent playerAnimationComponent,
        Transform playerTransform,
        MonoBehaviour refMonoBehaviour)
        : base(playerAnimationComponent, playerTransform, movementState)
    {
        this.m_AttackHandler = attackHandler ?? throw new ArgumentNullException(nameof(attackHandler));
        this.m_AttackState = attackState ?? throw new ArgumentNullException(nameof(attackState));
        this.m_CameraController = cameraController ?? throw new ArgumentNullException(nameof(cameraController));
        this.m_RootReferenceMonoBehaviour =
            refMonoBehaviour ?? throw new ArgumentNullException(nameof(refMonoBehaviour));
    }

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    public override void ApplyMovement()
    {
        base.ApplyMovement();

        // Apply rotation
        this.m_PlayerTransform.rotation = Quaternion.Euler(0f, this.m_CurrentAngleRotation, 0f);
    }
    
    public override void CalculateMovement()
    {
        // Calculate Move Direction
        this.m_MovementState.MoveDirection = new Vector3(this.m_InputDirection.x, 0, this.m_InputDirection.y).normalized;
        
        // Calculate Rotation
        this.CalculatePlayerRotation();
    }

    public override void PerformDodge()
    {
        if (!this.m_MovementState.CanDodge) return;
        
        this.m_PlayerAnimationComponent.SetTrigger("Dodge");
        this.m_PlayerAnimationComponent.ResetTrigger("AttackLight");
        
        if (this.m_AttackState.ParryStunned)
            this.m_AttackHandler.EndParryAction();

        this.m_RootReferenceMonoBehaviour.StartCoroutine(
            this.ApplyDodgeTranslation(Vector3.forward, this.m_DodgeForce));
        this.m_AttackHandler.ResetAttack();
    }

    public override void SetInputDirection(Vector2 inputDirection) 
        => this.m_InputDirection = inputDirection;

    private void CalculatePlayerRotation()
    {
        this.m_TargetAngleRotation = Mathf.Atan2(this.m_MovementState.MoveDirection.x, this.m_MovementState.MoveDirection.z)
            * Mathf.Rad2Deg + this.m_CameraController.GetCameraEulerAngles().y;
        
        this.m_CurrentAngleRotation = Mathf.SmoothDampAngle(
            this.m_PlayerTransform.eulerAngles.y,
            this.m_TargetAngleRotation,
            ref this.m_CurrentAngleSmoothVelocity,
            MOVEMENT_SMOOTHING_DAMPING_TIME);
    }
    
    #endregion Methods
  
}
