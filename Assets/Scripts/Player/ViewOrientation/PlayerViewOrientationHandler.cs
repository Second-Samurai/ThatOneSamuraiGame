using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera;
using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Player.ViewOrientation
{

    public class PlayerViewOrientationHandler: TOSGMonoBehaviourBase, IPlayerViewOrientationHandler
    {
        
        #region - - - - - - Fields - - - - - -

        private ICameraController m_CameraController;
        private IPlayerAttackHandler m_PlayerAttackHandler;
        private Vector2 m_ViewRotation = Vector2.zero;

        private bool m_IsInputActive;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_CameraController = this.GetComponent<ICameraController>();
            this.m_PlayerAttackHandler = this.GetComponent<IPlayerAttackHandler>();
        }

        private void Update()
        {
            if (!this.m_IsInputActive || IsPaused) return;
            this.RotateCameraToViewRotation();
        }

        #endregion Lifecycle Methods

        #region - - - - - - Event Handlers - - - - - -

        void IPlayerViewOrientationHandler.RotateViewOrientation(Vector2 mouseInputVector) 
            => this.m_ViewRotation = mouseInputVector;

        #endregion Event Handlers

        #region - - - - - - Methods - - - - - -

        private void RotateCameraToViewRotation()
        {
            // if (this.m_ViewRotation != Vector2.zero && !bLockedOn)
            //     camTargetScript.RotateCam(rotationVector);
            // else if (GameManager.instance.rewindManager.isTravelling)
            // {
            //     camTargetScript.RotateCam(rotationVector);
            // }
            //
            // if (_bRunLockCancelTimer) RunLockCancelTimer();
            // else _lockCancelTimer = maxLockCancelTimer;
        }

        #endregion Methods
        
    }

}