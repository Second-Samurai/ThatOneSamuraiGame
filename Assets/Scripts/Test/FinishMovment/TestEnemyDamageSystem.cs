using System;
using ThatOneSamuraiGame.Scripts.Base;
using UnityEngine;

public class TestEnemyDamageSystem : PausableMonoBehaviour, IDamageable
{

    #region - - - - - - Fields - - - - - -

    public bool CanTriggerFinishMovement;

    #endregion Fields
  
    #region - - - - - - Methods - - - - - -

    public EntityType GetEntityType()
        => EntityType.Enemy;

    public bool CheckCanDamage() 
        => throw new NotImplementedException();

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        if (this.CanTriggerFinishMovement)
        {
            IFinisherController _FinisherController = attacker.GetComponentInChildren<PlayerFinisherController>();
            _FinisherController.SetFinishingTargetEnemy(this.transform);
            _FinisherController.StartFinishingAction();
        }
    }

    public void DisableDamage() 
        => throw new NotImplementedException();

    public void EnableDamage() 
        => throw new NotImplementedException();

    #endregion Methods
  
}
