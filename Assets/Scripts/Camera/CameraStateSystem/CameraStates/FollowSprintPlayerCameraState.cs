using Cinemachine;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.ViewOrientation;
using UnityEngine;

/// <summary>
/// Responsible for following the player during sprinting movement.
/// </summary>
public class FollowSprintPlayerCameraState : PausableMonoBehaviour, ICameraState
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private GameObject m_Player;
    [SerializeField] private Transform m_FollowCameraTargetPoint;
    public CinemachineVirtualCamera m_SprintCamera;
    
    [SerializeField] private float m_RotationSpeed = 0.15f; // TODO: Change this to use the player prefs
    [SerializeField] private float m_MinimumPitchAngle = -35f;
    [SerializeField] private float m_MaximumPitchAngle = 60f;
    
    private IPlayerViewOrientationHandler m_PlayerViewOrientationHandler;

    private float m_TargetYaw;
    private float m_TargetPitch;

    #endregion Fields
  
    #region - - - - - - Initializers - - - - - -

    public void InitializeState(CameraStateContext context)
    {
        this.m_PlayerViewOrientationHandler = this.m_Player.GetComponent<IPlayerViewOrientationHandler>();
        _ = this.ValidateState();
    }

    public GameObject GetCameraObject()
    {
        return this.gameObject;
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

    SceneCameras ICameraState.GetSceneState()
        => SceneCameras.FollowSprintPlayer;

    void ICameraState.StartState()
    {
        this.m_SprintCamera.gameObject.SetActive(true);
    }

    void ICameraState.EndState()
    {
        this.m_SprintCamera.gameObject.SetActive(false);
    }

    public bool ValidateState()
        => GameValidator.NotNull(this.m_Player, nameof(this.m_Player))
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
        
        this.m_FollowCameraTargetPoint.rotation = Quaternion.Euler(
            new Vector3(
                this.m_TargetPitch, 
                this.m_TargetYaw, 
                this.m_FollowCameraTargetPoint.eulerAngles.z));
    }
    
    #endregion Methods
  
}
