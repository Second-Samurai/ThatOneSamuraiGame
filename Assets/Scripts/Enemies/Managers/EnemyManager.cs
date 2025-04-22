using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public static EnemyManager Instance;

    // TODO: Change to use getter-only properties
    // TODO: Should be managed by the scene area level
    public EnemyControlObserver EnemyObserver;
    
    // TODO: Move to seperate controller watching over all entities
    public List<GameObject> m_SceneEnemyObjects;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        GameValidator.NotNull(this.EnemyObserver, nameof(EnemyObserver));
        
        // TODO: Move to pipeline
        for (int i = 0; i < this.m_SceneEnemyObjects.Count; i++)
            this.m_SceneEnemyObjects[i].GetComponent<ArcherInitializationCommand>().Initialize();
    }

    #endregion Unity Methods
  
}
