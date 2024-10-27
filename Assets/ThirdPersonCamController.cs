using UnityEngine;
using Cinemachine;

public class ThirdPersonCamController : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public PlayerCamTargetController camTargetController;
    public CinemachineVirtualCamera sprintCam;

    CinemachineVirtualCamera cam;
    
    #endregion Fields
    //
    // #region - - - - - - Unity Lifecycle Methods - - - - - -
    //
    // private void Start()
    // {
    //     if (!camTargetController)
    //     {
    //         Debug.LogWarning("Cam not set in inspector! Trying to find component in game manager");
    //         camTargetController = GameManager.instance.PlayerController.gameObject.transform.parent.GetComponentInChildren<PlayerCamTargetController>();
    //     }
    //     cam = this.GetComponent<CinemachineVirtualCamera>();
    //     if (!cam.m_Follow)
    //     {
    //         Debug.LogWarning("Follow Target not set in inspector! Setting via code");
    //         cam.m_Follow = camTargetController.gameObject.transform;
    //     }
    // }
    //
    // #endregion Unity Lifecycle Methods

    #region - - - - - - Methods - - - - - -

    public void Initialise()
    {
        if (!camTargetController)
        {
            Debug.LogWarning("Cam not set in inspector! Trying to find component in game manager");
            camTargetController = GameManager.instance.PlayerController.gameObject.transform.parent.GetComponentInChildren<PlayerCamTargetController>();
        }
        cam = this.GetComponent<CinemachineVirtualCamera>();
        if (!cam.m_Follow)
        {
            Debug.LogWarning("Follow Target not set in inspector! Setting via code");
            cam.m_Follow = camTargetController.gameObject.transform;
        }
    }

    public void SetPriority(int var)
    {
        cam.m_Priority = var;
    }
    
    public void SetSensitivity(float sensitivity)
    {
        camTargetController.SetSensitivity(sensitivity);
    }
    
    public void SprintOn()
    {
        sprintCam.m_Priority = 15;
    }

    public void SprintOff()
    {
        sprintCam.m_Priority = 5;
    }

    #endregion Methods
  
}
