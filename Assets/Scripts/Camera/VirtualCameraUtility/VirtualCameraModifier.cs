using Cinemachine;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

public class VirtualCameraModifier : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField, RequiredField] private CinemachineVirtualCamera m_VirtualCamera;
    [SerializeField] private SceneCameraEnum m_CameraSelection;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    public void ApplyVirtualCameraSettings()
    {
        Cinemachine3rdPersonFollow _Camera3rdPersonFollow = this.m_VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        VirtualCameraInfo _CameraModifierInfo = this.GetCameraSettings();

        if (!GameValidator.NotNull(this.m_VirtualCamera.Follow, nameof(this.m_VirtualCamera.Follow))
            && !GameValidator.NotNull(_Camera3rdPersonFollow, nameof(_Camera3rdPersonFollow)))
            return;

        _Camera3rdPersonFollow.Damping = _CameraModifierInfo.Damping;
        _Camera3rdPersonFollow.ShoulderOffset = _CameraModifierInfo.ShoulderOffset;
        _Camera3rdPersonFollow.CameraSide = _CameraModifierInfo.CameraSide;
        _Camera3rdPersonFollow.CameraDistance = _CameraModifierInfo.CameraDistance;
    }

    private VirtualCameraInfo GetCameraSettings()
    {
        CameraSettings _Settings = GameManager.instance.CameraSettings;
        switch (this.m_CameraSelection)
        {
            case SceneCameraEnum.FollowPlayer:
                return _Settings.FollowCameraInfo;
            case SceneCameraEnum.FollowSprintPlayer:
                return _Settings.SprintCameraInfo;
            case SceneCameraEnum.FreeLook:
                GameLogger.LogError("No camera info exists for 'FreeLook'");
                break;
            case SceneCameraEnum.LockOn:
                GameLogger.LogError("No camera info exists for 'LockOn'");
                break;
        }

        return new VirtualCameraInfo();
    }

    #endregion Methods

}
