using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    float timeScale;
    public GameObject optionsMenu;
    PlayerInput _input;
    
    public GameEvent hidePopupEvent;
    public GameEvent hideLockOnPopupEvent;

    private void OnEnable()
    {
        hidePopupEvent.Raise();
        hideLockOnPopupEvent.Raise();
        
        timeScale = Time.timeScale;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        _input = GameManager.instance.playerController.gameObject.GetComponent<PlayerInput>();
        _input.SwitchCurrentActionMap("Menu");
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
}
