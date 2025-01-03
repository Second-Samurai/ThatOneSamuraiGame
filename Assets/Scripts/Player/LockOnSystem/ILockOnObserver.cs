using UnityEngine;
using UnityEngine.Events;

public interface ILockOnObserver
{

    #region - - - - - - Properties - - - - - -

    UnityEvent OnLockOnEnable { get; }

    UnityEvent<Transform> OnNewLockOnTarget { get; }

    UnityEvent OnLockOnDisable { get; }

    #endregion Properties

}