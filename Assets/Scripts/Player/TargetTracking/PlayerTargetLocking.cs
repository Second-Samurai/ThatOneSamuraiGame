using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.TargetTracking
{
    
    [RequireComponent(typeof(IPlayerMovement))]
    public class PlayerTargetLocking : PausableMonoBehaviour, IPlayerTargetTracking
    {

        #region - - - - - - Fields - - - - - -

        private ICameraController m_CameraController;
        private IPlayerMovement m_PlayerMovement;

        #endregion Fields

        #region - - - - - - Initializers - - - - - -

        public void Initialize(ICameraController cameraController)
        {
            this.m_CameraController = cameraController ?? throw new ArgumentNullException(nameof(cameraController));
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
        }

        #endregion Initializers
  
        #region - - - - - - Methods - - - - - -

        void IPlayerTargetTracking.ToggleLockLeft()
        {
            // TODO: Change to get new target
            // if (this.m_CameraController.IsLockedOn)
            //     this.m_CameraController.LockOn();
        }

        public void ToggleLockOn()
        {
            this.m_CameraController.SelectCamera(SceneCameras.LockOn);
            this.m_PlayerMovement.SetState(PlayerMovementStates.LockOn);
        }

        void IPlayerTargetTracking.ToggleLockRight()
        {
            // TODO: Change to get new target
            // if (this.m_CameraController.IsLockedOn)
            //     this.m_CameraController.LockOn();
        }

        #endregion Methods
        
    }
    
}
