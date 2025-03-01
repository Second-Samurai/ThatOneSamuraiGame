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
        private ILockOnSystem m_LockOnSystem;
        private IWeaponSystem m_WeaponSystem;

        private bool m_IsLockedOn;

        #endregion Fields

        #region - - - - - - Initializers - - - - - -

        public void Initialize(ICameraController cameraController)
        {
            this.m_CameraController = cameraController ?? throw new ArgumentNullException(nameof(cameraController));
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
            this.m_LockOnSystem = this.GetComponentInChildren<ILockOnSystem>();
            this.m_WeaponSystem = this.GetComponent<IWeaponSystem>();
        }

        #endregion Initializers
  
        #region - - - - - - Methods - - - - - -

        void IPlayerTargetTracking.ToggleLockLeft()
        {
            if (!this.m_IsLockedOn) return;
            this.m_LockOnSystem.SelectNewTarget();
        }

        public void ToggleTargetLocking()
        {
            // Cannot run lock on targeting if no swords are drawn
            if (!this.m_WeaponSystem.IsWeaponDrawn)
                return;

            if (this.m_IsLockedOn)
            {
                this.m_LockOnSystem.EndLockOn();
                return;
            }

            this.m_IsLockedOn = true;
            this.m_LockOnSystem.StartLockOn();
            this.m_PlayerMovement.SetState(PlayerMovementStates.LockOn);
        }

        void IPlayerTargetTracking.ToggleLockRight()
        {
            if (!this.m_IsLockedOn) return;
            this.m_LockOnSystem.SelectNewTarget();
        }

        #endregion Methods
        
    }
    
}
