using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ButtonController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam, optionsVCam;
    public GameObject menu, optionsMenu;
    
    public void CloseMenu()
    {
        vcam.m_Priority = 0;
        menu.SetActive(false);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
}
