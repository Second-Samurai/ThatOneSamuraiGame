﻿using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu;
using UnityEngine;

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

        private IUIEventMediator m_EventMediator;
        private IUIEventCollection m_EventCollection;
        
        private IGuardMeter m_GuardMeter;
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

        public IGuardMeter GuardMeter
        {
            get => this.m_GuardMeter;
            set => this.m_GuardMeter = value;
        }

        public IUIEventMediator UIEventMediator
        {
            get => this.m_EventMediator;
            set => this.m_EventMediator = value;
        }

        public IUIEventCollection UIEventCollection
        {
            get => this.m_EventCollection;
            set => this.m_EventCollection = value;
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