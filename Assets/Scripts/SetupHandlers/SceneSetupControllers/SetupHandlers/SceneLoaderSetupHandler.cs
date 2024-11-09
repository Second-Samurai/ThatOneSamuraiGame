using System.Collections.Generic;
using System.Linq;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public class SceneLoaderSetupHandler : MonoBehaviour, ISetupHandler
    {

        #region - - - - - - Fields - - - - - -

        private ISetupHandler m_NextHandler;

        #endregion Fields
  
        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle()
        {
            Transform _MainCamera = SceneManager.Instance.MainCamera.transform;
            
            List<ISceneLoader> _SceneLoaders = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                .Where(sho => sho.GetComponent<ISceneLoader>() != null)
                .Select(sho => sho.GetComponent<ISceneLoader>())
                .ToList();

            if (_SceneLoaders.Count != 0)
                foreach (ISceneLoader _SceneLoader in _SceneLoaders)
                    _SceneLoader.Initialise(_MainCamera.transform);
            
            this.m_NextHandler?.Handle();
        }

        #endregion Methods
  
    }

}