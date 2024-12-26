using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.General.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace ThatOneSamuraiGame.Scripts.Input.Gameplay
{

    /// <summary>
    /// Handles inputs for Gameplay actions for Mouse and Keyboard.
    /// </summary>
    /// <remarks>Provides input transformation and pass-through.</remarks>
    public class GameplayMouseAndKeyboardInputControl : PausableMonoBehaviour, IGameplayInputControl
    {

        #region - - - - - - Fields - - - - - -

        private GameplayInputControlData m_InputControlData;
        private ICommand m_InitializerCommand;
        private bool m_IsInputActive;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_InitializerCommand.Execute();
            this.m_IsInputActive = true;
        }
        
        #endregion Lifecycle Methods

        #region - - - - - - Event Handlers - - - - - -

        // -----------------------------------------------------
        // Movement related Events
        // -----------------------------------------------------

        void IGameplayInputControl.OnMovement(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerMovement.PreparePlayerMovement(context.ReadValue<Vector2>());
        }

        void IGameplayInputControl.OnSprint(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerMovement.PrepareSprint(context.ReadValueAsButton());
        }

        // -----------------------------------------------------
        // Target locking related Events
        // -----------------------------------------------------

        void IGameplayInputControl.OnLockOn(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerTargetTracking.ToggleLockOn();
        }

        void IGameplayInputControl.OnToggleLockLeft(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerTargetTracking.ToggleLockLeft();
        }

        void IGameplayInputControl.OnToggleLockRight(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerTargetTracking.ToggleLockRight();
        }

        // -----------------------------------------------------
        // Weapon / Attack related Events
        // -----------------------------------------------------

        void IGameplayInputControl.OnAttack(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerAttackHandler.Attack();
        }

        void IGameplayInputControl.OnSwordDraw(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerAttackHandler.DrawSword();
        }

        void IGameplayInputControl.OnStartHeavy(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;

            if (context.interaction is HoldInteraction)
                this.m_InputControlData.PlayerAttackHandler.StartHeavy();
        }

        void IGameplayInputControl.OnStartHeavyAlternative(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerAttackHandler.StartHeavyAlternative();
        }

        void IGameplayInputControl.OnStartBlock(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerAttackHandler.StartBlock();
        }

        void IGameplayInputControl.OnEndBlock(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerAttackHandler.EndBlock();
        }

        // -----------------------------------------------------
        // Special-Action related Events
        // -----------------------------------------------------

        void IGameplayInputControl.OnDodge(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerMovement.Dodge();
        }

        void IGameplayInputControl.OnInitRewind(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            // this.m_InputControlData.PlayerSpecialAction.ActivateRewind();
            Debug.Log("Rewind has been removed.");
        }

        // -----------------------------------------------------
        // Menu related Events
        // -----------------------------------------------------

        void IGameplayInputControl.OnPause(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive) return;
            
            // Ticket #46 - Clarify handling on UI events against game logic.
            this.m_InputControlData.PauseActionHandler.TogglePause();
        }

        // -----------------------------------------------------
        // View Orientation related Events
        // -----------------------------------------------------

        void IGameplayInputControl.OnRotateCamera(InputAction.CallbackContext context)
        {
            if (!this.m_IsInputActive || this.IsPaused) return;
            this.m_InputControlData.PlayerViewOrientationHandler.RotateViewOrientation(context.ReadValue<Vector2>());
        }

        #endregion Event Handlers

        #region - - - - - - Methods - - - - - -

        void IInputControl.ConfigureInputEvents(PlayerInput playerInput)
        {
            // Tech-Debt: #57 - Use constants to represent the different input actions.

            // Movement
            playerInput.actions["movement"].performed += ((IGameplayInputControl)this).OnMovement;
            playerInput.actions["movement"].canceled += ((IGameplayInputControl)this).OnMovement;
            playerInput.actions["sprint"].performed += ((IGameplayInputControl)this).OnSprint;
            playerInput.actions["sprint"].canceled += ((IGameplayInputControl)this).OnSprint;

            // Target Locking
            playerInput.actions["lockon"].performed += ((IGameplayInputControl)this).OnLockOn;
            playerInput.actions["togglelockleft"].performed += ((IGameplayInputControl)this).OnToggleLockLeft;
            playerInput.actions["togglelockright"].performed += ((IGameplayInputControl)this).OnToggleLockRight;

            // Attack
            playerInput.actions["attack"].performed += ((IGameplayInputControl)this).OnAttack;
            playerInput.actions["sworddraw"].performed += ((IGameplayInputControl)this).OnSwordDraw;
            playerInput.actions["startheavy"].performed += ((IGameplayInputControl)this).OnStartHeavy;
            playerInput.actions["startheavyalternative"].performed +=
                ((IGameplayInputControl)this).OnStartHeavyAlternative;
            playerInput.actions["startblock"].performed += ((IGameplayInputControl)this).OnStartBlock;
            playerInput.actions["endblock"].performed += ((IGameplayInputControl)this).OnEndBlock;

            // Special Action
            playerInput.actions["dodge"].performed += ((IGameplayInputControl)this).OnDodge;
            playerInput.actions["initrewind"].performed += ((IGameplayInputControl)this).OnInitRewind;

            // Menu Action
            playerInput.actions["pause"].performed += ((IGameplayInputControl)this).OnPause;

            // View Orientation
            playerInput.actions["rotatecamera"].performed += ((IGameplayInputControl)this).OnRotateCamera;
            playerInput.actions["rotatecamera"].canceled += ((IGameplayInputControl)this).OnRotateCamera;
        }

        void IInputControl.EnableInput() 
            => this.m_IsInputActive = true;

        void IInputControl.DisableInput() 
            => this.m_IsInputActive = false;

        void IGameplayInputControl.SetInitialiseGameplayInput(ICommand initializerCommand) 
            => this.m_InitializerCommand = initializerCommand;

        void IGameplayInputControl.SetInputControlData(GameplayInputControlData inputControlData)
            => this.m_InputControlData = inputControlData;

        #endregion Methods

    }

}
