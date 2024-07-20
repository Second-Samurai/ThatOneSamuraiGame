using System.Collections;
using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour, IPauseMenuController
{

    float timeScale;
    public GameObject optionsMenu;
    PlayerInput _input;
    
    public GameEvent hidePopupEvent;
    public GameEvent hideLockOnPopupEvent;

    // This is an event method invoked by Unity.
    private void OnEnable()
    {
        hidePopupEvent.Raise();
        hideLockOnPopupEvent.Raise();
        
        timeScale = Time.timeScale;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        
        // Below this behavior is legacy
        _input = GameManager.instance.playerController.gameObject.GetComponent<PlayerInput>();
        _input.SwitchCurrentActionMap("Menu");
    }
    
    // This is to replace the above method as it relies on the object being active to run.
    public void DisplayPauseScreen()
    {
        hidePopupEvent.Raise();
        hideLockOnPopupEvent.Raise();
        
        timeScale = Time.timeScale;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        
        IInputManager _InputManager = GameManager.instance.InputManager;
        _InputManager.SwitchToMenuControls();
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        GameManager.instance.checkpointManager.SaveActiveCheckpoint(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeButton()
    {
        Time.timeScale = timeScale;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (GameManager.instance.rewindManager.isTravelling == true)
        {
            _input.SwitchCurrentActionMap("Rewind");
            gameObject.SetActive(false);
        }
        else
        {
            _input.SwitchCurrentActionMap("Gameplay");
            gameObject.SetActive(false);
        }
    }

    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
    }

    public void DisableOptionsMenu()
    {
        optionsMenu.SetActive(false);
    }

    public void HidePauseScreen() 
        => this.gameObject.SetActive(false);
}
