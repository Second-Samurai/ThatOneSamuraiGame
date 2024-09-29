using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

// Note: This class is called the button controller but is attached to the 'Quit' UI Button.
//  Its purpose is to manage the screen state after quitting the game.
//      - This is only for the SwordCanvas and switches between menus 
//      - Closes the menu but also handles the quit game behavior

public class ButtonController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam, optionsVCam;
    public GameObject menu, optionsMenu;
    public GameObject cutscene;
    public Button continueButton;
    
    public void CloseMenu()
    {
        GameManager.instance.CheckpointManager.ResetCheckpoints();
        GameManager.instance.EnemySpawnManager.ResetList();

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

    // Opens the Options menu, de-activates the main menu
    public void OptionsMenu()
    {
        vcam.m_Priority = 0;
        menu.SetActive(false);
        optionsVCam.m_Priority = 20;
        optionsMenu.SetActive(true);
    }

    // Opens the MainMenu, de-activates the options menu
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
        GameManager.instance.CheckpointManager.LoadCheckpoint();
    }

    public void EnableContinue()
    {
        continueButton.interactable = true;
    }
}
