using ThatOneSamuraiGame.Scripts.Scene.DataContainers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts
{

    /// <summary>
    /// GameState keeps track of the game's current state.
    /// </summary>
    public class GameState : MonoBehaviour
    {
        
        #region - - - - - - Fields - - - - - -

        // Scene Objects
        [SerializeField]
        private GameObject m_ActivePlayer;
        [SerializeField]
        private GameObject m_SceneManagement;
        [SerializeField]
        private GameObject m_PauseManager;

        // Persistent Objects
        [SerializeField]
        private GameObject m_SessionUser;


        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public GameObject ActivePlayer
        {
            get => m_ActivePlayer;
            set => m_ActivePlayer = value;
        }

        public GameObject SceneManagement 
            => this.m_SceneManagement;

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

        public GameObject PauseManager
        {
            get => m_PauseManager;
            set => m_PauseManager = value;
        }

        #endregion Properties
        
    }

}
