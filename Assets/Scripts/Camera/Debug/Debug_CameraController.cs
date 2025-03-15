using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame
{

    public class Debug_CameraController : IDebugCommandRegistrater
    {

        #region - - - - - - Methods - - - - - -

        public void RegisterCommand(IDebugCommandSystem debugCommandSystem)
        {
            DebugCommand _SwitchToLockOn = new DebugCommand(
                "camera_switchtolockon",
                "Switch camera to lock on",
                "camera_switchtolockon",
                this.SwitchToLockOnCamera);
            DebugCommand _SwitchToFollow = new DebugCommand(
                "camera_switchtofollow",
                "Switch camera to follow",
                "camera_switchtofollow",
                this.SwitchToFollowCamera);
            
            debugCommandSystem.RegisterCommand(_SwitchToFollow);
            debugCommandSystem.RegisterCommand(_SwitchToLockOn);
        }

        // Uses player lock on targeting as the switching the camera will require a target.
        private void SwitchToLockOnCamera()
        {
            if (!Object.FindAnyObjectByType<PlayerTargetLocking>())
            {
                Debug.LogError($"[DEBUG_CONSOLE]: Cannot find {nameof(PlayerTargetLocking)} in scene.");
                return;
            }

            IWeaponSystem _PlayerWeaponSystem =
                Object.FindFirstObjectByType<PlayerWeaponSystem>(FindObjectsInactive.Exclude);
            if (!_PlayerWeaponSystem.IsWeaponDrawn)
            {
                Debug.Log("[DEBUG_CONSOLE]: Cannot LockOn if no sword is equipped");
                return;
            }
            
            IPlayerTargetTracking _PlayerTargetLocking =
                Object.FindFirstObjectByType<PlayerTargetLocking>(FindObjectsInactive.Exclude);
            _PlayerTargetLocking.ToggleTargetLocking();
        }

        private void SwitchToFollowCamera()
        {
            if (!Object.FindAnyObjectByType<CameraController>())
            {
                Debug.LogError($"Cannot find {nameof(CameraController)} in scene.");
                return;
            }

            ICameraController _CameraController =
                Object.FindFirstObjectByType<CameraController>(FindObjectsInactive.Exclude);
            _CameraController.SelectCamera(SceneCameras.FollowPlayer);
        }

        #endregion Methods
  
    }

}