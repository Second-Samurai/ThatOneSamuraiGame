using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public class CheckpointSetupHandler : MonoBehaviour, ISetupHandler
    {
        
        #region - - - - - - Fields - - - - - -

        private ISetupHandler m_NextHandler;

        #endregion Fields
        
        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle()
        {
            CheckpointManager _CheckpointManager = FindFirstObjectByType<CheckpointManager>();
            _CheckpointManager.Initialize();
            
            this.m_NextHandler?.Handle();
        }

        #endregion Methods

    }

}