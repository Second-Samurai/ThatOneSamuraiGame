using ThatOneSamuraiGame.Scripts.Scene.DataContainers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Scene.SceneManager
{

    /// <summary>
    /// Responsible for managing both state and behavior of the scene.
    /// </summary>
    public class SceneManager : MonoBehaviour, ISceneManager
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField] private SceneState m_SceneState;
        [SerializeField] private RewindManager m_RewindManager;
        [SerializeField] private EnemyTracker m_EnemyTracker;
        
        #endregion Fields
        
        #region - - - - - - Properties - - - - - -

        SceneState ISceneManager.SceneState
            => this.m_SceneState;

        #endregion Properties

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            if (!this.DoesSceneStateExist())
                this.m_SceneState = this.GetComponent<SceneState>();
        }

        #endregion Lifecycle Methods

        #region - - - - - - Validation Methods - - - - - -

        private bool DoesSceneStateExist()
            => this.m_SceneState != null;

        #endregion Validation Methods

    }

}