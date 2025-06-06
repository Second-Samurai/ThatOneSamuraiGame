﻿using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
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

        void ISetupHandler.Handle(SceneSetupContext setupContext)
        {
            CheckpointManager _CheckpointManager = FindFirstObjectByType<CheckpointManager>();
            _CheckpointManager.Initialize();

            SceneManager.Instance.CheckpointManager = _CheckpointManager;
            
            this.m_NextHandler?.Handle(setupContext);
        }

        #endregion Methods

    }

}