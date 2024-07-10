using UnityEngine.InputSystem;


namespace ThatOneSamuraiGame.Scripts.Input.Rewind
{
    
    public interface IRewindInputControl: IInputControl
    {

        #region - - - - - - Methods - - - - - -

        void OnScrub(InputAction.CallbackContext context);

        void OnPause(InputAction.CallbackContext context);

        void OnEndRewinf(InputAction.CallbackContext context);

        void OnRotateCamera(InputAction.CallbackContext context);

        #endregion Methods

    }
    
}