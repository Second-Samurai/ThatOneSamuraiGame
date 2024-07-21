using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Scene.DataContainers;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMediator;

namespace ThatOneSamuraiGame.Scripts.UI.Pause.PauseActionHandler
{
    
    public class PauseActionHandler : TOSGMonoBehaviourBase, IPauseActionHandler
    {
        
        #region - - - - - - Fields - - - - - -

        private IPauseMediator m_PauseMediator;
        private SceneState m_SceneState;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            GameState _GameState = GameManager.instance.GameState;
            this.m_PauseMediator = _GameState.PauseManager.GetComponent<IPauseMediator>();
            this.m_SceneState = _GameState.SceneManagement.GetComponent<SceneState>();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -
        
        void IPauseActionHandler.TogglePause() 
            => this.m_PauseMediator.Notify(
                    nameof(IPauseActionHandler), 
                    this.m_SceneState.IsGamePaused 
                        ? PauseActionType.Unpause
                        : PauseActionType.Pause);

        #endregion Methods

    }
    
}
