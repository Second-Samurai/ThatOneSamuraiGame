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
            
            ResetAnimationBools();
            
            // Add the enemy to the stun list (will automatically remove it from the enemy list
            AISystem.enemyTracker.AddEnemy(AISystem.transform, true);
            // Start a new impatience countdown
            AISystem.enemyTracker.StartImpatienceCountdown();

            // Set the guard broken animator bool to true, to ideally play the animation
            AISystem.animator.SetBool("IsGuardBroken", true);
            AISystem.animator.SetTrigger("BreakGuard");

            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;

            yield return null;
            
            AISystem.animator.ResetTrigger("BreakGuard");
        }
    }
}
