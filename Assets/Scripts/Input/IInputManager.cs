using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Input
{
    
    public interface IInputManager
    {
        
        #region - - - - - - Methods - - - - - -

        void ConfigureMenuInputControl();

        bool DoesGameplayInputControlExist();

        void DisableActiveInputControl();

        void EnableActiveInputControl();
        
        // -------------------------------------
        // Player object possession methods
        // -------------------------------------
        
        void PossesPlayerObject(GameObject playerObject);

        void UnpossesPlayerObject(GameObject playerObject);
        
        // -------------------------------------
        // InputControl state management
        // -------------------------------------

        void SwitchToGameplayControls();

        void SwitchToMenuControls();

        void SwitchToRewindControls();
        
        #endregion Methods

    }
    
}