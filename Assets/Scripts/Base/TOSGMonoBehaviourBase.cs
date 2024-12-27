using ThatOneSamuraiGame.Scripts.UI.Pause;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Base
{
    
    /// <summary>
    /// Derives from Unity's MonoBehaviour. Includes actions and behaviour extending MonoBehaviour.
    /// </summary>
    public class PausableMonoBehaviour : MonoBehaviour, IPausable
    {

        #region - - - - - - Fields - - - - - -

        private bool m_IsPaused = false;

        #endregion Fields
        
        #region - - - - - - Properties - - - - - -

        public bool IsPaused
            => this.m_IsPaused;

        #endregion Properties

        #region - - - - - - IPausable Methods - - - - - -

        /// <summary>
        /// Changes object's state to pause.
        /// </summary>
        public virtual void OnPause()
            => this.m_IsPaused = true;

        /// <summary>
        /// Changes object's state to unpause.
        /// </summary>
        public virtual void OnUnPause()
            => this.m_IsPaused = false;

        #endregion IPausable Methods
        
    }

}
