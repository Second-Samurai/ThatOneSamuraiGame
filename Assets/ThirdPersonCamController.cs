using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCamController : MonoBehaviour
{
    public PlayerCamTargetController camTargetController;
    CinemachineVirtualCamera cam;
    
    private void Start()
    {
        if (!camTargetController)
        {
            Debug.LogError("Cam not set in inspector! Trying to find component in game manager");
            camTargetController = GameManager.instance.playerController.gameObject.transform.parent.GetComponentInChildren<PlayerCamTargetController>();
        }
        cam = this.GetComponent<CinemachineVirtualCamera>();
        if (!cam.m_Follow)
        {
            Debug.LogError("Follow Target not set in inspector! Setting via code");
            cam.m_Follow = camTargetController.gameObject.transform;
        }
    }
    // Start is called before the first frame update
    public void SetPriority(int var)
    {
        cam.m_Priority = var;
    }
    public void SetSensitivity(float sensitivity)
    {
        camTargetController.SetSensitivity(sensitivity);
    }
}
