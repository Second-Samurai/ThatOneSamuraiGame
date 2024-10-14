using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Infrastructure.CustomRuntimeHandlers
{

    /// <summary>
    /// Ensures that the GameObject is not destroyed during gameplay.
    /// </summary>
    public class PersistentObject : MonoBehaviour
    {
        
        #region - - - - - - Unity Lifecycle Methods - - - - - -

        private void Awake()
        {
            if (this.transform.parent != null)
                this.transform.SetParent(null);
            
            Object.DontDestroyOnLoad(this.gameObject);
        }

        #endregion Unity Lifecycle Methods
  
    }

}