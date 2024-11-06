
using ThatOneSamuraiGame.Scripts.DebugScripts.DebugSceneInvokers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Initializers
{

    public class DebugPlayerInitializer : MonoBehaviour, IDebugStartupHandler
    {

        #region - - - - - - Fields - - - - - -

        public ThirdPersonCamController ThirdPersonCamController;

        #endregion Fields
  
        #region - - - - - - Methods - - - - - -

        public void Handle()
        {
            if (this.ThirdPersonCamController == null)
                Debug.LogError($"[ERROR]: No {nameof(ThirdPersonCamController)} found please set value.");
            
            PlayerInitializerCommand _InitializerCommand = 
                new PlayerInitializerCommand(this.gameObject, ThirdPersonCamController);
            _InitializerCommand.Execute();
        }
        
        #endregion Methods

    }

}