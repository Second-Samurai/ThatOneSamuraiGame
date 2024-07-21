using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseActionHandler;
using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Input.Menu
{

    /// <summary>
    /// Handles menu inputs for keyboard and mouse.
    /// </summary>
    public class MenuMouseAndKeyboardInputControl : TOSGMonoBehaviourBase, IMenuInputControl
    {

        #region - - - - - - Fields - - - - - -

        // Menu behavior handlers
        private IPauseActionHandler m_PauseActionHandler;

        // Local Fields
        private bool m_IsInputActive;

        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            var _GameState = GameManager.instance.GameState;
            this.m_PauseActionHandler = _GameState.SessionUser.GetComponent<IPauseActionHandler>();
        }

        #endregion Lifecycle Methods

        #region - - - - - - Event Handlers - - - - - -

        // Tech Debt: #54 - Change name of UnPause to toggle pause.
        //  - The input control should not care whether the UI is paused or not. 
        //  - The input control should only be concerned about the interaction to the menu.
        void IMenuInputControl.UnPause(InputAction.CallbackContext context)
        {
            if (!m_IsInputActive) return;

            // Ticket #46 - Clarify handling on UI events against game logic.
            this.m_PauseActionHandler.TogglePause();
        }

        #endregion Event Handlers

        #region - - - - - - Methods - - - - - -

        void IInputControl.ConfigureInputEvents(PlayerInput playerInput) 
            => playerInput.actions["unpause"].performed += ((IMenuInputControl)this).UnPause;

        void IInputControl.DisableInput() 
            => this.m_IsInputActive = false;

        void IInputControl.EnableInput() 
            => this.m_IsInputActive = true;

        #endregion Methods
        
    }

}
