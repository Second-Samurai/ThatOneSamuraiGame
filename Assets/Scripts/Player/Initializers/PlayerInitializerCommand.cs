using System;
using ThatOneSamuraiGame.Legacy;
using ThatOneSamuraiGame.Scripts.General.Services;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Initializers
{

    public class PlayerInitializerCommand : ICommand
    {

        #region - - - - - - Fields - - - - - -

        private readonly GameObject m_Player;
        
        // External dependencies
        // private readonly PlayerCamTargetController m_PlayerCameraTargetController;
        // private readonly ThirdPersonCamController m_ThirdPersonCamController;
        private readonly GameObject m_TargetHolder;
        private readonly ICameraController m_CameraController;

        #endregion Fields
  
        #region - - - - - - Constructors - - - - - -

        public PlayerInitializerCommand(
            GameObject player, 
            // PlayerCamTargetController playerCameraTargetController,
            // ThirdPersonCamController thirdPersonCamController, 
            GameObject targetHolder,
            ICameraController cameraController)
        {
            this.m_Player = player ?? throw new ArgumentNullException(nameof(player));
            // this.m_PlayerCameraTargetController = playerCameraTargetController ??
            //                                       throw new ArgumentNullException(nameof(playerCameraTargetController));
            // this.m_ThirdPersonCamController = thirdPersonCamController ??
            //                                   throw new ArgumentNullException(nameof(thirdPersonCamController));
            this.m_TargetHolder = targetHolder ?? throw new ArgumentNullException(nameof(targetHolder));
            this.m_CameraController = cameraController ?? throw new ArgumentNullException(nameof(cameraController));
        }

        #endregion Constructors
  
        #region - - - - - - Methods - - - - - -

        // TODO: Remove old player camera controller initialisation
        // TODO: Migrate player initialisation from controller to command
        public void Execute()
        {
            // Initialise camera behavior
            // this.m_ThirdPersonCamController.Initialise();
            
            // Initialise animation behavior
            // FinishingMoveController _FinishingMoveController =
            //     this.m_Player.GetComponentInChildren<FinishingMoveController>();
            // _FinishingMoveController.Initialise();

            PlayerTargetLocking _PlayerTargetLocking = this.m_Player.GetComponent<PlayerTargetLocking>();
            _PlayerTargetLocking.Initialize(this.m_CameraController);

            // Initialise lock-on behavior
            // AddToLockOnTracker _AddToLockOnTracker = this.m_Player.GetComponentInChildren<AddToLockOnTracker>();
            // _AddToLockOnTracker.Initialise();
            //
            // LockOnTargetManager _LockOnTargetManager = this.m_Player.GetComponentInChildren<LockOnTargetManager>();
            // _LockOnTargetManager.Initialise();
            
            // Initialise Camera Control
            // CameraControl _CameraController = this.m_Player.GetComponent<CameraControl>();
            // _CameraController.camTargetScript = this.m_PlayerCameraTargetController;

            // Temp: Call PlayerControl Initializer
            // TODO: Migrate PlayerController specific initialization here.
            PlayerController _PlayerController = this.m_Player.GetComponent<PlayerController>();
            _PlayerController.Init(this.m_TargetHolder);
        }

        #endregion Methods
  
    }

}