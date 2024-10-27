using System;
using ThatOneSamuraiGame.Scripts.General.Services;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Initializers
{

    public class PlayerInitializerCommand : ICommand
    {

        #region - - - - - - Fields - - - - - -

        private readonly GameObject m_Player;

        #endregion Fields
  
        #region - - - - - - Constructors - - - - - -

        public PlayerInitializerCommand(GameObject player)
        {
            this.m_Player = player ?? throw new ArgumentNullException(nameof(player));
        }

        #endregion Constructors
  
        #region - - - - - - Methods - - - - - -

        public void Execute()
        {
            // Initialise animation behavior
            FinishingMoveController _FinishingMoveController =
                this.m_Player.GetComponentInChildren<FinishingMoveController>();
            _FinishingMoveController.Initialise();

            // Initialise lock-on behavior
            AddToLockOnTracker _AddToLockOnTracker = this.m_Player.GetComponentInChildren<AddToLockOnTracker>();
            _AddToLockOnTracker.Initialise();
        }

        #endregion Methods
  
    }

}