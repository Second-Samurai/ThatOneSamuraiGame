using System;
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
    
    private readonly CameraController m_CameraController;
    private readonly PlayerMovementState m_MovementState;
    private readonly Animator m_PlayerAnimator;
    private readonly Transform m_PlayerTransform;
    
    private Vector2 m_InputDirection;
    
    private float m_CurrentAngleSmoothVelocity;
    private float m_CurrentAngleRotation;
    private float m_TargetAngleRotation;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public PlayerNormalMovement(
        CameraController cameraController,
        PlayerMovementState movementState,
        Animator playerAnimator,
        Transform playerTransform)
    {
        this.m_CameraController = cameraController ?? throw new ArgumentNullException(nameof(cameraController));
        this.m_MovementState = movementState ?? throw new ArgumentNullException(nameof(movementState));
        this.m_PlayerAnimator = playerAnimator ?? throw new ArgumentNullException(nameof(playerAnimator));
        this.m_PlayerTransform = playerTransform ?? throw new ArgumentNullException(nameof(playerTransform));
    }

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    public void ApplyMovement()
    {
        // Invokes player movement through the physically based animation movements
        this.m_PlayerAnimator.SetFloat(
            "XInput", 
            this.m_InputDirection.x, 
            MOVEMENT_SMOOTHING_DAMPING_TIME,
            Time.deltaTime);
            
        this.m_PlayerAnimator.SetFloat(
            "YInput",
            this.m_InputDirection.y,
            MOVEMENT_SMOOTHING_DAMPING_TIME,
            Time.deltaTime);
            
        this.m_PlayerAnimator.SetFloat(
            "InputSpeed",
            this.m_InputDirection.magnitude,
            MOVEMENT_SMOOTHING_DAMPING_TIME,
            Time.deltaTime);
        
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

    #endregion Methods
  
}
