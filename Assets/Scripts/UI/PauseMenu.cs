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

    // Note: Keep legacy code
    // private void OnEnable()
    // {
    //     hidePopupEvent.Raise();
    //     hideLockOnPopupEvent.Raise();
    //     
    //     timeScale = Time.timeScale;
    //     Time.timeScale = 0f;
    //     Cursor.visible = true;
    //     Cursor.lockState = CursorLockMode.Confined;
    //     
    //     IInputManager _InputManager = GameManager.instance.InputManager;
    //     _InputManager.SwitchToMenuControls();
    // }
    
    // This is to replace the above method as it relies on the object being active to run.
    public void DisplayPauseScreen()
    {
        this.gameObject.SetActive(true);
        
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
        => this.UnPauseGameplay();

    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
    }

    public void DisableOptionsMenu()
    {
        optionsMenu.SetActive(false);
    }

    public void HidePauseScreen() 
        => this.UnPauseGameplay();

    // Note: This is a temp method to prevent repetitive code.
    private void UnPauseGameplay()
    {
        IInputManager _InputManager = GameManager.instance.InputManager;
        
        Time.timeScale = timeScale;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        if (GameManager.instance.rewindManager.isTravelling == true)
        {
            _InputManager.SwitchToRewindControls();
            gameObject.SetActive(false);
        }
        else
        {
            _InputManager.SwitchToGameplayControls();
            gameObject.SetActive(false);
        }
    }
}
