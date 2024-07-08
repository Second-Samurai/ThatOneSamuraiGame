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

        private void PossesPlayerObject(GameObject playerObject)
        {
            switch (this.m_SelectedDevice)
            {
                case ControlDevices.KeyboardAndMouse:
                    playerObject.AddComponent<GameplayMouseAndKeyboardInputControl>();
                    break;
                case ControlDevices.Gamepad:
                    throw new NotImplementedException();
                default:
                    playerObject.AddComponent<GameplayMouseAndKeyboardInputControl>();
                    break;
            };
        }

        private void SwitchToGameplayControls()
        {
            this.m_PlayerInput.SwitchCurrentActionMap("Gameplay");
            this.m_GameplayInputControl.EnableInput();
            this.m_MenuInputControl.DisableInput();
        }

        private void SwitchToMenuControls()
        {
            this.m_PlayerInput.SwitchCurrentActionMap("Menu");
            this.m_GameplayInputControl.DisableInput();
            this.m_MenuInputControl.EnableInput();
        }

        #endregion Methods

    }
    
    public enum ControlDevices {
        KeyboardAndMouse,
        Gamepad
    }
    
}
