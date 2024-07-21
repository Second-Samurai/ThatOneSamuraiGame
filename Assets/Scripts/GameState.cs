using UnityEngine;

namespace ThatOneSamuraiGame.Scripts
{

    /// <summary>
    ///     GameState keeps track of the game's current state.
    /// </summary>
    public class GameState : MonoBehaviour
    {
        
        #region - - - - - - Fields - - - - - -

        // User/Player Objects
        [SerializeField]
        private GameObject m_ActivePlayer;

        // Persistent Objects
        [SerializeField]
        private GameObject m_SessionUser;

        // User Interfaces
        [SerializeField]
        private GameObject m_PauseMenu;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public GameObject ActivePlayer
        {
            get => m_ActivePlayer;
            set => m_ActivePlayer = value;
        }

        public GameObject SessionUser
        {
            get
            {
                if (m_SessionUser == null)
                    Debug.LogError("The game session user is missing. Please ensure the object exists");

                return m_SessionUser;
            }
            set => m_SessionUser = value;
        }

        public GameObject PauseMenu
        {
            get => m_PauseMenu;
            set => m_PauseMenu = value;
        }

        #endregion Properties
        
    }

}
