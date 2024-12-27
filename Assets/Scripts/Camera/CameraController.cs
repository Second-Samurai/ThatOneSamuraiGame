using System.Linq;
using Cinemachine;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

public interface ICameraController
{

    #region - - - - - - Methods - - - - - -

    GameObject GetCamera(SceneCameras targetCamera);

    Vector3 GetCameraEulerAngles();

    void SelectCamera(SceneCameras selectedCamera);

    #endregion Methods

}

public class CameraController : PausableMonoBehaviour, ICameraController
{

    #region - - - - - - Fields - - - - - -
    
    private ICameraStateSystem m_CameraStateSystem;
    private ICinemachineCamera m_ActiveCamera;
    private Camera m_MainCamera;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Awake()
    {
        this.m_CameraStateSystem = this.GetComponent<ICameraStateSystem>();
        this.m_CameraStateSystem.Initialize();
    }

    private void Start() 
        => this.m_MainCamera = Camera.main;

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

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

    #endregion Methods

}
