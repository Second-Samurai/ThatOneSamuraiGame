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

            // Get the target object and current enemy transform
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            PositionTowardsTarget(AISystem.transform, _target);
            
            AISystem.animator.SetBool("IsLightAttacking", true);

            yield break;

            // NOTE: End state is called through an animation event in the light attack animation
        }

        public override void EndState()
        {
            AISystem.animator.SetBool("IsLightAttacking", false);
            
            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;

            if (InRange(AISystem.transform.position, _target, AISystem.enemySettings.followUpAttackRange))
            {
                AISystem.OnLightAttack(); // Light attack again if close enough
            }
            else
            {
                AISystem.OnApproachPlayer(); // Approach player if they are too far away
            }
        }
    }
}
