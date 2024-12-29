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
        private ICombatController m_CombatController;
        private IPlayerMovement m_PlayerMovement;
        private ILockOnSystem m_LockOnSystem;

        private bool m_IsLockedOn;

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
            if (this.m_IsLockedOn)
                this.m_LockOnSystem.StartLockOn(); // Change to use a seperate method called 'Get new target'
        }

        public void ToggleLockOn()
        {
            // Cannot run lock on targeting if no swords are drawn
            if (!this.m_CombatController.IsSwordDrawn)
                return;

            this.m_IsLockedOn = true;
            
            this.m_LockOnSystem.StartLockOn();
            this.m_CameraController.SelectCamera(SceneCameras.LockOn);
            this.m_PlayerMovement.SetState(PlayerMovementStates.LockOn);
        }

        void IPlayerTargetTracking.ToggleLockRight()
        {
            // Note: Does not matter whether the target is left or right.
            if (this.m_IsLockedOn)
                this.m_LockOnSystem.StartLockOn(); // Change to use a seperate method called 'Get new target'
        }

        #endregion Methods
        
    }
    
}
