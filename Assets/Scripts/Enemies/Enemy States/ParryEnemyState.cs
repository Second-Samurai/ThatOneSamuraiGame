using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class ParryEnemyState : EnemyState
    {
        private Vector3 _target;

        //Class constructor
        public ParryEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            AISystem.attackIndicator.ShowIndicator();
            // Disable canParry
            AISystem.eDamageController.enemyGuard.canParry = false;
            
            // Make the next attack unblockable
            AISystem.bIsUnblockable = true;
            AISystem.swordEffects.EndBlockEffect();
            AISystem.swordEffects.BeginUnblockableEffect();

            AISystem.parryEffects.PlayParry();
            
            // Set the parry trigger
            Animator.SetTrigger("TriggerCounterAttack");
            
            yield break;
            
            // NOTE: End state is called through an animation event in the parry attack animation
        }
        
        public override void Tick()
        {
            // Get target position and face towards it
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            PositionTowardsTarget(AISystem.transform, _target);
        }

        // End state is called by animation event
        public override void EndState()
        {
            AISystem.attackIndicator.HideIndicator();
            
            // Restore future attacks to be blockable
            AISystem.bIsUnblockable = false;
            AISystem.swordEffects.EndUnblockableEffect();
            
            ChooseActionUsingDistance(AISystem.enemySettings.GetTarget().position + AISystem.floatOffset);
        }
    }
}
