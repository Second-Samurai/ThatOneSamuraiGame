using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Camera
{
    
    public interface IControlledCameraState
    {

        #region - - - - - - Properties - - - - - -

        Vector3 CurrentEulerAngles { get; }
        
        bool IsCameraViewTargetLocked { get; }

        #endregion Properties
        
    }
    
}