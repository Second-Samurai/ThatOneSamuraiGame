using System;
using Cinemachine;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

public interface ILockOnCamera
{

    #region - - - - - - Methods - - - - - -

    void SetLockOnTarget(Transform targetTransform);

    void SetFollowedTransform(Transform followTransform);

    #endregion Methods

}

/// <summary>
/// Responsible for locking the camera behavior to a target object.
/// </summary>
public class LockOnCameraState : PausableMonoBehaviour, ICameraState, ILockOnCamera
{

    #region - - - - - - Fields - - - - - -

    public Transform m_FollowCameraTargetPoint;
    public CinemachineFreeLook m_LockOnCamera;
    public CinematicBars m_CinematicBars;

    private Transform m_TargetTransform;
    private Transform m_FollowedTransform;
    private bool m_RunLockCancelTimer = false;

    #endregion Fields
  
    #region - - - - - - Initialize - - - - - -

    public void InitializeState(CameraStateContext context) 
        => this.m_LockOnCamera.Follow = this.m_FollowCameraTargetPoint;

    #endregion Initialize
  
    #region - - - - - - Methods - - - - - -

    // TODO: Remove might be unecessary
    public GameObject GetCameraObject() 
        => this.gameObject;

    public void SetLockOnTarget(Transform targetTransform) 
        => this.m_TargetTransform = targetTransform;

    public void SetFollowedTransform(Transform followTransform) 
        => this.m_FollowedTransform = followTransform;

    public void StartState()
    {
        this.m_LockOnCamera.gameObject.SetActive(true);
        
        GameLogger.Log(
            (nameof(this.m_TargetTransform), this.m_TargetTransform),
            (nameof(this.m_FollowedTransform), this.m_FollowedTransform));
        
        this.m_LockOnCamera.LookAt = this.m_TargetTransform;
        this.m_LockOnCamera.Follow = this.m_FollowedTransform;
        
        this.m_RunLockCancelTimer = false;
        this.m_CinematicBars.ShowBars(200f, .3f); // TODO: Move to lock on controller
    }

    public void EndState()
    {
        this.m_LockOnCamera.gameObject.SetActive(false);
        
        // End LockOn
        this.m_CinematicBars.HideBars(.3f);
    }
    
    public bool ValidateState()
    {
        throw new NotImplementedException();
    }

    public SceneCameras GetSceneState() 
        => SceneCameras.LockOn;

    #endregion Methods
  
}
