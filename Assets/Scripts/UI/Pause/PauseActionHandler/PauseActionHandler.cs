using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Scene.DataContainers;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
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
            GameManager _GameManager = GameManager.instance;
            this.m_PauseMediator = _GameManager.PauseManager.PauseMediator;
            this.m_SceneState = ((ISceneManager)SceneManager.Instance).SceneState;
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
