using UnityEngine;
using UnityEngine.Events;

public class EnemyStateObserver : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public UnityEvent OnEnemyDeath = new();

    #endregion Fields

}
