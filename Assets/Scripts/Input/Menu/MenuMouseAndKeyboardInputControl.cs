using System;
using ThatOneSamuraiGame.Scripts.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Input.Menu
{

    /// <summary>
    /// Handles menu inputs for keyboard and mouse.
    /// </summary>
    public class MenuMouseAndKeyboardInputControl: TOSGMonoBehaviourBase, IMenuInputControl
    {
        
        #region - - - - - - Fields - - - - - -

        private bool m_IsInputActive;

        #endregion Fields
        
        #region - - - - - - Event Handlers - - - - - -

        void IMenuInputControl.UnPause(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive) return;
            
            throw new NotImplementedException();
        }

        #endregion Event Handlers

        #region - - - - - - Methods - - - - - -

        void IInputControl.ConfigureInputEvents(PlayerInput playerInput)
        {
            Debug.LogWarning("No event handlers have been configured for 'Menu' keyboard and mouse.");
        }
        
        void IInputControl.DisableInput() 
            => this.m_IsInputActive = false;

        void IInputControl.EnableInput() 
            => this.m_IsInputActive = true;

        #endregion Methods
        
    }

}