
using ThatOneSamuraiGame.Scripts.DebugScripts.DebugSceneInvokers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Initializers
{

    public class DebugPlayerInitializer : MonoBehaviour, IDebugStartupHandler
    {

        #region - - - - - - Methods - - - - - -

        public void Handle()
        {
            PlayerInitializerCommand _InitializerCommand = new PlayerInitializerCommand(this.gameObject);
            _InitializerCommand.Execute();
        }
        
        #endregion Methods

    }

}