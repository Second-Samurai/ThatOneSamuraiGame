using ThatOneSamuraiGame.Legacy;
using ThatOneSamuraiGame.Scripts.Base;

namespace ThatOneSamuraiGame.Scripts.Player.TargetTracking
{
    
    public class PlayerTargetLocking : PausableMonoBehaviour, IPlayerTargetTracking
    {

        #region - - - - - - Fields - - - - - -

        private Legacy.ICameraController m_CameraController;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start() 
            => this.m_CameraController = this.GetComponent<Legacy.ICameraController>();

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -

        void IPlayerTargetTracking.ToggleLockLeft()
        {
            if (this.m_CameraController.IsLockedOn)
                this.m_CameraController.LockOn();
        }

        void IPlayerTargetTracking.ToggleLockOn()
            => this.m_CameraController.ToggleLockOn();
        
        void IPlayerTargetTracking.ToggleLockRight()
        {
            if (this.m_CameraController.IsLockedOn)
                this.m_CameraController.LockOn();
        }

        #endregion Methods
        
    }
    
}
