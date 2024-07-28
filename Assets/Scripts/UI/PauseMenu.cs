using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Input.Gameplay;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseActionHandler;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMediator;
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

    [SerializeField]
    private IPauseMediator m_PauseMediator;

    // Note: Keeping legacy code until whole class is cut over.
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

    #region - - - - - - Lifecycle Methods - - - - - -

    private void Start()
        => this.m_PauseMediator = GameManager.instance.PauseManager.PauseMediator;

    #endregion Lifecycle Methods

    #region - - - - - - Methods - - - - - -

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

        // Included switching the below variable as the rewind behavior is poorly managed to discretely define concrete states
        GameManager.instance.RewindManager.isTravelling = false;
        
        IInputManager _InputManager = GameManager.instance.InputManager;
        _InputManager.SwitchToMenuControls();
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        GameManager.instance.CheckpointManager.SaveActiveCheckpoint(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeButton()
        => this.m_PauseMediator.Notify(nameof(IPauseMenuController), PauseActionType.Unpause);

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
        
        if (GameManager.instance.RewindManager.isTravelling == true // relying on this alone is not enough, additional checks added to differentiate state.
            && GameManager.instance.RewindManager.rewindUI.isActiveAndEnabled) 
        {
            _InputManager.SwitchToRewindControls();
            gameObject.SetActive(false);
        }
        else
        {
            _InputManager.SwitchToGameplayControls();
            gameObject.SetActive(false);
        }

        #endregion Methods
        
    }
    
}
