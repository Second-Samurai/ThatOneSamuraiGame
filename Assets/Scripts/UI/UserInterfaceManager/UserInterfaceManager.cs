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

        [SerializeField] private GameSettings m_GameSettings;
        [SerializeField] private ButtonController m_ButtonController;
        private GameObject m_GuardMeterCanvas;
        
        private IPauseMenuController m_PauseMenuController;
        private UserInterfaceConfiguration m_UserInterfaceConfiguration;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        IPauseMenuController IUserInterfaceManager.PauseMenu
            => this.m_PauseMenuController;

        ButtonController IUserInterfaceManager.ButtonController
            => this.m_ButtonController;

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

        #region - - - - - - Methods - - - - - -

        void IUserInterfaceManager.SetupUserInterface()
        {
            this.m_GuardMeterCanvas = Object.Instantiate(
                                        this.m_GameSettings.guardCanvasPrefab, 
                                        transform.position,
                                        Quaternion.identity);
        }

        #endregion Methods
        
    }

}