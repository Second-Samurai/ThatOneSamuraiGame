using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public class EnemySetupHandler : MonoBehaviour, ISetupHandler
    {

        #region - - - - - - Fielkds - - - - - -

        [SerializeField] private EnemyTracker m_EnemyTracker;

        private ISetupHandler m_NextHandler;

        #endregion Fielkds

        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle(SceneSetupContext setupContext)
        {
            _ = GameValidator.NotNull(this.m_EnemyTracker, nameof(this.m_EnemyTracker));

            SceneManager.Instance.EnemyTracker = this.m_EnemyTracker;
            GameManager.instance.gameSettings.enemySettings.SetTarget(FindFirstObjectByType<PlayerController>().transform);
            
            // Initialize Enemy Tracker
            this.m_EnemyTracker.Initialize(new EnemyTrackerInitializerData(setupContext.LockOnSystem));
            
            print("[LOG]: Completed Scene Enemy setup.");
            this.m_NextHandler?.Handle(setupContext);
        }

        #endregion Methods
  
    }

}