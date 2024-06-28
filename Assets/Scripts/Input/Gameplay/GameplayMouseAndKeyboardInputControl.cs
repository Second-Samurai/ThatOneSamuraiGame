using System;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using ThatOneSamuraiGame.Scripts.UI.Pause;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Input.Gameplay
{
    
    /// <summary>
    /// Handles inputs for Gameplay actions for Mouse and Keyboard.
    /// </summary>
    public class GameplayMouseAndKeyboardInputControl : MonoBehaviour
    {
        
        #region - - - - - - Fields - - - - - -

        private IPlayerAttackHandler m_PlayerAttackHandler;
        private IPlayerMovement m_PlayerMovement;
        private IPlayerTargetTracking m_PlayerTargetTracking;
        private IPlayerSpecialAction m_PlayerSpecialAction;
        private IPausable m_PauseAction;

        private bool m_IsInputActive = false;
        private bool m_IsInputPaused = false;
        
        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            GameState gameState = GameManager.instance.GameState;
            
            this.m_PlayerMovement = gameState.ActivePlayer.GetComponent<IPlayerMovement>();
            this.m_PlayerTargetTracking = gameState.ActivePlayer.GetComponent<IPlayerTargetTracking>();
            this.m_PlayerAttackHandler = gameState.ActivePlayer.GetComponent<IPlayerAttackHandler>();
            this.m_PlayerSpecialAction = gameState.ActivePlayer.GetComponent<IPlayerSpecialAction>();
            this.m_PauseAction = gameState.PauseScreen.GetComponent<IPausable>();

            this.m_IsInputActive = true;
        }
        
        #endregion Lifecycle Methods
        
        #region - - - - - - Event Handlers - - - - - -

        // -----------------------------------------------------
        // Movement related Events
        // -----------------------------------------------------
        
        private void OnMovement(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerMovement.MovePlayer(inputValue.Get<Vector2>());
        }

        private void OnSprint(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerMovement.SprintPlayer(inputValue.isPressed);
        }
        
        // -----------------------------------------------------
        // Target locking related Events
        // -----------------------------------------------------

        private void OnLockOn(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerTargetTracking.ToggleLockOn();
        }

        private void OnToggleLockLeft(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerTargetTracking.ToggleLockLeft();
        }

        private void OnToggleLockRight(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerTargetTracking.ToggleLockRight();
        }
        
        // -----------------------------------------------------
        // Weapon / Attack related Events
        // -----------------------------------------------------
        
        private void OnAttack(InputValue inputValue) 
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
        }

        private void OnSwordDraw(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
        }

        private void OnStartHeavy(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
        }

        private void OnStartHeavyAlternative(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
        }

        private void OnStartBlock(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
        }

        private void OnEndBlock(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
        }
        
        // -----------------------------------------------------
        // Special-Action related Events
        // -----------------------------------------------------

        private void OnDodge(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
        }
        
        // -----------------------------------------------------
        // Menu related Events
        // -----------------------------------------------------

        private void OnPause(InputValue inputValue)
        {
            throw new NotImplementedException();
        }
        
        #endregion Event Handlers

        #region - - - - - - Methods - - - - - -

        public void Pause()
            => this.m_IsInputActive = true;

        public void OnUnpaused()
            => this.m_IsInputActive = false;

        #endregion Methods

    }
    
}
