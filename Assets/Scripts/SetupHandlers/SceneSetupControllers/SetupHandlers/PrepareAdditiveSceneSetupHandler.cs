using ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers;
using UnityEngine;

public class PrepareAdditiveSceneSetupHandler : MonoBehaviour, ISetupHandler
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
        // Validate required dependencies
        GameValidator.NotNull(this.m_SceneEnemyController, nameof(m_SceneEnemyController));
        
        EnemyManager.Instance.SetActiveSceneEnemyController(this.m_SceneEnemyController);
        
        this.m_NextHandler?.Handle(setupContext);
    }

    #endregion Methods
  
}
