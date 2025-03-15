using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.TargetTracking
{
    
    [RequireComponent(typeof(IPlayerMovement))]
    public class PlayerTargetLocking : PausableMonoBehaviour, IPlayerTargetTracking
    {

        #region - - - - - - Fields - - - - - -

        private IPlayerMovement m_PlayerMovement;
        private ILockOnSystem m_LockOnSystem;
        private IWeaponSystem m_WeaponSystem;

        private bool m_IsLockedOn;

        #endregion Fields

        #region - - - - - - Initializers - - - - - -

        public void Initialize()
        {
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
            this.m_LockOnSystem = this.GetComponentInChildren<ILockOnSystem>();
            this.m_WeaponSystem = this.GetComponent<IWeaponSystem>();
        }

        #endregion Initializers
  
        #region - - - - - - Methods - - - - - -

        void IPlayerTargetTracking.ToggleLockLeft()
        {
            if (!this.m_LockOnSystem.IsLockingOnTarget) return;
            this.m_LockOnSystem.SelectNewTarget();
        }

        public void ToggleTargetLocking()
        {
            // Cannot run lock on targeting if no swords are drawn
            if (!this.m_WeaponSystem.IsWeaponDrawn)
                return;

            if (this.m_LockOnSystem.IsLockingOnTarget)
            {
                this.m_LockOnSystem.EndLockOn();
                return;
            }

            this.m_LockOnSystem.StartLockOn();
            this.m_PlayerMovement.SetState(PlayerMovementStates.LockOn);
        }

        void IPlayerTargetTracking.ToggleLockRight()
        {
            if (!this.m_LockOnSystem.IsLockingOnTarget) return;
            this.m_LockOnSystem.SelectNewTarget();
        }

        #endregion Methods
        
    }
    
}
