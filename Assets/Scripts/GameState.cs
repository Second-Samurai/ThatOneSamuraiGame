using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts
{

    /// <summary>
    /// GameState keeps track of the game's current state.
    /// </summary>
    public class GameState : MonoBehaviour
    {
        
        #region - - - - - - Fields - - - - - -

        public bool IsGameplayActive;

        // Persisted Objects
        [SerializeField] private GameObject m_SessionUser;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public GameObject SessionUser 
            => this.m_SessionUser;

        #endregion Properties
        
    }

}
