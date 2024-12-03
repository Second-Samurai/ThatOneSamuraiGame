using Cinemachine;
using ThatOneSamuraiGame.Scripts.Base;

/// <summary>
/// Responsible for following the player during sprinting movement.
/// </summary>
public class FollowSprintPlayerCameraState : PausableMonoBehaviour, ICameraState
{

    #region - - - - - - Fields - - - - - -

    public CinemachineVirtualCamera m_SprintCamera;

    #endregion Fields
  
    #region - - - - - - Initializers - - - - - -

    void ICameraState.InitializeState(CameraStateContext context)
    {
    }

    #endregion Initializers
  
    #region - - - - - - Methods - - - - - -

    void ICameraState.StartState()
    {
        this.m_SprintCamera.gameObject.SetActive(true);
    }

    void ICameraState.EndState()
    {
        this.m_SprintCamera.gameObject.SetActive(false);
    }

    bool ICameraState.ValidateState()
    {
        return true;
    }

    #endregion Methods
  
}
