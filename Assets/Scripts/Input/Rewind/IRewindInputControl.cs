using UnityEngine.InputSystem;


namespace ThatOneSamuraiGame.Scripts.Input.Rewind
{
    
    public interface IRewindInputControl: IInputControl
    {

        #region - - - - - - Event Handlers - - - - - -

        // -----------------------------------------------------
        // Rewind related Events
        // -----------------------------------------------------
        
        void OnEndRewind(InputAction.CallbackContext context);

        void OnScrub(InputAction.CallbackContext context);

        // -----------------------------------------------------
        // Menu related Events
        // -----------------------------------------------------
        
        void OnPause(InputAction.CallbackContext context);
        
        // -----------------------------------------------------
        // View Orientation related Events
        // -----------------------------------------------------

        void OnRotateCamera(InputAction.CallbackContext context);

        #endregion Event Handlerss

    }
    
}