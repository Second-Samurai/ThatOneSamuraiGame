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

    GameObject GetCamera(SceneCameras targetCamera);

    Vector3 GetCameraEulerAngles();

    void SelectCamera(SceneCameras selectedCamera);

    #endregion Methods

}

public class CameraController : PausableMonoBehaviour, ICameraController
{

    #region - - - - - - Fields - - - - - -

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

    public Vector3 GetCameraEulerAngles()
    {
        return Camera.main == null ? Vector3.zero : Camera.main.transform.eulerAngles;
    }

    public void SelectCamera(SceneCameras selectedCamera)
    {
        this.m_CameraStateSystem.SetState(selectedCamera);
    }

    public GameObject GetCamera(SceneCameras targetCamera)
    {
        return this.m_CameraStateSystem.GetCameraStates()
            .Single(cs => cs.GetSceneState() == targetCamera)
            .GetCameraObject();
    }

    public IFreelookCameraController GetFreelookCameraController()
    {
        // TODO: Rename the method below to .GetCurrentStateObject.
        // Possibly also 
        IFreelookCameraController _Controller =
            this.m_CameraStateSystem.GetGetCurrentState().GetComponent<IFreelookCameraController>();

        if (_Controller == null)
        {
            GameLogger.LogWarning($"No '{nameof(IFreelookCameraController)} was found. Please switch to the state before retrieval.");
            return null;
        }

        return _Controller;
    }

    #endregion Methods

}
