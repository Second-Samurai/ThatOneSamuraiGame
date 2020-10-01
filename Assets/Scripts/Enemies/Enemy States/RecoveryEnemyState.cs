using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class RecoveryEnemyState : EnemyState
    {
        //Class constructor
        public RecoveryEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Set the parry stun trigger
            Animator.SetTrigger("TriggerRecovery");
            
            // Reset the trigger after a frame has passed
            yield return null;
            Animator.ResetTrigger("TriggerRecovery");
            
            // NOTE: EndState is called through an animation event in the recovery animation
        }
        
        // End state is called at the end of the recovery animation through animation events
        public override void EndState()
        {
            ChooseActionUsingDistance(AISystem.enemySettings.GetTarget().position + AISystem.floatOffset);
        }
    }
}
