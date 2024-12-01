using Cinemachine;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

public interface ICameraController
{

    #region - - - - - - Methods - - - - - -

    void SelectCamera(SceneCameras selectedCamera);

    void SetCameraRotation(Vector3 eulerRotation);

    #endregion Methods

}

public class CameraController : PausableMonoBehaviour, ICameraController
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private CinemachineBlendListCamera m_CameraBlendList;
    private ICameraStateSystem m_CameraStateSystem;
    
    private ICinemachineCamera m_ActiveCamera;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_CameraStateSystem = this.GetComponent<ICameraStateSystem>();
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public void SelectCamera(SceneCameras selectedCamera)
    {
        if (selectedCamera < 0 || selectedCamera >= this.m_CameraBlendList.ChildCameras.Length)
        {
            GameLogger.LogError("Invalid camera index provided.");
            return;
        }

        this.m_CameraBlendList.Priority = selectedCamera;
        this.m_CameraBlendList.LiveChild = this.m_CameraBlendList.ChildCameras[selectedCamera];

        Debug.Log(this.m_CameraBlendList.Priority);
        Debug.Log(this.m_CameraBlendList.LiveChild);
        
        this.m_ActiveCamera = this.m_CameraBlendList.LiveChild;
        this.m_CameraStateSystem.SetState(SceneCameras.FollowPlayer);
    }

    public void SetCameraRotation(Vector3 eulerRotation)
    {
        if (!GameValidator.NotNull(this.m_ActiveCamera, nameof(m_ActiveCamera)) || !this.m_ActiveCamera.IsValid)
            return;

        Transform _CameraTransform = this.m_ActiveCamera.VirtualCameraGameObject.transform;
        _CameraTransform.Rotate(Vector3.up, eulerRotation.x);
        _CameraTransform.Rotate(Vector3.right, eulerRotation.y);
    }

    #endregion Methods

}
