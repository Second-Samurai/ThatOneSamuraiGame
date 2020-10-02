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
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;

            // Set the parry stun trigger
            Animator.SetTrigger("TriggerQuickBlock");

            yield break;

            // NOTE: End state is called through an animation event in the quick block animation
        }

        public override void EndState()
        {
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;

            // Move to block state OR choose an action using distance
            int decision = Random.Range(0, 2);
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
