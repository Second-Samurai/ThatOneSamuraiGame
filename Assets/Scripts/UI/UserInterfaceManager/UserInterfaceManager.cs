using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager
{

    /// <summary>
    /// Responsible for managing initialisation, state and view presentation of graphical user interfaces.
    /// </summary>
    public class UserInterfaceManager: MonoBehaviour, IUserInterfaceManager
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField] private GameSettings m_GameSettings;
        [SerializeField] private ButtonController m_SwordCanvasController; // This is known as the 'ButtonController' from its source.
        
        private GameObject m_GuardMeterCanvas;
        private IPauseMenuController m_PauseMenuController;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        IPauseMenuController IUserInterfaceManager.PauseMenu
            => this.m_PauseMenuController;

        ButtonController IUserInterfaceManager.ButtonController
            => this.m_SwordCanvasController;

        #endregion Properties

        #region - - - - - - Methods - - - - - -

        void IUserInterfaceManager.SetupUserInterface()
        {
            this.m_PauseMenuController = Object.FindFirstObjectByType<PauseMenu>();
            this.m_GuardMeterCanvas = Object.Instantiate(
                                        this.m_GameSettings.guardCanvasPrefab, 
                                        transform.position,
                                        Quaternion.identity);
        }

        #endregion Methods
        
    }

}