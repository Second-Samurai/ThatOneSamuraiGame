namespace ThatOneSamuraiGame.Scripts.Camera
{
    
    // Note: This is not the correct behavior, this abstracts direct access of the player from the camera. 
    //       This is so that the developer is prevented from modifying or invoking logic nested within the camera's scripts.
    //       - The temporary solution will be acceptable for now.
    //
    // Consideration:
    //       - Create a proxy within the player that will manage camera behaviours on the Camera's behalf
    public interface ICameraController
    {

        #region - - - - - - Methods - - - - - -

        void ToggleSprintCameraState(bool isSprinting);

        #endregion Methods

    }
    
}
