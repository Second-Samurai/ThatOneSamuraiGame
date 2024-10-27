using System;
using System.Collections;
using ThatOneSamuraiGame.Scripts.Scene.Loaders;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;
using UnityEngine.Events;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.GameSetupHandlers
{

    public class GameSetupHandler : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        public UnityEvent OnGameSetupCompletion;

        #endregion Fields
  
        #region - - - - - - Methods - - - - - -

        public IEnumerator RunSetup()
        {
            yield return StartCoroutine(this.SetupAudio());
            yield return StartCoroutine(this.SetupGraphics());
            yield return StartCoroutine(this.SetupManagers());
            yield return StartCoroutine(this.SetupServices());
            yield return StartCoroutine(this.LaunchGameScene());
            
            print($"[{DateTime.Now}]: Game Management layer has been setup.");
        }

        private IEnumerator SetupAudio()
        {
            if (FindAnyObjectByType<AudioManager>()) 
                yield return null;

            GameObject _AudioManagerObject = Instantiate(
                GameManager.instance.gameSettings.audioManger,
                transform.position,
                Quaternion.identity);
            GameManager.instance.audioManager = _AudioManagerObject.GetComponent<AudioManager>();
            
            yield return null;
        }

        private IEnumerator SetupGraphics()
        {
            GameManager.instance.postProcessingController = 
                Instantiate(
                    GameManager.instance.gameSettings.dayPostProcessing, 
                    transform.position, Quaternion.identity).GetComponent<PostProcessingController>();

            yield return null;
        }

        private IEnumerator SetupServices()
        {
            ISceneManager _SceneManager = SceneManager.Instance;

            SceneLoader _SceneLoader = _SceneManager.SceneLoader;
            _SceneLoader.InitialiseSceneLoader();
            
            yield return null;
        }

        private IEnumerator SetupManagers()
        {
            // GameManager.instance.RewindManager.InitialiseRewindManager();
            // GameManager.instance.CheckpointManager.InitializeCheckpointManager();

            yield return null;
        }

        private IEnumerator LaunchGameScene()
        {
            this.OnGameSetupCompletion.Invoke();
            yield return null;
        }

        #endregion Methods
  
    }

}