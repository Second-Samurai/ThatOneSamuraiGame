﻿using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public class SceneUserInterfaceSetupHandler : MonoBehaviour, ISetupHandler
    {
        
        #region - - - - - - Fields - - - - - -

        [SerializeField] private GameSettings m_GameSettings;
        [SerializeField] private PauseMenu m_PauseMenu;
        [SerializeField] private GameObject m_GuardMeter;
        [SerializeField] private UIEventMediator m_EventMediator;
        
        private ISetupHandler m_NextHandler;

        #endregion Fields
        
        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle(SceneSetupContext setupContext)
        {
            // Setup the ButtonController (actually is known as a Sword Canvas Controller)
            ButtonController _ButtonController = FindFirstObjectByType<ButtonController>();
            
            UserInterfaceManager.Instance.ButtonController = _ButtonController;
            UserInterfaceManager.Instance.PauseMenu = this.m_PauseMenu;
            UserInterfaceManager.Instance.GuardMeter = this.m_GuardMeter.GetComponent<IGuardMeter>();
            UserInterfaceManager.Instance.UIEventMediator = this.m_EventMediator;
            UserInterfaceManager.Instance.UIEventCollection = this.m_EventMediator;

            GameManager.instance.InputManager.ConfigureMenuInputControl();
            GameManager.instance.PauseManager.PauseMediator.SetPauseMenuController(this.m_PauseMenu);
            
            this.m_NextHandler?.Handle(setupContext);
        }

        #endregion Methods
  
    }

}