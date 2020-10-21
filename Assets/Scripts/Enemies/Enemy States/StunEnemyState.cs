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

            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            // Stop unblockable if enemy was previously doing an unblockable attack
            AISystem.EndUnblockable();
            
            // Trigger the guard break
            Animator.SetTrigger("TriggerGuardBreak");

            yield break;
        }
    }
}
