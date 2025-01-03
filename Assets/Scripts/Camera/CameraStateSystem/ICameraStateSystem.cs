using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;

public interface ICameraStateSystem
{

    #region - - - - - - Methods - - - - - -

    void Initialize();
    
    void SetState(SceneCameras selectedCamera);

    List<ICameraState> GetCameraStates();

    #endregion Methods

}