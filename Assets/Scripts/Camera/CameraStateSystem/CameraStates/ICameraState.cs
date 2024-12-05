using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;

public interface ICameraState
{

    #region - - - - - - Methods - - - - - -

    void InitializeState(CameraStateContext context);

    void StartState();

    void EndState();

    bool ValidateState();

    SceneCameras GetSceneState();

    #endregion Methods

}
