using ThatOneSamuraiGame.Legacy;
using ThatOneSamuraiGame.Scripts.DebugScripts;
using ThatOneSamuraiGame.Scripts.DebugScripts.DebugSceneInvokers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Initializers
{

    // TODO: Remove legacy camera functionality
    public class DebugPlayerInitializer : DebugComponent, IDebugStartupHandler
    {

        #region - - - - - - Fields - - - - - -

        // public PlayerCamTargetController PlayerCameraTargetController;
        // public ThirdPersonCamController ThirdPersonCamController;

        #endregion Fields
  
        #region - - - - - - Methods - - - - - -

        public void Handle()
        {
            GameSettings _GameSettings = GameManager.instance.gameSettings;
            //
            // if (this.ThirdPersonCamController == null)
            //     Debug.LogError($"[ERROR]: No {nameof(ThirdPersonCamController)} found please set value.");

            GameObject _TargetHolder = Object.Instantiate(
                _GameSettings.targetHolderPrefab,
                _GameSettings.targetHolderPrefab.transform.position,
                Quaternion.identity);
            PlayerInitializerCommand _InitializerCommand = 
                new PlayerInitializerCommand(
                    this.gameObject, 
                    // this.PlayerCameraTargetController,
                    // ThirdPersonCamController, 
                    _TargetHolder);
            _InitializerCommand.Execute();
        }
        
        #endregion Methods

    }

}