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

            // Jump forward if the player is too far away
            if (!InRange(AISystem.transform.position, _target, AISystem.enemySettings.followUpAttackRange))
            {
                DodgeImpulse(_target.normalized, AISystem.enemySettings.dodgeForce);
            }

            AISystem.animator.SetBool("IsLightAttacking", true);

            yield break;

            // NOTE: End state is called through an animation event in the light attack animation
        }
        
        public override void Tick()
        {
            // Get target position and face towards it
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            PositionTowardsTarget(AISystem.transform, _target);
        }

        public override void EndState()
        {
            AISystem.animator.SetBool("IsLightAttacking", false);
            
            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            // In enemy state, choose a following action based on player distance
            ChooseActionUsingDistance(_target);
        }
    }
}
