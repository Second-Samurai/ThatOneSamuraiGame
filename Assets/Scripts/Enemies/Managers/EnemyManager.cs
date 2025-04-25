using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public static EnemyManager Instance;
    
    private SceneEnemyController m_ActiveSceneEnemyControllers;

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public SceneEnemyController SceneEnemyController
        => this.m_ActiveSceneEnemyControllers;

    #endregion Properties
  
    #region - - - - - - Unity Methods - - - - - -

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    public void SetActiveSceneEnemyController(SceneEnemyController sceneEnemyController) 
        => this.m_ActiveSceneEnemyControllers = sceneEnemyController;

    #endregion Methods
  
}
