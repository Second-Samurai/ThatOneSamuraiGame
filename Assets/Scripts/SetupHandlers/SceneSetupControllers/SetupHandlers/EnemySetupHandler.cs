using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public class EnemySetupHandler : MonoBehaviour, ISetupHandler
    {

        #region - - - - - - Fielkds - - - - - -

        private ISetupHandler m_NextHandler;

        #endregion Fielkds

        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle()
        {
            EnemyTracker _EnemyTracker = FindFirstObjectByType<EnemyTracker>();
            if (_EnemyTracker == null)
                Debug.LogWarning("[WARNING]: No EnemyTracker is found in scene.");

            SceneManager.Instance.m_EnemyTracker = _EnemyTracker;
            GameManager.instance.gameSettings.enemySettings.SetTarget(FindFirstObjectByType<PlayerController>().transform);
            
            print("[LOG]: Completed Scene Enemy setup.");
            this.m_NextHandler?.Handle();
        }

        #endregion Methods
  
    }

}