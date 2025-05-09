using ThatOneSamuraiGame.Scripts.General.Services;
using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Input.Gameplay
{
    
    public interface IGameplayInputControl: IInputControl
    {
        
        #region - - - - - - Event Handlers - - - - - -

        // -----------------------------------------------------
        // Movement related methods
        // -----------------------------------------------------
        
        void OnMovement(InputAction.CallbackContext context);

        void OnSprint(InputAction.CallbackContext context);
        
        // -----------------------------------------------------
        // View Orientation related methods
        // -----------------------------------------------------

        void OnRotateCamera(InputAction.CallbackContext context);
        
        // -----------------------------------------------------
        // Target locking related Events
        // -----------------------------------------------------

        void OnLockOn(InputAction.CallbackContext context);

        void OnToggleLockLeft(InputAction.CallbackContext context);

        void OnToggleLockRight(InputAction.CallbackContext context);
        
        // -----------------------------------------------------
        // Weapon / Attack related Events
        // -----------------------------------------------------

        void OnAttack(InputAction.CallbackContext context);

        void OnSwordDraw(InputAction.CallbackContext context);

        void OnStartHeavy(InputAction.CallbackContext context);

        void OnStartHeavyAlternative(InputAction.CallbackContext context);

        void OnStartBlock(InputAction.CallbackContext context);

        void OnEndBlock(InputAction.CallbackContext context);
        
        // -----------------------------------------------------
        // Special-Action related Events
        // -----------------------------------------------------

        void OnDodge(InputAction.CallbackContext context);

        void OnInitRewind(InputAction.CallbackContext context);        
        
        // -----------------------------------------------------
        // Menu related Events
        // -----------------------------------------------------

        void OnPause(InputAction.CallbackContext context);

        // -----------------------------------------------------
        // Debug related Events
        // -----------------------------------------------------

        void OnDebug(InputAction.CallbackContext context);

        void OnSubmitDebugCommand(InputAction.CallbackContext context);

        #endregion Event Handlers

        #region - - - - - - Methods - - - - - -

        void SetInitialiseGameplayInput(ICommand initializerCommand);

        void SetInputControlData(GameplayInputControlData inputControlData);

        #endregion Methods

    }
    
}