using System;
using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public class SceneUserInterfaceSetupHandler : MonoBehaviour, ISetupHandler
    {
        
        #region - - - - - - Fields - - - - - -

        private ISetupHandler m_NextHandler;

        #endregion Fields
        
        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle()
        {
            // Setup the ButtonController (actually is known as a Sword Canvas Controller)
            ButtonController _ButtonController = FindFirstObjectByType<ButtonController>();
            GameManager.instance.UserInterfaceManager.ButtonController = _ButtonController;
            
            this.m_NextHandler?.Handle();
        }

        #endregion Methods
  
    }

}