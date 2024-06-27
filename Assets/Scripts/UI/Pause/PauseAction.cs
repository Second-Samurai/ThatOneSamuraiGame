using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.UI.Pause
{
    
    public class PauseAction : MonoBehaviour, IPausable
    {
        
        #region - - - - - - Methods - - - - - -
        
        void IPausable.OnPause()
        {
            throw new System.NotImplementedException();
        }
        
        #endregion Methods

    }
    
}
