using UnityEngine;

namespace ThatOneSamuraiGame.Scripts
{
    
    /// <summary>
    /// GameState keeps track of the game's current state.
    /// </summary>
    public class GameState : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        // User/Player Objects
        [SerializeField]
        private GameObject m_ActivePlayer;
        [SerializeField]
        private GameObject m_SessionUser;
        
        // User Interfaces
        [SerializeField]
        private GameObject m_PauseMenu;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public GameObject ActivePlayer
        {
            get { return this.m_ActivePlayer; }
            set { this.m_ActivePlayer = value; }
        }

        public GameObject SessionUser
        {
            get
            {
                if (this.m_SessionUser == null)
                    Debug.LogError("The game session user is missing. Please ensure the object exists");

                return m_SessionUser;
            }
            set => this.m_SessionUser = value;
        }

        public GameObject PauseMenu
        {
            get { return this.m_PauseMenu; }
            set { this.m_PauseMenu = value; }
        }

        #endregion Properties

    }
    
}
