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
            
            // Stop unblockable if enemy was previously doing an unblockable attack and play block effect
            AISystem.EndUnblockable();
            AISystem.swordEffects.BeginBlockEffect();

            // Set the parry stun trigger
            Animator.SetTrigger("TriggerQuickBlock");

            yield break;

            // NOTE: End state is called through an animation event in the quick block animation
        }
        
        

        public override void EndState()
        {
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            // Stop the block effect
            AISystem.swordEffects.EndBlockEffect();

            // Move to block state OR choose an action using distance
            int decision = Random.Range(0, 3);
            if (decision == 0)
            {
                ChooseActionUsingDistance(_target);
            }
            else
            {
                AISystem.OnBlock();
            }
        }
    }
}
