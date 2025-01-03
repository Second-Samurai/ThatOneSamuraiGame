using UnityEngine;
using UnityEngine.Events;

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
