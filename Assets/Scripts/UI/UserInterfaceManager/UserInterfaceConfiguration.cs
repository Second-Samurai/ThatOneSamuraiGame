using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager
{

    /// <summary>
    /// This stores both the injected User objects and prefab references.
    /// </summary>
    public class UserInterfaceConfiguration : MonoBehaviour
    {
        
        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private GameObject m_PauseMenu;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        // Properties are extensively used to guard the actual references to the objects.
        // All values in here are intended to be read-only.

        public GameObject PauseMenu
        {
            get
            {
                if (this.m_PauseMenu == null)
                    Debug.LogError("PauseMenu Missing. Object references are expected to be set.");
                
                return this.m_PauseMenu;
            }
        }

        #endregion Properties

    }

}