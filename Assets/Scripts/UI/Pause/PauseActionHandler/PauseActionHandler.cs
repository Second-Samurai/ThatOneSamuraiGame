using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.UI.Pause.PauseActionHandler
{
    
    public class PauseActionHandler : TOSGMonoBehaviourBase, IPauseActionHandler
    {
        
        #region - - - - - - Fields - - - - - -

        // Menus
        private IPauseMenuController m_PauseMenu;

        // Local Fields
        private bool m_IsPauseScreenDisplayed;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start() 
            => this.m_PauseMenu = GameManager.instance.UserInterfaceManager.PauseMenu;

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -
        
        void IPauseActionHandler.TogglePause()
        {
            if (!this.m_IsPauseScreenDisplayed)
            {
                this.m_PauseMenu.DisplayPauseScreen();
                this.m_IsPauseScreenDisplayed = true;
                return;
            }
            
            this.m_PauseMenu.HidePauseScreen();
            this.m_IsPauseScreenDisplayed = false;
        }
        public override void OnPause()
        {
            base.OnPause();
            this.m_IsPauseScreenDisplayed = true;
        }

        public override void OnUnPause()
        {
            base.OnUnPause();
            this.m_IsPauseScreenDisplayed = false;
        }

        #endregion Methods

    }
    
}
