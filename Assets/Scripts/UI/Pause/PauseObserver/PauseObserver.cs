using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.UI.Pause.PauseObserver
{

    public class PauseObserver : MonoBehaviour, IPauseObserver
    {

        #region - - - - - - Methods - - - - - -

        void IPauseObserver.PauseAllObjects()
        {
            Debug.LogWarning("Objects are paused here. No implementation exists.");
        }

        void IPauseObserver.UnpauseAllObjects()
        {
            Debug.LogWarning("Objects are unpaused here. No implementation exists.");
        }

        #endregion Methods
        
    }

}