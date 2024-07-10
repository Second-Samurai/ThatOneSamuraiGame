using System;
using ThatOneSamuraiGame.Scripts.Input.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Input
{
    
    /// <summary>
    /// Manages the input possession between scenes.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        
        #region - - - - - - - Fields - - - - - - -

        private IInputControl m_GameplayInputControl;
        private IInputControl m_MenuInputControl;

        [SerializeField]
        private PlayerInput m_PlayerInput;

        private ControlDevices m_SelectedDevice;
        private InputActionMapType m_ActiveInputActionMap;
        
        #endregion Fields

        #region - - - - - - - Lifecycle Methods - - - - - - -

        private void Start()
        {
            this.m_PlayerInput ??= this.GetComponent<PlayerInput>();
            
            // Temp: Set the default control device to be keyboard and mouse
            this.m_SelectedDevice = ControlDevices.KeyboardAndMouse;
        }

        #endregion Lifecycle Methods

        #region - - - - - - Methods - - - - - -

        /// <summary>
        /// Activates the menu controls for the game.
        /// </summary>
        /// <remarks>This input control is the first to activate in the game.</remarks>
        public void ActivateMenuInputControl()
        {
            
            this.m_ActiveInputActionMap = InputActionMapType.Menu;
        }

        public void PossesPlayerObject(GameObject playerObject)
        {
            IGameplayInputControl _GameplayInputControl;
            
            switch (this.m_SelectedDevice)
            {
                case ControlDevices.KeyboardAndMouse:
                    _GameplayInputControl = playerObject.AddComponent<GameplayMouseAndKeyboardInputControl>();
                    break;
                case ControlDevices.Gamepad:
                    throw new NotImplementedException();
                default:
                    _GameplayInputControl = playerObject.AddComponent<GameplayMouseAndKeyboardInputControl>();
                    break;
            };
            
            _GameplayInputControl.ConfigureInputEvents(this.m_PlayerInput);
        }

        /// <summary>
        /// Removes subscribed input events from the PlayerObject.
        /// </summary>
        public void UnpossesPlayerObject(GameObject playerObject)
        {
            IGameplayInputControl _GameplayInputControl = playerObject.GetComponent<IGameplayInputControl>();
            
            this.m_PlayerInput.actions["movement"].performed -= _GameplayInputControl.OnMovement;
            this.m_PlayerInput.actions["sprint"].performed -= _GameplayInputControl.OnSprint;
        }

        public void SwitchToGameplayControls()
        {
            this.m_PlayerInput.SwitchCurrentActionMap("Gameplay");
            this.m_GameplayInputControl.EnableInput();
            // this.m_MenuInputControl.DisableInput();
        }

        public void SwitchToMenuControls()
        {
            this.m_PlayerInput.SwitchCurrentActionMap("Menu");
            this.m_GameplayInputControl.DisableInput();
            // this.m_MenuInputControl.EnableInput();
        }

        #endregion Methods

    }
    
    public enum ControlDevices 
    {
        KeyboardAndMouse,
        Gamepad
    }

    public enum InputActionMapType
    {
        Gamplay,
        Menu
    }
    
}
