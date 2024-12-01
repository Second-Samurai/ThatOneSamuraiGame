using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.ViewOrientation;
using UnityEngine;

/// <summary>
/// Responsible for following player during normal movement.
/// </summary>
public class FollowPlayerCameraState : PausableMonoBehaviour, ICameraState
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private GameObject m_Player;
    [SerializeField] private Transform m_FollowCameraTargetPoint;
    [SerializeField] private float m_RotationSpeed = 1f;
    [SerializeField] private float m_MinimumPitchAngle = -35f;
    [SerializeField] private float m_MaximumPitchAngle = 60f;

    private IPlayerViewOrientationHandler m_PlayerViewOrientationHandler;
    private ICameraController m_CameraController;

    #endregion Fields
  
    #region - - - - - - Initializers - - - - - -

    public void InitializeState(CameraStateContext context)
    {
        this.m_CameraController =
            context.CameraController ?? throw new ArgumentNullException(nameof(context.CameraController));
        this.m_PlayerViewOrientationHandler = this.m_Player.GetComponent<IPlayerViewOrientationHandler>();
        
        _ = this.ValidateState();
    }

    #endregion Initializers

    #region - - - - - - Unity Methods - - - - - -

    private void Update()
    {
        if (this.IsPaused) return;
        
        this.ApplyViewOrientation();
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -
    
    public void StartState()
    {
        if (!this.ValidateState()) return;
    }

    public void EndState()
    {
        throw new NotImplementedException();
    }

    public bool ValidateState()
        => GameValidator.NotNull(this.m_CameraController, nameof(this.m_CameraController))
           && GameValidator.NotNull(this.m_Player, nameof(this.m_Player))
           && GameValidator.NotNull(this.m_FollowCameraTargetPoint, nameof(this.m_FollowCameraTargetPoint));

    private void ApplyViewOrientation()
    {
        Vector2 _InputScreenPosition = this.m_PlayerViewOrientationHandler.GetInputScreenPosition();
        this.m_CameraController.SetCameraRotation(new Vector3(
            x: _InputScreenPosition.x * this.m_RotationSpeed, 
            y: Mathf.Clamp(
                _InputScreenPosition.y * this.m_RotationSpeed, 
                this.m_MinimumPitchAngle, 
                this.m_MaximumPitchAngle), 
            z: 0));
    }

    #endregion Methods

}
