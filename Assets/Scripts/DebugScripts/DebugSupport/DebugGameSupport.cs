using ThatOneSamuraiGame.Scripts.SetupHandlers.GameSetupHandlers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.DebugScripts.DebugSupport
{

    public class DebugGameSupport : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        // Debug Flag
        public bool IN_DEVELOPMENT = false;

        #endregion Fields

        #region - - - - - - Unity Lifecycle - - - - - -

        /// <remarks>
        /// This class should only be awake if constructed after the debug 'game scene' is constructed.
        /// </remarks>
        private void Awake()
        {
            if (!Object.FindAnyObjectByType<DebugSceneStartupSupport>()) return;
            
            // Add all debug services and handlers requiring initialisation
            DebugSceneStartupSupport _SceneStartupSupport = Object.FindFirstObjectByType<DebugSceneStartupSupport>();
            GameSetupHandler _GameSetupHandler = Object.FindFirstObjectByType<GameSetupHandler>();
            if (_SceneStartupSupport != null) 
                _GameSetupHandler.OnGameSetupCompletion.AddListener(_SceneStartupSupport.ActivateSceneObjects);
            
            this.IN_DEVELOPMENT = _SceneStartupSupport.IsSceneInDevelopment;
        }

        #endregion Unity Lifecycle

    }

}