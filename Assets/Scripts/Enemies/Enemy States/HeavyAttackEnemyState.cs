using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class HeavyAttackEnemyState : EnemyState
    {
        private Vector3 _target;

        //Class constructor
        public HeavyAttackEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            //ResetAnimationBools();

            AISystem.attackIndicator.ShowIndicator();
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            AISystem.bIsUnblockable = true;

            // Set the attack trigger
            Animator.SetTrigger("TriggerHeavyAttack");

            // Rotate towards player
            bIsRotating = true;

            yield break;

            // NOTE: End state is called through an animation event in the light attack animation
            // NOTE: An animation event triggers the enemy to scoot forwards if the player is too far away
        }

        public override void Tick()
        {
            // bIsRotating is set to false through an animation event
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
            AISystem.attackIndicator.HideIndicator();
            AISystem.bIsUnblockable = false;
            // Ensure rotate to player is set back in end state
            bIsRotating = true;

            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;

            // In enemy state, choose a following action based on player distance
            ChooseActionUsingDistance(_target);
        }
    }
}
