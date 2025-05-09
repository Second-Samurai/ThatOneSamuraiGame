using System;
using System.Collections;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Enumeration;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace ThatOneSamuraiGame.Scripts.Scene.Loaders
{

    public interface ISceneLoader
    {

        #region - - - - - - Methods - - - - - -

        void LoadScene(GameSceneEnum selectedGameScene, LoadSceneMode loadSceneMode);

        void UnloadScene(GameSceneEnum selectedGameScene);

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
        {
            if (this.IsSceneLoaded(selectedGameScene)) return;
            this.StartCoroutine(this.LoadSceneRoutine(selectedGameScene, loadSceneMode));
        }

        public void UnloadScene(GameSceneEnum selectedGameScene) 
            => this.StartCoroutine(this.UnloadSceneRoutine(selectedGameScene));

        private IEnumerator LoadSceneRoutine(GameScene gameScene, LoadSceneMode loadSceneMode)
        {
            if (gameScene > this.m_SceneCount)
                throw new ArgumentOutOfRangeException();

            var _AsyncOperation = UnitySceneManager.LoadSceneAsync(gameScene.GetValue(), loadSceneMode);
            GameLogger.Log($"[LOG]: Loading scene {gameScene.GetValue()} -> '{gameScene}'");

            while (!_AsyncOperation.isDone)
                yield return null;
            
            this.m_SceneManager.SetupCurrentScene(gameScene);
        }

        private IEnumerator UnloadSceneRoutine(GameScene gameScene)
        {
            var _AsyncOperation = UnitySceneManager.UnloadSceneAsync(gameScene.GetValue());
            GameLogger.Log($"[LOG]: Unload scene {gameScene.GetValue()} -> '{gameScene}'");

            while (!_AsyncOperation.isDone)
                yield return null;
        }

        private bool IsSceneLoaded(GameScene gameScene)
        {
            for (int i = 0; i < UnitySceneManager.sceneCount; i++)
            {
                if (UnitySceneManager.GetSceneAt(i).name == gameScene)
                    return true;
            }

            return false;
        }

        #endregion Methods
  
    }

}