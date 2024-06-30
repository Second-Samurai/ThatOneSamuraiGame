using ThatOneSamuraiGame.Scripts.UI.Pause;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Base
{
    
    /// <summary>
    /// Derives from Unity's MonoBehaviour. Includes actions and behaviour extending MonoBehaviour.
    /// </summary>
    public class TOSGMonoBehaviourBase : MonoBehaviour, IPausable
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
        void IPausable.OnPause()
            => this.m_IsPaused = true;

        /// <summary>
        /// Changes object's state to unpause.
        /// </summary>
        void IPausable.OnUnPause()
            => this.m_IsPaused = true;

        #endregion IPausable Methods
        
    }

}
