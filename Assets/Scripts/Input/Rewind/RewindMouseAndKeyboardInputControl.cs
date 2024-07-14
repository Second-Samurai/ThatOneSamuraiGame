using System;
using ThatOneSamuraiGame.Scripts.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Input.Rewind
{

    public class RewindMouseAndKeyboardInputControl : TOSGMonoBehaviourBase, IRewindInputControl
    {

        #region - - - - - - Fields - - - - - -

        private RewindManager m_RewindManager;
        
        private bool m_IsInputActive;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_RewindManager = this.GetComponent<RewindManager>();
            
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Event Handlers - - - - - -

        void IRewindInputControl.OnScrub(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive) return;
            
            throw new NotImplementedException();
        }

        void IRewindInputControl.OnPause(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive) return;
            
            throw new NotImplementedException();
        }

        void IRewindInputControl.OnEndRewinf(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive) return;
            
            throw new NotImplementedException();
        }

        void IRewindInputControl.OnRotateCamera(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive) return;
            
            throw new NotImplementedException();
        }

        #endregion Event Handlers

        #region - - - - - - Methods - - - - - -

        void IInputControl.ConfigureInputEvents(PlayerInput playerInput)
        {
            Debug.LogWarning("No event handlers have been configured for 'Rewind' keyboard and mouse.");
        }

        void IInputControl.DisableInput() 
            => this.m_IsInputActive = false;

        void IInputControl.EnableInput() 
            => this.m_IsInputActive = false;

        #endregion Methods
    }

}