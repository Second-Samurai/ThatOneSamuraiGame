using System;
using ThatOneSamuraiGame.Scripts.General.Services;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using ThatOneSamuraiGame.Scripts.Player.ViewOrientation;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseActionHandler;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Input.Gameplay
{

    public class GameplayInputInitializerCommand : ICommand
    {

        #region - - - - - - Fields - - - - - -

        private readonly GameObject m_ActivePlayer;
        private readonly GameplayInputControlData m_InputControlData;
        private readonly GameObject m_SessionUser;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public GameplayInputInitializerCommand(
            GameplayInputControlData inputControlData, 
            GameObject activePlayer, 
            GameObject sessionUser)
        {
            this.m_InputControlData = inputControlData ?? throw new ArgumentNullException(nameof(inputControlData));
            this.m_ActivePlayer = activePlayer ?? throw new ArgumentNullException(nameof(activePlayer));
            this.m_SessionUser = sessionUser ?? throw new ArgumentNullException(nameof(sessionUser));
        }

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        void ICommand.Execute()
        {
            this.m_InputControlData.PauseActionHandler = this.m_SessionUser.GetComponent<IPauseActionHandler>();
            this.m_InputControlData.PlayerAttackHandler = this.m_ActivePlayer.GetComponent<IPlayerAttackHandler>();
            this.m_InputControlData.PlayerMovement = this.m_ActivePlayer.GetComponent<IPlayerMovement>();
            this.m_InputControlData.PlayerTargetTracking = this.m_ActivePlayer.GetComponent<IPlayerTargetTracking>();
            this.m_InputControlData.PlayerViewOrientationHandler = this.m_ActivePlayer.GetComponent<IPlayerViewOrientationHandler>();
            this.m_InputControlData.DebugHandler = this.m_ActivePlayer.GetComponent<IDebugHandler>();
            
            // Validate the values
            _ = GameValidator.NotNull(this.m_InputControlData.PauseActionHandler,
                    nameof(this.m_InputControlData.PauseActionHandler));
            _ = GameValidator.NotNull(this.m_InputControlData.PlayerAttackHandler, 
                    nameof(this.m_InputControlData.PlayerAttackHandler));
            _ = GameValidator.NotNull(this.m_InputControlData.PlayerMovement, 
                    nameof(this.m_InputControlData.PlayerMovement));
            _ = GameValidator.NotNull(this.m_InputControlData.PlayerTargetTracking, 
                    nameof(this.m_InputControlData.PlayerTargetTracking));
            _ = GameValidator.NotNull(this.m_InputControlData.PlayerViewOrientationHandler, 
                    nameof(this.m_InputControlData.PlayerViewOrientationHandler));
        }

        #endregion Methods
        
    }

}