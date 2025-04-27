using UnityEngine;
using UnityEngine.Events;

// TODO: Not 100% on whether we still need this
public class EnemyControlObserver : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public UnityEvent OnEnemyStart = new(); // TODO: Not 100% on whether we still need this
    public UnityEvent OnEnemyStop = new();
    public UnityEvent<GameObject> OnEnemyDeath = new();

    #endregion Fields

}
