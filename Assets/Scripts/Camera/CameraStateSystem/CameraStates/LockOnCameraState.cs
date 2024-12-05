using Cinemachine;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;

/// <summary>
/// Responsible for locking the camera behavior to a target object.
/// </summary>
public class LockOnCameraState : PausableMonoBehaviour, ICameraState
{
    public CinemachineFreeLook m_LockOnCamera;
    public CinematicBars m_CinematicBars;

    private bool m_RunLockCancelTimer = false;
    
    public void InitializeState(CameraStateContext context)
    {
        throw new System.NotImplementedException();
    }

    public void StartState()
    {
        this.m_LockOnCamera.gameObject.SetActive(true);
        
        // Trigger LockOn
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
        throw new System.NotImplementedException();
    }

    public SceneCameras GetSceneState()
    {
        throw new System.NotImplementedException();
    }
}
