using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public class SceneUserInterfaceSetupHandler : MonoBehaviour, ISetupHandler
    {
        
        #region - - - - - - Fields - - - - - -

        [SerializeField] private GameSettings m_GameSettings;
        [SerializeField] private PauseMenu m_PauseMenu;
        
        private ISetupHandler m_NextHandler;

        #endregion Fields
        
        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle()
        {
            // Setup the ButtonController (actually is known as a Sword Canvas Controller)
            ButtonController _ButtonController = FindFirstObjectByType<ButtonController>();
            
            UserInterfaceManager.Instance.ButtonController = _ButtonController;
            UserInterfaceManager.Instance.PauseMenu = this.m_PauseMenu;
            UserInterfaceManager.Instance.GuardMeterCanvas = Instantiate(
                this.m_GameSettings.guardCanvasPrefab, 
                transform.position,
                Quaternion.identity);

            GameManager.instance.InputManager.ConfigureMenuInputControl();
            GameManager.instance.PauseManager.PauseMediator.SetPauseMenuController(this.m_PauseMenu);
            
            this.m_NextHandler?.Handle();
        }

        #endregion Methods
  
    }

}