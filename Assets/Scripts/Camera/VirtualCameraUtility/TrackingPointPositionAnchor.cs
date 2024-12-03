using ThatOneSamuraiGame.Scripts.Base;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Camera.VirtualCameraUtility
{

    /// <summary>
    /// Responsible for locking a virtual camera to a target's position.
    /// </summary>
    public class TrackingPointPositionAnchor : PausableMonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField] private Transform m_TargetTransform;
        [SerializeField] private Vector3 m_PositionOffset;

        #endregion Fields

        #region - - - - - - Unity Methods - - - - - -

        private void Update()
        {
            if (this.IsPaused || this.m_TargetTransform == null) return;
            this.transform.position = this.m_TargetTransform.position + this.m_PositionOffset;
        }

        #endregion Unity Methods

        #region - - - - - - Gizmos - - - - - -

        private void OnDrawGizmos()
        {
            if (this.m_TargetTransform == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(this.transform.position + this.m_PositionOffset, 0.1f);
        }

        #endregion Gizmos
  
    }

}