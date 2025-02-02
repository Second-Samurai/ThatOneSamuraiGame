using System;
using ThatOneSamuraiGame.Scripts.General.Services;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Initializers
{

    public class PlayerInitializerCommand : ICommand
    {

        #region - - - - - - Fields - - - - - -

        private readonly GameObject m_Player;
        private readonly GameObject m_TargetHolder;
        private readonly ICameraController m_CameraController;
        private readonly ILockOnObserver m_LockOnObserver;

        #endregion Fields
  
        #region - - - - - - Constructors - - - - - -

        public PlayerInitializerCommand(
            GameObject player, 
            GameObject targetHolder,
            ICameraController cameraController,
            ILockOnObserver lockOnObserver)
        {
            this.m_Player = player ?? throw new ArgumentNullException(nameof(player));
            this.m_TargetHolder = targetHolder ?? throw new ArgumentNullException(nameof(targetHolder));
            this.m_CameraController = cameraController ?? throw new ArgumentNullException(nameof(cameraController));
            this.m_LockOnObserver = lockOnObserver ?? throw new ArgumentNullException(nameof(lockOnObserver));
        }

        #endregion Constructors
  
        #region - - - - - - Methods - - - - - -

        public void Execute()
        {
            PlayerTargetLocking _PlayerTargetLocking = this.m_Player.GetComponent<PlayerTargetLocking>();
            _PlayerTargetLocking.Initialize(this.m_CameraController);

            // TODO: Migrate PlayerController specific initialization here.
            PlayerController _PlayerController = this.m_Player.GetComponent<PlayerController>();
            _PlayerController.Init(this.m_TargetHolder);

            IInitialize<PlayerAttackInitializerData> _PlayerAttackInitializer =
                this.m_Player.GetComponent<IInitialize<PlayerAttackInitializerData>>();
            _PlayerAttackInitializer.Initialize(new PlayerAttackInitializerData(this.m_CameraController));

            IInitialize<PlayerMovementInitializerData> _PlayerMovementInitializer =
                this.m_Player.GetComponent<IInitialize<PlayerMovementInitializerData>>();
            _PlayerMovementInitializer.Initialize(new PlayerMovementInitializerData(
                this.m_CameraController, this.m_LockOnObserver));
            
            IInitialize<PlayerFinisherControllerInitializerData> _PlayerFinisherControllerInitializer =
                this.m_Player.GetComponentInChildren<IInitialize<PlayerFinisherControllerInitializerData>>();
            _PlayerFinisherControllerInitializer
                .Initialize(new PlayerFinisherControllerInitializerData(this.m_CameraController, this.m_LockOnObserver));
        }

        #endregion Methods
  
    }

}