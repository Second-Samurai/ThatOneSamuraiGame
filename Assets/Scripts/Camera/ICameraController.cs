using System;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Camera
{
    
    // Note: This is not the correct behavior, this abstracts direct access of the player from the camera. 
    //       This is so that the developer is prevented from modifying or invoking logic nested within the camera's scripts.
    //       - The temporary solution will be acceptable for now.
    //
    // Consideration:
    //       - Create a proxy within the player that will manage camera behaviours on the Camera's behalf
    [Obsolete]
    public interface ICameraController
    {

        #region - - - - - - Properties - - - - - -

        bool IsLockedOn { get; }

        #endregion Properties
        
        #region - - - - - - Methods - - - - - -

        bool LockOn();

        void ToggleLockOn();
        
        void ToggleSprintCameraState(bool isSprinting);

        void ResetCameraRoll();

        void RollCamera();

        void RotateCamera(Vector2 rotationVector);

        #endregion Methods

    }
    
}
