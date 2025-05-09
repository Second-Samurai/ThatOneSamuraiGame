using ThatOneSamuraiGame.Scripts.DebugScripts;
using ThatOneSamuraiGame.Scripts.DebugScripts.DebugSceneInvokers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Initializers
{

    public class DebugPlayerInitializer : DebugComponent, IDebugStartupHandler
    {

        #region - - - - - - Fields - - - - - -

        public CameraController CameraController;
        public LockOnObserver LockOnObserver;

        #endregion Fields
  
        #region - - - - - - Methods - - - - - -

        public void Handle()
        {
            GameSettings _GameSettings = GameManager.instance.gameSettings;
            GameObject _TargetHolder = Object.Instantiate(
                _GameSettings.targetHolderPrefab,
                _GameSettings.targetHolderPrefab.transform.position,
                Quaternion.identity);
            PlayerInitializerCommand _InitializerCommand = 
                new PlayerInitializerCommand(
                    this.gameObject, 
                    _TargetHolder,
                    CameraController,
                    LockOnObserver);
            
            _InitializerCommand.Execute();
        }
        
        #endregion Methods

    }

}