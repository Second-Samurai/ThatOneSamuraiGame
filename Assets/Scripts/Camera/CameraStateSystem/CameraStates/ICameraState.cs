using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

public interface ICameraState
{

    #region - - - - - - Methods - - - - - -

    void InitializeState(CameraStateContext context);

    GameObject GetCameraObject();

    void StartState();

    void EndState();

    bool ValidateState();

    SceneCameras GetSceneState();

    #endregion Methods

}
