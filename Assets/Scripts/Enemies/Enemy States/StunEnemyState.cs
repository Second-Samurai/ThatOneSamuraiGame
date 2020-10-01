using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class StunEnemyState : EnemyState
    {
        //Class constructor
        public StunEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            //NOTE: A majority of the guard broken behaviour is handled in Guarding and EDamageController

            // Start a new impatience countdown
            AISystem.enemyTracker.StartImpatienceCountdown();
            
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            // Trigger the guard break
            Animator.SetTrigger("TriggerGuardBreak");

            // Reset the trigger after a frame has passed
            yield return null;
            Animator.ResetTrigger("TriggerGuardBreak");
        }
    }
}
