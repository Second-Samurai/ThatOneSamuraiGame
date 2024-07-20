using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager
{

    /// <summary>
    /// Centralized gateway to user interface provider and management.
    /// </summary>
    public class UserInterfaceManager: MonoBehaviour, IUserInterfaceManager
    {

        #region - - - - - - Fields - - - - - -

        // Menus
        private IPauseMenuController m_PauseMenuController;
        
        // Coupled fields
        private UserInterfaceConfiguration m_UserInterfaceConfiguration;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        IPauseMenuController IUserInterfaceManager.PauseMenu
            => this.m_PauseMenuController;

        #endregion Properties

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_UserInterfaceConfiguration = this.GetComponent<UserInterfaceConfiguration>();
            
            // Menus
            this.m_PauseMenuController =
                this.m_UserInterfaceConfiguration.PauseMenu.GetComponent<IPauseMenuController>();
        }
        
        #endregion Lifecycle Methods
        
    }

}