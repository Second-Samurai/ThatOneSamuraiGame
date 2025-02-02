using UnityEngine;

public interface IFinisherController
{

    #region - - - - - - Methods - - - - - -

    void StartFinishingAction();

    void SetFinishingTargetEnemy(Transform targetEnemyTransform);

    #endregion Methods

}