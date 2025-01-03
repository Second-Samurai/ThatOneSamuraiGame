
using UnityEngine;

public interface ILockOnSystem
{

    #region - - - - - - Methods - - - - - -

    GameObject GetCurrentTarget();

    void SelectNewTarget();

    void StartLockOn();

    void EndLockOn();

    #endregion Methods

}