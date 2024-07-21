using ThatOneSamuraiGame.Scripts.Scene.DataContainers;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseActionHandler;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseObserver;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.UI.Pause.PauseMediator
{

    /// <summary>
    /// Responsible for mediating events between pause related components.
    /// </summary>
    public class PauseMediator : MonoBehaviour, IPauseMediator
    {

        #region - - - - - - Fields - - - - - -

        private IPauseMenuController m_PauseMenuController;
        private IPauseObserver m_PauseObserver;
        private SceneState m_SceneState;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PauseMenuController = GameManager.instance.UserInterfaceManager.PauseMenu;
            this.m_PauseObserver = this.GetComponent<IPauseObserver>();
            this.m_SceneState = GameManager.instance.GameState.SceneManagement.GetComponent<SceneState>();
        }

        #endregion Lifecycle Methods

        #region - - - - - - Methods - - - - - -

        void IPauseMediator.Notify(string component, PauseActionType pauseActionType)
        {
            switch (pauseActionType)
            {
                case PauseActionType.Pause:
                    this.HandlePause(component);
                    break;
                case PauseActionType.Unpause:
                    this.HandleUnPause(component);
                    break;
                default:
                    Debug.LogWarning("Unknown action type has been passed in.");
                    break;
            }
        }

        private void HandlePause(string component)
        {
            if (component != nameof(IPauseActionHandler)
                && component != nameof(IPauseMenuController)) return;
            
            this.m_SceneState.IsGamePaused = true;
            this.m_PauseMenuController.DisplayPauseScreen();
            this.m_PauseObserver.PauseAllObjects();
        }

        private void HandleUnPause(string component)
        {
            if (component != nameof(IPauseActionHandler)
                && component != nameof(IPauseMenuController)) return;
            
            this.m_SceneState.IsGamePaused = false;
            this.m_PauseMenuController.HidePauseScreen();
            this.m_PauseObserver.UnpauseAllObjects();
        }

        #endregion Methods
    }

    public enum PauseActionType
    {
        Pause,
        Unpause
    }

}