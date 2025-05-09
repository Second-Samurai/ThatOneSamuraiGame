using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.DebugScripts;
using ThatOneSamuraiGame.Scripts.DebugScripts.DebugSceneInvokers;
using UnityEngine;

public class DebugCameraControlInvoker : DebugComponent, IDebugStartupHandler
{

    #region - - - - - - Methods - - - - - -

    public void Handle()
    {
        ICameraController _CameraController = this.GetComponent<ICameraController>();
        _CameraController.SelectCamera(SceneCameras.FollowPlayer);
    }

    #endregion Methods
  
}
