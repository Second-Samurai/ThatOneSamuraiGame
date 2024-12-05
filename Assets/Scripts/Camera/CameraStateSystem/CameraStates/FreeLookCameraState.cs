using System;
using System.Numerics;
using Cinemachine;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;

public interface IFreelookCameraController
{

    #region - - - - - - Methods - - - - - -

    void SetCameraLocation(Vector3 newPosition);

    void SetCameraRotation(Vector3 eulerAngles);

    #endregion Methods

}

/// <summary>
/// Responsible for allowing the camera to be externally directed without being locked to an object.
/// </summary>
public class FreeLookCameraState : PausableMonoBehaviour, ICameraState, IFreelookCameraController
{

    #region - - - - - - Fields - - - - - -

    public CinemachineVirtualCamera m_FreeLookCamera;

    #endregion Fields
  
    #region - - - - - - Initializers - - - - - -

    public void InitializeState(CameraStateContext context)
    {
    }

    #endregion Initializers
  
    #region - - - - - - Methods - - - - - -

    public void StartState()
    {
        this.m_FreeLookCamera.gameObject.SetActive(true);
    }

    public void EndState()
    {
        this.m_FreeLookCamera.gameObject.SetActive(false);
    }

    public bool ValidateState()
    {
        throw new NotImplementedException();
    }

    SceneCameras ICameraState.GetSceneState()
        => SceneCameras.FreeLook;

    void IFreelookCameraController.SetCameraLocation(Vector3 newPosition)
    {
        throw new NotImplementedException();
    }

    void IFreelookCameraController.SetCameraRotation(Vector3 eulerAngles)
    {
        throw new NotImplementedException();
    }

    #endregion Methods
  
}
