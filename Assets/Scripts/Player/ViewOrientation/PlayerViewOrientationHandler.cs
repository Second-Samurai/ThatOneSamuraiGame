using ThatOneSamuraiGame.Scripts.Base;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.ViewOrientation
{

    public class PlayerViewOrientationHandler: PausableMonoBehaviour, IPlayerViewOrientationHandler
    {
        
        #region - - - - - - Fields - - - - - -

        private ThatOneSamuraiGame.Legacy.ICameraController m_CameraController;
        
        private Vector2 m_ViewRotation = Vector2.zero;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start() 
            => this.m_CameraController = this.GetComponent<ThatOneSamuraiGame.Legacy.ICameraController>();

        private void Update()
        {
            if (IsPaused) return;
            this.RotateCameraToViewRotation();
        }

        #endregion Lifecycle Methods

        #region - - - - - - Event Handlers - - - - - -

        void IPlayerViewOrientationHandler.RotateViewOrientation(Vector2 mouseInputVector) 
            => this.m_ViewRotation = mouseInputVector;

        #endregion Event Handlers

        #region - - - - - - Methods - - - - - -

        Vector2 IPlayerViewOrientationHandler.GetInputScreenPosition()
            => this.m_ViewRotation;

        // FOLLOW BEHAVIOUR
        private void RotateCameraToViewRotation()
        {
            // Note: This calculation can be collapsed to a single if statement.
            if (this.m_ViewRotation != Vector2.zero && !this.m_CameraController.IsLockedOn)
            {
                this.m_CameraController.RotateCamera(this.m_ViewRotation);  
            }
        }

        #endregion Methods
        
    }

}