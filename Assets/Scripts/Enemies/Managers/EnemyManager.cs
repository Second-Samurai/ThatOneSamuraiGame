using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public static EnemyManager Instance;
        
    public SceneEnemyController SceneEnemyController;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    #endregion Unity Methods
  
}
