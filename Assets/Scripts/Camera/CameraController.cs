using System.Linq;
using Cinemachine;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

/// <summary>
/// Responsible for main control of the Camera's Systems.
/// This is mainly a facade to each of the different camera systems.
/// </summary>
public class CameraController : PausableMonoBehaviour, ICameraController
{

    #region - - - - - - Fields - - - - - -
    
    private ICameraActionHandler m_CameraActionHandler;
    private ICameraStateSystem m_CameraStateSystem;
    private ICinemachineCamera m_ActiveCamera;
    private Camera m_MainCamera;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Awake()
    {
        this.m_CameraActionHandler = this.GetComponent<ICameraActionHandler>();
        this.m_CameraStateSystem = this.GetComponent<ICameraStateSystem>();
        this.m_CameraStateSystem.Initialize();
    }

    private void Start() 
        => this.m_MainCamera = Camera.main;

    #endregion Unity Methods

    #region - - - - - - CameraStateSystem Methods - - - - - -

    public Vector3 GetCameraEulerAngles() 
        => !this.m_MainCamera ? Vector3.zero : this.m_MainCamera.transform.eulerAngles;

    public void SelectCamera(SceneCameras selectedCamera) 
        => this.m_CameraStateSystem.SetState(selectedCamera);

    public GameObject GetCamera(SceneCameras targetCamera)
    {
        return this.m_CameraStateSystem.GetCameraStates()
            .Single(cs => cs.GetSceneState() == targetCamera)
            .GetCameraObject();
    }

    #endregion CameraStateSystem Methods

    #region - - - - - - CameraActionHandler Methods - - - - - -

    // This both sets and starts the camera action.
    public void SetCameraAction(ICameraAction cameraAction)
        => this.m_CameraActionHandler.SetCameraAction(cameraAction);

    public void EndCameraAction()
        => this.m_CameraActionHandler.EndCameraAction();

    #endregion CameraActionHandler Methods

}
