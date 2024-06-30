using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.TargetTracking
{
    
    public class PlayerTargetLocking : TOSGMonoBehaviourBase, IPlayerTargetTracking
    {

        #region - - - - - - Fields - - - - - -

        private ICameraController m_CameraController;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
            => this.m_CameraController = this.GetComponent<ICameraController>();

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -

        void IPlayerTargetTracking.ToggleLockLeft()
            => this.m_CameraController.ToggleLockOn();

        void IPlayerTargetTracking.ToggleLockOn()
        {
            if (this.m_CameraController.IsLockedOn)
                this.m_CameraController.LockOn();
        }
        
        void IPlayerTargetTracking.ToggleLockRight()
        {
            if (this.m_CameraController.IsLockedOn)
                this.m_CameraController.LockOn();
        }

        #endregion Methods
        
    }
    
}
