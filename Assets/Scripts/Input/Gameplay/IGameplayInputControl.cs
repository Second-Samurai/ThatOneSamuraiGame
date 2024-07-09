using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Input.Gameplay
{
    
    public interface IGameplayInputControl: IInputControl
    {

        #region - - - - - - Methods - - - - - -

        //--------------------------------------
        // Movement related methods
        //--------------------------------------
        
        void OnMovement(InputAction.CallbackContext callback);

        void OnSprint(InputAction.CallbackContext callback);

        #endregion Methods

    }
    
}