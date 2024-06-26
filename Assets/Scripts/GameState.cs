using UnityEngine;

namespace ThatOneSamuraiGame.Scripts
{
    
    /// <summary>
    /// GameState keeps track of the game's current state.
    /// </summary>
    public class GameState : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private GameObject m_ActivePlayer;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public GameObject ActivePlayer
        {
            get { return this.m_ActivePlayer; }
            set { this.m_ActivePlayer = value; }
        }

        #endregion Properties

    }
    
}