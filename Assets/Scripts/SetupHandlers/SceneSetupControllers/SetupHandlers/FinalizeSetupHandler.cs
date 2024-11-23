using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public class FinalizeSetupHandler : MonoBehaviour, ISetupHandler
    {

        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler) { }

        void ISetupHandler.Handle()
        {
            // Validate management dependencies
            SceneManager.Instance.IsMembersValid();
            UserInterfaceManager.Instance.IsMembersValid();
            InputManager.Instance.IsMembersValid();
        }

        #endregion Methods
  
    }

}