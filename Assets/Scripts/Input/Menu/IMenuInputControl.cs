using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Input.Menu
{
    
    public interface IMenuInputControl: IInputControl
    {

        #region - - - - - - Methods - - - - - -

        void UnPause(InputAction.CallbackContext context);

        #endregion Methods

    }
    
}