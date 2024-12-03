using System;
using Cinemachine;
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
    [SerializeField] private float m_RotationSpeed = 0.15f; // TODO: Change this to use the player prefs
    [SerializeField] private float m_MinimumPitchAngle = -35f;
    [SerializeField] private float m_MaximumPitchAngle = 60f;

    public CinemachineVirtualCamera m_FollowCamera;

    private IPlayerViewOrientationHandler m_PlayerViewOrientationHandler;
    private ICameraController m_CameraController;

    private float m_TargetYaw;
    private float m_TargetPitch;

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

    private void LateUpdate()
    {
        if (this.IsPaused) return;
        
        this.ApplyViewOrientation();
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -
    
    public void StartState()
    {
        this.m_FollowCamera.gameObject.SetActive(true);
    }

    public void EndState()
    {
        this.m_FollowCamera.gameObject.SetActive(false);
    }

    public bool ValidateState()
        => GameValidator.NotNull(this.m_CameraController, nameof(this.m_CameraController))
           && GameValidator.NotNull(this.m_Player, nameof(this.m_Player))
           && GameValidator.NotNull(this.m_FollowCameraTargetPoint, nameof(this.m_FollowCameraTargetPoint));

    private void ApplyViewOrientation()
    {
        Vector2 _InputScreenPosition = this.m_PlayerViewOrientationHandler.GetInputScreenPosition();
        
        if (_InputScreenPosition == Vector2.zero) 
            return;

        this.m_TargetYaw =
            Mathf.Clamp(this.m_TargetYaw + _InputScreenPosition.x * this.m_RotationSpeed, float.MinValue, float.MaxValue);
        this.m_TargetPitch =
            Mathf.Clamp(this.m_TargetPitch + _InputScreenPosition.y * this.m_RotationSpeed * -1, this.m_MinimumPitchAngle,
                this.m_MaximumPitchAngle);
        
        m_FollowCameraTargetPoint.rotation = Quaternion.Euler(new Vector3(this.m_TargetPitch, this.m_TargetYaw, this.m_FollowCameraTargetPoint.eulerAngles.z));
    }

    #endregion Methods

}
