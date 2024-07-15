using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.ViewOrientation;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Input.Rewind
{

    public class RewindMouseAndKeyboardInputControl : TOSGMonoBehaviourBase, IRewindInputControl
    {

        #region - - - - - - Fields - - - - - -

        private IPlayerViewOrientationHandler m_PlayerViewOrientationHandler;
        private RewindInput m_RewindInput; // Ticket: # - Refactor methods for the player implementation.
        private RewindManager m_RewindManager;
        
        private bool m_IsInputActive;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PlayerViewOrientationHandler = this.GetComponent<IPlayerViewOrientationHandler>();
            this.m_RewindInput = this.GetComponent<RewindInput>();
            this.m_RewindManager = GameManager.instance.rewindManager;
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Event Handlers - - - - - -
        
        // -----------------------------------------------------
        // Rewind related Events
        // -----------------------------------------------------

        void IRewindInputControl.OnEndRewind(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_RewindInput.OnEndRewind(); // Temporarily act as a pass-through.
        }

        void IRewindInputControl.OnScrub(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_RewindManager.rewindDirection = context.ReadValue<float>();
        }

        // -----------------------------------------------------
        // Menu related Events
        // -----------------------------------------------------
        
        void IRewindInputControl.OnPause(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            
            // Ticket #46 - Clarify handling on UI events against game logic.
            throw new NotImplementedException();
        }
        
        // -----------------------------------------------------
        // View Orientation related Events
        // -----------------------------------------------------

        void IRewindInputControl.OnRotateCamera(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_PlayerViewOrientationHandler.RotateViewOrientation(context.ReadValue<Vector2>());
        }

        #endregion Event Handlers

        #region - - - - - - Methods - - - - - -

        void IInputControl.ConfigureInputEvents(PlayerInput playerInput)
        {
            // Subscribe all events from input control
            // Methods should be subscribed as the first in each event list
            
            // Rewind
            playerInput.actions["endrewind"].performed += ((IRewindInputControl)this).OnEndRewind;
            playerInput.actions["scrub"].performed += ((IRewindInputControl)this).OnScrub;
            playerInput.actions["scrub"].canceled += ((IRewindInputControl)this).OnScrub;
            
            // Menu Action
            playerInput.actions["pause"].performed += ((IRewindInputControl)this).OnPause;
            
            // View Orientation
            playerInput.actions["rotatecamera"].performed += ((IRewindInputControl)this).OnRotateCamera;
            playerInput.actions["rotatecamera"].canceled += ((IRewindInputControl)this).OnRotateCamera;
        }

        void IInputControl.DisableInput() 
            => this.m_IsInputActive = false;

        void IInputControl.EnableInput() 
            => this.m_IsInputActive = true;

        #endregion Methods
        
    }

}