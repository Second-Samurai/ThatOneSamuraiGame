using System.Collections.Generic;
using System.Linq;
using ThatOneSamuraiGame.Scripts.DebugScripts.DebugSupport;
using ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers
{

    public class SceneSetupController : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [FormerlySerializedAs("m_SetupHandlers")] [SerializeField] private List<GameObject> m_SetupHandlersObjects;

        #endregion Fields

        #region - - - - - - Unity Lifecycle Methods - - - - - -

        private void Awake()
        {
            // -------------------------------------------------------------------------------
            // During debug, setup invocation is invoked from the DebugStartup services.
            // -------------------------------------------------------------------------------
            DebugGameSetupSupport _GameSetupSupport = Object.FindFirstObjectByType<DebugGameSetupSupport>();
            DebugSceneSetupSupport _SceneSetupSupport = Object.FindFirstObjectByType<DebugSceneSetupSupport>();
            if (_GameSetupSupport != null && _GameSetupSupport.IN_DEVELOPMENT
                || _SceneSetupSupport != null && _SceneSetupSupport.IsSceneInDevelopment)
                return;

            this.RunSetup();
        }

        #endregion Unity Lifecycle Methods
  
        #region - - - - - - Methods - - - - - -

        public void RunSetup()
        {
            if (this.m_SetupHandlersObjects == null 
                || !this.m_SetupHandlersObjects.FirstOrDefault()
                || this.m_SetupHandlersObjects.Count == 0)
            {
                Debug.LogError("[ERROR]: There are no handlers configured for this scene.");
                return;
            }

            List<ISetupHandler> _SetupHandlers = this.m_SetupHandlersObjects
                .Select(sh => sh.GetComponent<ISetupHandler>())
                .ToList();
            
            // Configure the handler chain for sequential invocation.
            for (int i = 0; i < _SetupHandlers.Count - 1; i++)
            {
                ISetupHandler _Handler = _SetupHandlers[i];
                _Handler.SetNext(_SetupHandlers[i + 1]);
            }
            
            // Initiate the chain invocation.
            _SetupHandlers.First().Handle();
        }

        #endregion Methods
  
    }

}