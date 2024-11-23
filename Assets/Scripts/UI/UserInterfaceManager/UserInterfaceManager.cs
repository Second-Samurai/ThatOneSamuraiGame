using System.Runtime.CompilerServices;
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

        public static UserInterfaceManager Instance;

        [SerializeField] private GameSettings m_GameSettings;
        [SerializeField] private ButtonController m_SwordCanvasController; // This is known as the 'ButtonController' from its source.
        
        private GameObject m_GuardMeterCanvas;
        private IPauseMenuController m_PauseMenuController;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public IPauseMenuController PauseMenu
        {
            get => this.m_PauseMenuController;
            set => this.m_PauseMenuController = value;
        }

        public ButtonController ButtonController
        {
            get => this.m_SwordCanvasController;
            set => this.m_SwordCanvasController = value;
        }

        public GameObject GuardMeterCanvas
        {
            get => this.m_GuardMeterCanvas;
            set => this.m_GuardMeterCanvas = value;
        }

        #endregion Properties

        #region - - - - - - Unity Methods - - - - - -

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }


        #endregion Unity Methods
  
        #region - - - - - - Methods - - - - - -

        public bool IsMembersValid()
        {
            return GameValidator.NotNull(this.m_GameSettings, nameof(this.m_GameSettings))
                   && GameValidator.NotNull(this.m_PauseMenuController, nameof(this.m_PauseMenuController))
                   && GameValidator.NotNull(this.m_SwordCanvasController, nameof(this.m_SwordCanvasController));
        }

        public void SetupUserInterface()
        {
        }

        #endregion Methods
        
    }

}