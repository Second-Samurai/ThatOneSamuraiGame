using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.UI.Pause
{
    
    public class PauseActionHandler : MonoBehaviour, IPauseActionHandler
    {
        
        #region - - - - - - Fields - - - - - -

        private IPauseMenuController m_PauseMenu;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PauseMenu = GameManager.instance.UserInterfaceManager.PauseMenu;
        }    

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -
        
        void IPauseActionHandler.TogglePause()
        {
            throw new System.NotImplementedException();
        }

        #endregion Methods
        
    }
    
}
