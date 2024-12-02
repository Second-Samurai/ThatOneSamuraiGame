using System.Collections.Generic;
using System.Linq;
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

    // [SerializeField] private CinemachineBlendListCamera m_CameraBlendList;
    [SerializeField] private List<CinemachineVirtualCamera> m_CameraList;
    
    private ICameraStateSystem m_CameraStateSystem;
    private ICinemachineCamera m_ActiveCamera;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Awake()
    {
        this.m_CameraStateSystem = this.GetComponent<ICameraStateSystem>();
        this.m_CameraStateSystem.Initialize();
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public void SelectCamera(SceneCameras selectedCamera)
    {
        if (selectedCamera < 0 || this.m_CameraList.All(cc => cc.Priority != selectedCamera))
        // if (selectedCamera < 0 || this.m_CameraBlendList.ChildCameras.All(cc => cc.Priority != selectedCamera))
        {
            GameLogger.LogError("Invalid camera index provided.");
            return;
        }

        this.m_ActiveCamera = this.m_CameraList.Single(cvc => cvc.Priority == selectedCamera);
        // this.m_ActiveCamera.Priority = selectedCamera;

        Debug.Log(this.m_ActiveCamera.Priority);
        Debug.Log(this.m_ActiveCamera.Name);
        this.m_CameraStateSystem.SetState(SceneCameras.FollowPlayer);
    }

    // TODO: Move this logic to the camera transformer or modifier
    public void SetCameraRotation(Vector3 eulerRotation)
    {
        Debug.Log("Active Camera: " + CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.Name);
        return;
        
        if (!GameValidator.NotNull(this.m_ActiveCamera, nameof(this.m_ActiveCamera)) || !this.m_ActiveCamera.IsValid)
            return;

        Transform _CameraTransform = this.m_ActiveCamera.VirtualCameraGameObject.transform;
        _CameraTransform.rotation = Quaternion.Euler(eulerRotation);
    }

    #endregion Methods

}
