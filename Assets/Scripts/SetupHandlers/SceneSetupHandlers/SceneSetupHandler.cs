using System;
using System.Collections;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupHandlers
{

    /// <summary>
    /// Responsible for handling initial setup logic for a scene.
    /// </summary>
    public class SceneSetupHandler : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        public RewindBar RewindBar;

        #endregion Fields
  
        #region - - - - - - Unity Lifecycle Methods - - - - - -

        private void Awake() 
            => this.StartCoroutine(this.RunSetup());

        #endregion Unity Lifecycle Methods
  
        #region - - - - - - Methods - - - - - -

        public void InitialiseSceneSetupHandler()
        {
            
        }

        public IEnumerator RunSetup()
        {
            yield return StartCoroutine(this.SetupUserInterface());
            yield return StartCoroutine(this.SetupScene());
            
            print($"[{DateTime.Now}]: Scene has been setup.");
        }
        
        private IEnumerator SetupScene()
        {
            ISceneManager _SceneManager =
                Object.FindFirstObjectByType<SceneManager>(FindObjectsInactive.Exclude);
            _SceneManager.SetupScene();
            yield return null;
        }

        private IEnumerator SetupUserInterface()
        {
            IUserInterfaceManager _UserInterfaceManager =
                Object.FindFirstObjectByType<UserInterfaceManager>(FindObjectsInactive.Exclude);
            _UserInterfaceManager.SetupUserInterface();
            
            yield return null;
        }

        #endregion Methods
  
    }

}