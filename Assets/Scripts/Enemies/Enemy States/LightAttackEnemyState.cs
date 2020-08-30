using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class LightAttackEnemyState : EnemyState
    {
        private Vector3 _target;
        private Transform _transform;

        //Class constructor
        public LightAttackEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Get the target object and current enemy transform
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            _transform = AISystem.transform;
            
            PositionTowardsTarget(_transform, _target);

            AISystem.animator.SetBool("IsLightAttacking", true);
            
            yield return new WaitForSeconds(AISystem.GetAnimationLength("SwordsmanLightAttack"));

            EndState();
        }

        public override void EndState()
        {
            AISystem.animator.SetBool("IsLightAttacking", false);
            
            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;

            if (InRange(_transform.position, _target, AISystem.enemySettings.followUpAttackRange))
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
