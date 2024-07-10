using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera;
using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Player.ViewOrientation
{

    public class PlayerViewOrientationHandler: TOSGMonoBehaviourBase, IPlayerViewOrientationHandler
    {
        
        #region - - - - - - Fields - - - - - -

        private ICameraController m_CameraController;
        
        private bool m_IsInputActive;
        private Vector2 m_ViewRotation = Vector2.zero;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start() 
            => this.m_CameraController = this.GetComponent<ICameraController>();

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
            // Note: This calculation can be collapsed to a single if statement.
            if (this.m_ViewRotation != Vector2.zero && !this.m_CameraController.IsLockedOn)
                this.m_CameraController.RotateCamera(this.m_ViewRotation);
            else if (GameManager.instance.rewindManager.isTravelling)
                this.m_CameraController.RotateCamera(this.m_ViewRotation);
        }

        #endregion Methods
        
    }

}