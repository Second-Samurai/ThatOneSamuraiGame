using UnityEngine;

public interface ILockOnSystem
{

    #region - - - - - - Methods - - - - - -

    void RemoveTargetFromTracking(Transform targetToRemove); //TODO replace all instances calling upon the death or removal of target from list.

    void SelectNewTarget();

    void StartLockOn();

    void EndLockOn();

    #endregion Methods

}