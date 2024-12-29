using System;
using Cinemachine;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;
using UnityEngine.Rendering;
using Vector3 = System.Numerics.Vector3;

public interface IFreelookCameraController
{

    #region - - - - - - Properties - - - - - -

    float DutchAngle { get; set; }
    
    float FieldOfView { get; set; }

    #endregion Properties
  
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

    #region - - - - - - Properties - - - - - -

    float IFreelookCameraController.DutchAngle
    {
        get => this.m_FreeLookCamera.m_Lens.Dutch;
        set => this.m_FreeLookCamera.m_Lens.Dutch = value;
    }

    float IFreelookCameraController.FieldOfView
    {
        get => this.m_FreeLookCamera.m_Lens.FieldOfView;
        set => this.m_FreeLookCamera.m_Lens.FieldOfView = value;
    }
    
    #endregion Properties
  
    #region - - - - - - Initializers - - - - - -

    public void InitializeState(CameraStateContext context)
    {
    }

    #endregion Initializers

    public GameObject GetCameraObject()
    {
        return this.gameObject;
    }
    
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
