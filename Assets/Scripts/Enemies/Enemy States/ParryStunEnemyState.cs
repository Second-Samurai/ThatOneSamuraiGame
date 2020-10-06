using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class ParryStunEnemyState : EnemyState
    {
        //Class constructor
        public ParryStunEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            //NOTE: Damage handling can be found in EDamageController
            
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            // Set the parry stun trigger
            Animator.SetTrigger("TriggerParryStun");
            
            yield break;
        }
        
        // End state is called through an animation event at the end of the animation
        public override void EndState()
        {
            if(AISystem.enemyType == EnemyType.TUTORIALENEMY)
            {
                // Dodge direction is set in the state before OnDodge is called
                // This is so we can choose a dodge direction based on the previous state
                Animator.SetFloat("MovementZ", -1);
                AISystem.OnDodge();
            }
            else 
            {
                // In enemy state, choose a following action based on player distance
                ChooseActionUsingDistance(AISystem.enemySettings.GetTarget().position + AISystem.floatOffset);
            }
            
            
        }
    }
}
