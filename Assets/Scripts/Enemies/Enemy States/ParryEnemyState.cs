using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class ParryEnemyState : EnemyState
    {
        private Vector3 _target;

        //Class constructor
        public ParryEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Get target position and face towards it
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            PositionTowardsTarget(AISystem.transform, _target);
            
            // Disable canParry
            AISystem.eDamageController.enemyGuard.canParry = false;
            
            // Disable the block animation, start the parry animation
            AISystem.animator.SetBool("IsBlocking", false);
            AISystem.animator.SetBool("IsParried", true);

            yield break;
        }

        // End state is called by animation event
        public override void EndState()
        {
            AISystem.animator.SetBool("IsParried", false);

            AISystem.dodgeDirectionZ = -1.0f;
            AISystem.OnDodge();
        }
    }
}
