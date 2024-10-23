using System;
using System.Collections;
using ThatOneSamuraiGame.Scripts.Enumeration;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Scene.Loaders
{

    public class SceneLoader : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        private ISceneManager m_SceneManager;
        
        private int m_SceneCount;

        #endregion Fields

        #region - - - - - - Methods - - - - - -

        public void InitialiseSceneLoader()
        {
            this.m_SceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            this.m_SceneManager = SceneManager.SceneManager.Instance;
        }

        public IEnumerator LoadScene(GameScenes gameScene)
        {
            if (gameScene > this.m_SceneCount)
                throw new ArgumentOutOfRangeException();

            UnityEngine.SceneManagement.SceneManager.LoadScene(gameScene.GetValue());
            Debug.Log($"[LOG]: Loading scene {gameScene.GetValue()} -> '{gameScene}'");

            this.m_SceneManager.SetupCurrentScene(gameScene);

            yield return null;
        }

        #endregion Methods
  
    }

}