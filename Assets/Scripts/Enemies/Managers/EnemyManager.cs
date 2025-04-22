using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public static EnemyManager Instance;

    // TODO: Change to use getter-only properties
    // TODO: Should be managed by the scene area level
    public EnemyControlObserver EnemyObserver;

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
    }

    #endregion Unity Methods
  
}
