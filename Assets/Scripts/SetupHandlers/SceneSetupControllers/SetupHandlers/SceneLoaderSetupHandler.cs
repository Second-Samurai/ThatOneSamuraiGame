using System.Collections.Generic;
using System.Linq;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public class SceneLoaderSetupHandler : MonoBehaviour, ISetupHandler
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField] private HitstopController m_HitstopController;
        
        private ISetupHandler m_NextHandler;

        #endregion Fields
  
        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle(SceneSetupContext setupContext)
        {
            Transform _MainCamera = SceneManager.Instance.MainCamera.transform;
            
            List<ISceneLoader> _SceneLoaders = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                .Where(sho => sho.GetComponent<ISceneLoader>() != null)
                .Select(sho => sho.GetComponent<ISceneLoader>())
                .ToList();

            if (_SceneLoaders.Count != 0)
                foreach (ISceneLoader _SceneLoader in _SceneLoaders)
                    _SceneLoader.Initialise(_MainCamera.transform);
            
            // Set values from context object to Managers
            this.AssignToSingletonManager(setupContext);
            
            print("[LOG]: Completed Scene Loader setup.");
            this.m_NextHandler?.Handle(setupContext);
        }

        // Responsible for assigning available objects from the context object to the Singleton.
        private void AssignToSingletonManager(SceneSetupContext setupContext)
        {
            SceneManager.Instance.HitstopController = this.m_HitstopController;
            SceneManager.Instance.CameraController = setupContext.CameraController;
            SceneManager.Instance.LockOnObserver = setupContext.LockOnObserver;
            SceneManager.Instance.LockOnSystem = setupContext.LockOnSystem;
        }

        #endregion Methods
  
    }

}