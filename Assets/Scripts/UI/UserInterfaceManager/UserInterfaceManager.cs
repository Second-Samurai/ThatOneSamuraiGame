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
        private IPauseMenuController m_PauseController;
        
        // Coupled fields
        private UserInterfaceConfiguration m_UserInterfaceConfiguration;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        IPauseMenuController IUserInterfaceManager.PauseMenu
            => this.m_PauseController;

        #endregion Properties

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_UserInterfaceConfiguration = this.GetComponent<UserInterfaceConfiguration>();
            
            // Menus
            this.m_PauseController = this.GetComponent<IPauseMenuController>();
        }

        #endregion Lifecycle Methods
        
    }

}