using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager
{

    /// <summary>
    /// Responsible for managing initialisation, state and view presentation of graphical user interfaces.
    /// </summary>
    public class UserInterfaceManager: MonoBehaviour, IUserInterfaceManager
    {

        #region - - - - - - Fields - - - - - -

        // Menus
        private IPauseMenuController m_PauseMenuController;
        
        private UserInterfaceConfiguration m_UserInterfaceConfiguration;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        IPauseMenuController IUserInterfaceManager.PauseMenu
            => this.m_PauseMenuController;

        #endregion Properties

        #region - - - - - - Lifecycle Methods - - - - - -

        // Note: Persistent services referred by the GameManager must initialise before MonoBehaviour.Start. 
        private void Awake()
        {
            this.m_UserInterfaceConfiguration = this.GetComponent<UserInterfaceConfiguration>();
            
            // Menus
            this.m_PauseMenuController =
                this.m_UserInterfaceConfiguration.PauseMenu.GetComponent<IPauseMenuController>();
        }
        
        #endregion Lifecycle Methods
        
    }

}