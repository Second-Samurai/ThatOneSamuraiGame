using System.Collections;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.General.Services;
using ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers;
using UnityEngine;

public class GraduallyLoadEnemiesHandler : MonoBehaviour, ISetupHandler
{

    #region - - - - - - Fields - - - - - -

    [SerializeField, RequiredField] private SceneEnemyController m_SceneEnemyController;
    
    private ISetupHandler m_NextHandler;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    public void SetNext(ISetupHandler setupHandler) 
        => this.m_NextHandler = setupHandler;

    public void Handle(SceneSetupContext setupContext)
    {
        this.m_SceneEnemyController.CollectAllEnemiesWithinScene();
        this.StartCoroutine(this.HandleEnemyLoad());
        GameLogger.Log("Scene enemies are loaded");
        
        this.m_NextHandler?.Handle(setupContext);
    }

    private IEnumerator HandleEnemyLoad()
    {
        yield return this.PreBuildAllEnemyRigs();
        yield return this.ActivateSceneEnemyController();
    }

    private IEnumerator PreBuildAllEnemyRigs()
    {
        for (int i = 0; i < this.m_SceneEnemyController.SceneEnemyObjects.Count; i++)
        {
            IAnimationRigBuilder _Builder = this.m_SceneEnemyController.SceneEnemyObjects[i].GetComponent<IAnimationRigBuilder>();
            _Builder?.BuildRig();
            yield return null;
        }
    }
    
    private IEnumerator ActivateSceneEnemyController()
    {
        for (int i = 0; i < this.m_SceneEnemyController.SceneEnemyObjects.Count; i++)
        {
            this.m_SceneEnemyController.SceneEnemyObjects[i].SetActive(true);
            ICommand _SetupCommand = this.m_SceneEnemyController.SceneEnemyObjects[i].GetComponent<ICommand>();
            _SetupCommand.Execute();
            yield return null;
        }
    }

    #endregion Methods
  
}
