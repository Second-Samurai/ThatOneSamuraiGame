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
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            _transform = AISystem.transform;
            
            PositionTowardsPlayer(_transform, _target);

            AISystem.SetLightAttacking(true);
            yield return new WaitForSeconds(AISystem.GetAnimationLength("SwordsmanLightAttack"));

            EndState();
        }

        public override void EndState()
        {
            AISystem.SetLightAttacking(false);
            
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
