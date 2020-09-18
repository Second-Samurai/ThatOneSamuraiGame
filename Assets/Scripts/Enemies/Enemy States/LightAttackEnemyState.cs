using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class LightAttackEnemyState : EnemyState
    {
        private Vector3 _target;

        //Class constructor
        public LightAttackEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;

            AISystem.animator.SetBool("IsLightAttacking", true);
            
            // Rotate towards player
            bIsRotating = true;

            yield break;

            // NOTE: End state is called through an animation event in the light attack animation
            // NOTE: An animation event all triggers the enemy to jump forwards if the player is too far away
        }
        
        public override void Tick()
        {
            // bISRotating is set to false through an animation event
            // This is so the enemy stops rotating while they strike
            if (bIsRotating)
            {
                // Get target position and face towards it
                _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
                PositionTowardsTarget(AISystem.transform, _target);
            }
        }

        public override void EndState()
        {
            // Ensure rotate to player is set back in end state
            bIsRotating = true;
            
            AISystem.animator.SetBool("IsLightAttacking", false);
            
            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            // In enemy state, choose a following action based on player distance
            ChooseActionUsingDistance(_target);
        }
        
        
    }
}
