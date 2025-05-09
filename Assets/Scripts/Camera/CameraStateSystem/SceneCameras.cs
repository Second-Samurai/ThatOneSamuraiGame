using ThatOneSamuraiGame.Scripts.Enumeration;

namespace ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem
{

    public class SceneCameras : SmartEnum
    {

        #region - - - - - - Fields - - - - - -

        public static SceneCameras FollowPlayer = new SceneCameras("FollowPlayerCamera", 15);
        public static SceneCameras FollowSprintPlayer = new SceneCameras("FollowSprintPlayerCamera", 10);
        public static SceneCameras FreeLook = new SceneCameras("FreeLookCamera", 3);
        public static SceneCameras LockOn = new SceneCameras("LockOnCamera", 4);

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        private SceneCameras(string name, int value) : base(name, value) { }

        #endregion Constructors
  
        #region - - - - - - Methods - - - - - -

        public static implicit operator int(SceneCameras gameScenes)
            => gameScenes.GetValue();

        public static implicit operator string(SceneCameras gameScenes)
            => gameScenes.ToString();

        #endregion Methods
  
    }

    // This exists as a quick workaround to creating a custom property drawer for the smart enum
    public enum SceneCameraEnum
    {
        FollowPlayer,
        FollowSprintPlayer,
        FreeLook,
        LockOn
    }

}