using ThatOneSamuraiGame.Scripts.SetupHandlers.GameSetupHandlers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.DebugScripts.DebugSupport
{

    public class DebugGameSetupSupport : MonoBehaviour
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
            if (!Object.FindAnyObjectByType<DebugSceneSetupSupport>()) return;
            
            // Add all debug services and handlers requiring initialisation
            DebugSceneSetupSupport sceneSetupSupport = Object.FindFirstObjectByType<DebugSceneSetupSupport>();
            GameSetupHandler _GameSetupHandler = Object.FindFirstObjectByType<GameSetupHandler>();
            if (sceneSetupSupport != null) 
                _GameSetupHandler.OnGameSetupCompletion.AddListener(sceneSetupSupport.ActivateSceneObjects);
            
            this.IN_DEVELOPMENT = sceneSetupSupport.IsSceneInDevelopment;
        }

        #endregion Unity Lifecycle

    }

}