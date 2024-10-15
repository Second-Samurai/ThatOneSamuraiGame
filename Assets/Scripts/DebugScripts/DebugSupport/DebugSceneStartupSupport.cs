using System;
using ThatOneSamuraiGame.Scripts.SetupHandlers.GameSetupHandlers;
using ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupHandlers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ThatOneSamuraiGame.Scripts.DebugScripts.DebugSupport
{

    public class DebugSceneStartupSupport : MonoBehaviour
    {
        
        #region - - - - - - Fields - - - - - -

        public bool IsSceneInDevelopment = false;

        private GameObject[] m_ActiveSceneObjects;

        #endregion Fields
  
        #region - - - - - - Unity Lifecycle Methods - - - - - -

        private void Awake()
        {
            // Validate whether boot scene was run first
            if (this.ValidateSupportRequirements()) return;
            
            DebugSceneStartupSupport[] _StartupSupport = 
                Object.FindObjectsByType<DebugSceneStartupSupport>(FindObjectsSortMode.None);

            // Remove duplicate instance
            if (_StartupSupport.Length > 1)
            {
                Debug.LogWarning(
                    $"[WARNING]: Startup Support Detected. Deleting this object for scene " +
                    $"{SceneManager.GetActiveScene().name}");
                Destroy(this.gameObject);
                return;
            }
            
            this.LoadPersistenceScene();
        }

        #endregion Unity Lifecycle Methods

        #region - - - - - - Methods - - - - - -

        public virtual void ActivateSceneObjects()
        {
            foreach (GameObject _SceneObject in this.m_ActiveSceneObjects)
            {
                // if (!_SceneObject.layer.Equals(GameLayer.Ignore))
                //     _SceneObject.SetActive(true);
            }

            // Clear debug object collection from memory
            this.m_ActiveSceneObjects = Array.Empty<GameObject>();
            
            // Run the scene startup behavior
            SceneSetupHandler _StartupHandler =
                Object.FindFirstObjectByType<SceneSetupHandler>(FindObjectsInactive.Exclude);
            if (_StartupHandler == null) return;

            // _StartupHandler.InitialiseSceneStartupController();
            // this.StartCoroutine(_StartupHandler.RunSceneStartup());
        }

        protected void LoadPersistenceScene()
        {
            if (!this.IsSceneInDevelopment) return;
            this.DeactivateScene();
            
            // Loads the persistent scene first
            // SceneManager.LoadScene(GameScenes.PersistenceScene.GetValue(), LoadSceneMode.Additive);
        }

        private void DeactivateScene()
        {
            // Move gameobject to root
            this.gameObject.transform.parent = null;
            
            // Disable all scene object
            this.m_ActiveSceneObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject _SceneObject in this.m_ActiveSceneObjects)
            {
                // if (!_SceneObject.layer.Equals(GameLayer.Ignore) 
                //     && _SceneObject.GetInstanceID() != this.gameObject.GetInstanceID())
                //     _SceneObject.SetActive(false);
            }
        }

        protected bool ValidateSupportRequirements() 
            => Object.FindAnyObjectByType<DebugGameSupport>(FindObjectsInactive.Exclude);

        #endregion Methods
        
    }

}