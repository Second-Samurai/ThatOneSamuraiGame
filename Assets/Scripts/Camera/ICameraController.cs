using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

public interface ICameraController
{

    #region - - - - - - Methods - - - - - -

    GameObject GetCamera(SceneCameras targetCamera);

    Vector3 GetCameraEulerAngles();

    void SelectCamera(SceneCameras selectedCamera);

    void SetCameraAction(ICameraAction cameraAction);

    #endregion Methods

}
