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
    /// <remarks>Provides input transformation and pass-through.</remarks>
    public class GameplayMouseAndKeyboardInputControl : MonoBehaviour
    {
        
        #region - - - - - - Fields - - - - - -

        private IPauseActionHandler m_PauseAction;
        private IPlayerAttackHandler m_PlayerAttackHandler;
        private IPlayerMovement m_PlayerMovement;
        private IPlayerTargetTracking m_PlayerTargetTracking;
        private IPlayerSpecialAction m_PlayerSpecialAction;
        
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
            this.m_PauseAction = gameState.PauseMenu.GetComponent<IPauseActionHandler>();

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
            this.m_PlayerMovement.PreparePlayerMovement(inputValue.Get<Vector2>());
        }

        private void OnSprint(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerMovement.Sprint(inputValue.isPressed);
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
            this.m_PlayerAttackHandler.Attack();
        }

        private void OnSwordDraw(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerAttackHandler.DrawSword();
        }

        private void OnStartHeavy(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerAttackHandler.StartHeavy();
        }

        private void OnStartHeavyAlternative(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerAttackHandler.StartHeavyAlternative();
        }

        private void OnStartBlock(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerAttackHandler.StartBlock();
        }

        private void OnEndBlock(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerAttackHandler.EndBlock();
        }
        
        // -----------------------------------------------------
        // Special-Action related Events
        // -----------------------------------------------------

        private void OnDodge(InputValue inputValue)
        {
            if (!this.m_IsInputActive || this.m_IsInputPaused) return;
            this.m_PlayerSpecialAction.Dodge();
        }
        
        // -----------------------------------------------------
        // Menu related Events
        // -----------------------------------------------------

        private void OnPause(InputValue inputValue)
        {
            if (!this.m_IsInputActive) return;
            this.m_PauseAction.TogglePause();
        }
        
        #endregion Event Handlers

        #region - - - - - - Methods - - - - - -

        public void Pause()// Might not be needed, delete later
            => this.m_IsInputActive = true;

        public void OnUnpaused() // might not be needed, delete later
            => this.m_IsInputActive = false;

        #endregion Methods

    }
    
}
