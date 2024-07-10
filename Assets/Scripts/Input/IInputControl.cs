using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Input
{
    
    public interface IInputControl
    {

        #region - - - - - - Methods - - - - - -
        
        void ConfigureInputEvents(PlayerInput playerInput);
        
        void DisableInput();

        void EnableInput();

        #endregion Methods

    }
    
}