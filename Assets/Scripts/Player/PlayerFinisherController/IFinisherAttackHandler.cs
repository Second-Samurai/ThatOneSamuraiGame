using UnityEngine;

public interface IFinisherAttackHandler
{

    #region - - - - - - Methods - - - - - -

    void RunFinishingAttack(GameObject targetEnemy = null);

    void SetFinishingTargetEnemy(GameObject targetEnemy);

    void EnableFinisherAttack();

    void DisableFinisherAttack();

    #endregion Methods

}