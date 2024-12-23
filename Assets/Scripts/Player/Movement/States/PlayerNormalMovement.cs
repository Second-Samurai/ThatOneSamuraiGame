using System;
using System.Collections;
using System.Diagnostics;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

public interface IPlayerMovementState
{

    #region - - - - - - Methods - - - - - -

    void CalculateMovement();

    void ApplyMovement();

    void PerformDodge();

    void SetInputDirection(Vector2 inputDirection);

    #endregion Methods
  
}

public class PlayerNormalMovement : IPlayerMovementState
{

    #region - - - - - - Fields - - - - - -

    private const float MOVEMENT_SMOOTHING_DAMPING_TIME = .1f;
    private const float DODGE_TIME_LIMIT = .15f;
    
    private readonly IPlayerAttackHandler m_AttackHandler;
    private readonly PlayerAttackState m_AttackState;
    private readonly CameraController m_CameraController;
    private readonly PlayerMovementState m_MovementState;
    private readonly Animator m_PlayerAnimator;
    private readonly Transform m_PlayerTransform;
    private readonly MonoBehaviour m_RootReferenceMonoBehaviour;
    
    private Vector2 m_InputDirection;
    
    private float m_CurrentAngleSmoothVelocity;
    private float m_CurrentAngleRotation;
    private float m_TargetAngleRotation;
    private float m_DeltaTime;
    private float m_DodgeForce = 10f;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public PlayerNormalMovement(
        IPlayerAttackHandler attackHandler,
        PlayerAttackState attackState,
        CameraController cameraController,
        PlayerMovementState movementState,
        Animator playerAnimator,
        Transform playerTransform,
        MonoBehaviour refMonoBehaviour)
    {
        this.m_AttackHandler = attackHandler ?? throw new ArgumentNullException(nameof(attackHandler));
        this.m_AttackState = attackState ?? throw new ArgumentNullException(nameof(attackState));
        this.m_CameraController = cameraController ?? throw new ArgumentNullException(nameof(cameraController));
        this.m_MovementState = movementState ?? throw new ArgumentNullException(nameof(movementState));
        this.m_PlayerAnimator = playerAnimator ?? throw new ArgumentNullException(nameof(playerAnimator));
        this.m_PlayerTransform = playerTransform ?? throw new ArgumentNullException(nameof(playerTransform));
        this.m_RootReferenceMonoBehaviour =
            refMonoBehaviour ?? throw new ArgumentNullException(nameof(refMonoBehaviour));

        this.m_DeltaTime = Time.deltaTime;
    }

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    // NOTE: This might be the same for the other states. Thus this should exist under 'PlayerMovement'
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
        this.m_PlayerTransform.rotation = Quaternion.Euler(0f, this.m_CurrentAngleRotation, 0f);
    }
    
    public void CalculateMovement()
    {
        // Calculate Move Direction
        this.m_MovementState.MoveDirection = new Vector3(this.m_InputDirection.x, 0, this.m_InputDirection.y).normalized;
        
        // Calculate Rotation
        this.RotatePlayerToMovementDirection();
    }

    public void PerformDodge()
    {
        if (!this.m_MovementState.CanDodge) return;
        
        this.m_PlayerAnimator.SetTrigger("Dodge");
        this.m_PlayerAnimator.ResetTrigger("AttackLight");
        
        if (this.m_AttackState.HasBeenParried)
            this.m_AttackHandler.EndParryAction();

        this.m_RootReferenceMonoBehaviour.StartCoroutine(
            this.ApplyDodgeTranslation(Vector3.forward, this.m_DodgeForce));
        this.m_MovementState.CanDodge = false;
        this.m_AttackHandler.ResetAttack();
        
        // Limitations:
        // -Needs a way of reseting the dodge state after animation events
        // -The coroutine is the same on both normal and lockon
        // -The InputDirection is passed seperately instead of being processed as part of one action.
        // -Dodging affects the action of many other states. Perhaps a n observer wll be needed for exe
    }

    public void SetInputDirection(Vector2 inputDirection) 
        => this.m_InputDirection = inputDirection;

    private void RotatePlayerToMovementDirection()
    {
        this.m_TargetAngleRotation = Mathf.Atan2(this.m_MovementState.MoveDirection.x, this.m_MovementState.MoveDirection.z)
            * Mathf.Rad2Deg + this.m_CameraController.GetCameraEulerAngles().y;
        
        this.m_CurrentAngleRotation = Mathf.SmoothDampAngle(
            this.m_PlayerTransform.eulerAngles.y,
            this.m_TargetAngleRotation,
            ref this.m_CurrentAngleSmoothVelocity,
            MOVEMENT_SMOOTHING_DAMPING_TIME);
    }
    
    private IEnumerator ApplyDodgeTranslation(Vector3 lastDirection, float force)
    {
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
