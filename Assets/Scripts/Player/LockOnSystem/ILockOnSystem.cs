
using UnityEngine;

public interface ILockOnSystem
{

    #region - - - - - - Properties - - - - - -

    public bool IsLockingOnTarget { get; }

    #endregion Properties
  
    #region - - - - - - Methods - - - - - -

    GameObject GetCurrentTarget();

    void SelectNewTarget();

    void StartLockOn();

    void EndLockOn();

    #endregion Methods

}