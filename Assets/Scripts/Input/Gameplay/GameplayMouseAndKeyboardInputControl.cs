using System;
using ThatOneSamuraiGame.Scripts.Base;
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
    public class GameplayMouseAndKeyboardInputControl : TOSGMonoBehaviourBase, IGameplayInputControl
    {

        #region - - - - - - Fields - - - - - -

        private IPauseActionHandler m_PauseAction;
        private IPlayerAttackHandler m_PlayerAttackHandler;
        private IPlayerMovement m_PlayerMovement;
        private IPlayerTargetTracking m_PlayerTargetTracking;
        private IPlayerSpecialAction m_PlayerSpecialAction;
        
        private bool m_IsInputActive;

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

        void IGameplayInputControl.OnMovement(InputAction.CallbackContext context)
        {
            Debug.Log("Encountering this point");
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerMovement.PreparePlayerMovement(context.ReadValue<Vector2>());
        }

        void IGameplayInputControl.OnSprint(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerMovement.PrepareSprint(context.ReadValueAsButton());
        }

        // -----------------------------------------------------
        // Target locking related Events
        // -----------------------------------------------------
        void IGameplayInputControl.OnLockOn(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerTargetTracking.ToggleLockOn();
        }

        void IGameplayInputControl.OnToggleLockLeft(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerTargetTracking.ToggleLockLeft();
        }

        void IGameplayInputControl.OnToggleLockRight(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerTargetTracking.ToggleLockRight();
        }

        // -----------------------------------------------------
        // Weapon / Attack related Events
        // -----------------------------------------------------

        void IGameplayInputControl.OnAttack(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerAttackHandler.Attack();
        }

        void IGameplayInputControl.OnSwordDraw(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerAttackHandler.DrawSword();
        }

        void IGameplayInputControl.OnStartHeavy(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerAttackHandler.StartHeavy();
        }

        void IGameplayInputControl.OnStartHeavyAlternative(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerAttackHandler.StartHeavyAlternative();
        }

        void IGameplayInputControl.OnStartBlock(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerAttackHandler.StartBlock();
        }

        void IGameplayInputControl.OnEndBlock(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerAttackHandler.EndBlock();
        }

        // -----------------------------------------------------
        // Special-Action related Events
        // -----------------------------------------------------

        void IGameplayInputControl.OnDodge(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.m_PlayerSpecialAction.Dodge();
        }

        // -----------------------------------------------------
        // Menu related Events
        // -----------------------------------------------------

        void IGameplayInputControl.OnPause(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive) return;
            
            // Ticket # - Create revamped pause system
            throw new NotImplementedException();
        }

        #endregion Event Handlers

        #region - - - - - - Methods - - - - - -
        
        void IGameplayInputControl.ConfigureInputEvents(PlayerInput playerInput)
        {
            // Subscribe all events from input control
            // Methods should be subscribed as the first in each event list
            
            // Movement
            playerInput.actions["movement"].performed += ((IGameplayInputControl)this).OnMovement;
            playerInput.actions["movement"].canceled += ((IGameplayInputControl)this).OnMovement;
            playerInput.actions["sprint"].performed += ((IGameplayInputControl)this).OnSprint;
            playerInput.actions["sprint"].canceled += ((IGameplayInputControl)this).OnSprint;
            
            // Target Locking
            playerInput.actions["lockon"].performed += ((IGameplayInputControl)this).OnLockOn;
            playerInput.actions["lockon"].canceled += ((IGameplayInputControl)this).OnLockOn;
            playerInput.actions["togglelockleft"].performed += ((IGameplayInputControl)this).OnToggleLockLeft;
            playerInput.actions["togglelockleft"].canceled += ((IGameplayInputControl)this).OnToggleLockLeft;
            playerInput.actions["togglelockright"].performed += ((IGameplayInputControl)this).OnToggleLockRight;
            playerInput.actions["togglelockright"].canceled += ((IGameplayInputControl)this).OnToggleLockRight;
            
            // Attack
            playerInput.actions["attack"].performed += ((IGameplayInputControl)this).OnAttack;
            playerInput.actions["attack"].canceled += ((IGameplayInputControl)this).OnAttack;
            playerInput.actions["sworddraw"].performed += ((IGameplayInputControl)this).OnSwordDraw;
            playerInput.actions["sworddraw"].canceled += ((IGameplayInputControl)this).OnSwordDraw;
            playerInput.actions["startheavy"].performed += ((IGameplayInputControl)this).OnStartHeavy;
            playerInput.actions["startheavy"].canceled += ((IGameplayInputControl)this).OnStartHeavy;
            playerInput.actions["startheavyalternative"].performed += ((IGameplayInputControl)this).OnStartHeavyAlternative;
            playerInput.actions["startheavyalternative"].canceled += ((IGameplayInputControl)this).OnStartHeavyAlternative;
            playerInput.actions["startblock"].performed += ((IGameplayInputControl)this).OnStartBlock;
            playerInput.actions["startblock"].canceled += ((IGameplayInputControl)this).OnStartBlock;
            playerInput.actions["endblock"].performed += ((IGameplayInputControl)this).OnEndBlock;
            playerInput.actions["endblock"].canceled += ((IGameplayInputControl)this).OnEndBlock;
            
            // Special Action
            playerInput.actions["dodge"].performed += ((IGameplayInputControl)this).OnDodge;
            playerInput.actions["dodge"].canceled += ((IGameplayInputControl)this).OnDodge;
            
            // Menu Action
            playerInput.actions["pause"].performed += ((IGameplayInputControl)this).OnToggleLockLeft;
            playerInput.actions["pause"].canceled += ((IGameplayInputControl)this).OnToggleLockLeft;
        }

        void IInputControl.EnableInput()
            => this.m_IsInputActive = true;

        void IInputControl.DisableInput()
            => this.m_IsInputActive = false;

        #endregion Methods

    }

}
