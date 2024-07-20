using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.UI.Pause
{
    
    public class PauseActionHandler : MonoBehaviour, IPauseActionHandler
    {
        
        #region - - - - - - Fields - - - - - -

        private IPauseMenuController m_PauseMenu;

        private bool m_IsPauseScreenDisplayed;

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
            if (this.m_IsPauseScreenDisplayed)
            {
                this.m_PauseMenu.DisplayPauseScreen();
                this.m_IsPauseScreenDisplayed = true;
            }
            else
            {
                this.m_PauseMenu.HidePauseScreen();
                this.m_IsPauseScreenDisplayed = false;
            }
        }

        #endregion Methods
        
    }
    
}
