using System;
using System.Collections;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.GameSetupHandlers
{

    public class GameSetupHandler : MonoBehaviour
    {

        #region - - - - - - Methods - - - - - -

        public IEnumerator RunSetup()
        {
            yield return StartCoroutine(this.SetupManagers());
            yield return StartCoroutine(this.SetupAudio());
            yield return StartCoroutine(this.SetupGraphics());
            yield return StartCoroutine(this.LaunchGameScene());
            
            print($"[{DateTime.Now}]: Game Management layer has been setup.");
        }

        private IEnumerator SetupManagers()
        {
            Debug.Log("[LOG]: There are no managers to currently setup. Placeholder log thrown instead.");
            yield return null;
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

        private IEnumerator LaunchGameScene()
        {
            yield return null;
        }

        #endregion Methods
  
    }

}