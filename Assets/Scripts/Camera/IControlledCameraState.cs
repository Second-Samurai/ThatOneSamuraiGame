using System;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Camera
{
    
    [Obsolete]
    public interface IControlledCameraState
    {

        #region - - - - - - Properties - - - - - -

        Vector3 CurrentEulerAngles { get; }
        
        Vector3 ForwardDirection { get; }
        
        bool IsCameraViewTargetLocked { get; }

        #endregion Properties
        
    }
    
}