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

            AISystem.animator.SetBool("IsQuickBlocking", true);
            
            yield break;
            
            // NOTE: End state is called through an animation event in the light attack animation
        }

        public override void EndState()
        {
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            AISystem.animator.SetBool("IsQuickBlocking", false);

            // Move to block state OR choose an action using distance
            int decision = Random.Range(0, 4);
            if (decision == 0)
            {
                AISystem.OnBlock();
            }
            else
            {
                ChooseActionUsingDistance(_target);
            }
        }
    }
}
