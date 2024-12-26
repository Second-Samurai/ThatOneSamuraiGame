using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using Unity.XR.OpenVR;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.TargetTracking
{
    
    [RequireComponent(typeof(IPlayerMovement))]
    public class PlayerTargetLocking : PausableMonoBehaviour, IPlayerTargetTracking
    {

        #region - - - - - - Fields - - - - - -

        private ICameraController m_CameraController;
        private ICombatController m_CombatController;
        private IPlayerMovement m_PlayerMovement;
        private ILockOnSystem m_LockOnSystem;

        #endregion Fields

        #region - - - - - - Initializers - - - - - -

        public void Initialize(ICameraController cameraController)
        {
            this.m_CameraController = cameraController ?? throw new ArgumentNullException(nameof(cameraController));
            this.m_CombatController = this.GetComponent<ICombatController>();
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
            this.m_LockOnSystem = this.GetComponentInChildren<ILockOnSystem>();
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
            // Cannot run lock on targeting if no swords are drawn
            if (!this.m_CombatController.IsSwordDrawn) return;
            
            this.m_LockOnSystem.StartLockOn();
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
