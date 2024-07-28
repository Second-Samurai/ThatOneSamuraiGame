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
        
        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            if (!this.DoesSceneStateExist())
                this.m_SceneState = this.GetComponent<SceneState>();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -

        SceneState ISceneManager.SceneState
            => this.m_SceneState;

        #endregion Methods

        #region - - - - - - Validation Methods - - - - - -

        private bool DoesSceneStateExist()
            => this.m_SceneState != null;

        #endregion Validation Methods

    }

}