using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Camera.Debugging
{

    public class DebugCameraStateGUIController : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        public GameObject CameraControl;
        
        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public ICameraController CameraController
            => CameraControl.GetComponent<ICameraController>();

        #endregion Properties

        #region - - - - - - Methods - - - - - -

        public void SwitchToFollowCamera()
        {
            if (this.CameraControl == null) return;
            
            this.CameraController.SelectCamera(SceneCameras.FollowPlayer);
        }

        public void SwitchToSprintCamera()
        {
            if (this.CameraControl == null) return;
            
            this.CameraController.SelectCamera(SceneCameras.FollowSprintPlayer);
        }

        public void SwitchToFreeLookCamera()
        {
            if (this.CameraControl == null) return;
            
            this.CameraController.SelectCamera(SceneCameras.FreeLook);
        }

        public void SwitchToLockOnCamera()
        {
            if (this.CameraControl == null) return;
            
            this.CameraController.SelectCamera(SceneCameras.LockOn);
        }
        
        #endregion Methods
  
    }

}