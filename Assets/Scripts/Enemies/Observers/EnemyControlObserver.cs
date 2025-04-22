using UnityEngine;
using UnityEngine.Events;

public class EnemyControlObserver : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    // TODO: Come up with a better name for the event
    public UnityEvent OnEnemyStart = new();

    // TODO: Come up with a better name for the event
    public UnityEvent OnEnemyStop = new();

    #endregion Fields

}
