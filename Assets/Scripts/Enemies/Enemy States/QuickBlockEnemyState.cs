using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class QuickBlockEnemyState : EnemyState
    {
        private Vector3 _target;

        //Class constructor
        public QuickBlockEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            ResetAnimationBools();
            
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;

            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            PositionTowardsTarget(AISystem.transform, _target);
            
            AISystem.animator.SetBool("IsQuickBlocking", true);
            
            yield break;
            
            // NOTE: End state is called through an animation event in the light attack animation
        }

        public override void EndState()
        {
            AISystem.animator.SetBool("IsQuickBlocking", false);
            
            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            // TODO: Remove this. Temporary state switch until blocking is introduced
            ChooseActionUsingDistance(_target);
            
            // TODO: Replace with this
            //AISystem.OnBlock();
        }
    }
}
