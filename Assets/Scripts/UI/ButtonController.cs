using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using ThatOneSamuraiGame.Scripts.Input;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam, optionsVCam;
    public GameObject menu, optionsMenu;
    public GameObject cutscene;
    public Button continueButton;
    
    public void CloseMenu()
    {
        GameManager.instance.checkpointManager.ResetCheckpoints();
        GameManager.instance.enemySpawnManager.ResetList();

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
        InputManager _inputManager = GameManager.instance.InputManager;
        _inputManager.SwitchToMenuControls();
    }

    public void EnableContinue()
    {
        continueButton.interactable = true;
    }
}
