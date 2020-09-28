using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem; 

public class ButtonController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam, optionsVCam;
    public GameObject menu, optionsMenu;
    PlayerInput _input;
    public GameObject cutscene;
    
    public void CloseMenu()
    {
        vcam.m_Priority = 0;
        optionsVCam.m_Priority = 0;
        optionsMenu.SetActive(false);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //_input.SwitchCurrentActionMap("Gameplay");
        cutscene.SetActive(true);
        menu.SetActive(false);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        vcam.m_Priority = 0;
        menu.SetActive(false);
        optionsVCam.m_Priority = 20;
        optionsMenu.SetActive(true);
    }

    public void MainMenu()
    {
        optionsVCam.m_Priority = 0;
        optionsMenu.SetActive(false);
        vcam.m_Priority = 20;
        menu.SetActive(true);
    }

    public void Continue()
    {
        vcam.m_Priority = 0;
        optionsVCam.m_Priority = 0;
        optionsMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
        menu.SetActive(false);
        GameManager.instance.checkpointManager.LoadCheckpoint();
    }

    private void Start()
    {
        _input = GameManager.instance.playerController.gameObject.GetComponent<PlayerInput>();
        _input.SwitchCurrentActionMap("Menu");
    }
}
