using System;
using System.Collections;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Enumeration;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThatOneSamuraiGame.Scripts.Scene.Loaders
{

    public interface ISceneLoader
    {

        #region - - - - - - Methods - - - - - -

        void LoadScene(GameSceneEnum selectedGameScene, LoadSceneMode loadSceneMode);

        #endregion Methods

    }
    
    public class SceneLoader : MonoBehaviour, ISceneLoader
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

        public void LoadScene(GameSceneEnum selectedGameScene, LoadSceneMode loadSceneMode) 
            => this.StartCoroutine(this.LoadSceneRoutine(selectedGameScene, loadSceneMode));

        private IEnumerator LoadSceneRoutine(GameScene gameScene, LoadSceneMode loadSceneMode)
        {
            if (gameScene > this.m_SceneCount)
                throw new ArgumentOutOfRangeException();

            var _AsyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(gameScene.GetValue(), loadSceneMode);
            GameLogger.Log($"[LOG]: Loading scene {gameScene.GetValue()} -> '{gameScene}'");

            while (!_AsyncOperation.isDone)
                yield return null;

            this.m_SceneManager.SetupCurrentScene(gameScene);

            yield return null;
        }

        #endregion Methods
  
    }

}