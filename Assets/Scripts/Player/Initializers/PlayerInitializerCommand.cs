using System;
using ThatOneSamuraiGame.Scripts.General.Services;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Initializers
{

    public class PlayerInitializerCommand : ICommand
    {

        #region - - - - - - Fields - - - - - -

        private readonly GameObject m_Player;
        private readonly ThirdPersonCamController m_ThirdPersonCamController;

        #endregion Fields
  
        #region - - - - - - Constructors - - - - - -

        public PlayerInitializerCommand(GameObject player, ThirdPersonCamController thirdPersonCamController)
        {
            this.m_Player = player ?? throw new ArgumentNullException(nameof(player));
            this.m_ThirdPersonCamController = thirdPersonCamController ??
                                              throw new ArgumentNullException(nameof(thirdPersonCamController));
        }

        #endregion Constructors
  
        #region - - - - - - Methods - - - - - -

        public void Execute()
        {
            // Initialise camera behavior
            this.m_ThirdPersonCamController.Initialise();
            
            // Initialise animation behavior
            FinishingMoveController _FinishingMoveController =
                this.m_Player.GetComponentInChildren<FinishingMoveController>();
            _FinishingMoveController.Initialise();

            // Initialise lock-on behavior
            AddToLockOnTracker _AddToLockOnTracker = this.m_Player.GetComponentInChildren<AddToLockOnTracker>();
            _AddToLockOnTracker.Initialise();

            LockOnTargetManager _LockOnTargetManager = this.m_Player.GetComponentInChildren<LockOnTargetManager>();
            _LockOnTargetManager.Initialise();
        }

        #endregion Methods
  
    }

}