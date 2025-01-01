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

public class LockOnObserver : MonoBehaviour, ILockOnObserver
{

    #region - - - - - - Fields - - - - - -

    private readonly UnityEvent m_OnLockOnEnable = new();
    private readonly UnityEvent<Transform> m_OnNewLockOnTarget = new();
    private readonly UnityEvent m_OnLockOnDisable = new();

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public UnityEvent OnLockOnEnable
        => this.m_OnLockOnEnable;
    
    public UnityEvent<Transform> OnNewLockOnTarget
        => this.m_OnNewLockOnTarget;

    public UnityEvent OnLockOnDisable
        => this.m_OnLockOnDisable;

    #endregion Properties

}
