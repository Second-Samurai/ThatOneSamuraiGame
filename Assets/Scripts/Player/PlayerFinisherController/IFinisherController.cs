using UnityEngine;

public interface IFinisherController
{

    #region - - - - - - Methods - - - - - -

    void RunFinishingAttack(GameObject targetEnemy = null);

    void SetFinishingTargetEnemy(GameObject targetEnemy);

    bool C { get; }

    #endregion Methods

}