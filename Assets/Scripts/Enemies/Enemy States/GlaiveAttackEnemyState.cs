using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class GlaiveAttackEnemyState : EnemyState
    {
        private Vector3 _target;

        //Class constructor
        public GlaiveAttackEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            if (AISystem.enemyType == EnemyType.BOSS)
            {
                AISystem.eDamageController.enemyGuard.bSuperArmour = true;
                AISystem.glaiveMesh.enabled = true;
            }
            //ResetAnimationBools();
            AISystem.swordEffects.BeginUnblockableEffect();
            AISystem.attackIndicator.ShowIndicator();
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            AISystem.bIsUnblockable = true;
            int decision = Random.Range(0, 2);
            if (decision == 0) // Normal Attack
            {
                // Set the attack trigger
                Animator.SetTrigger("TriggerHeavyAttack"); 
            }
            else // Combo
            {
                Animator.SetTrigger("TriggerHeavyCombo");
            }
           
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
            if (AISystem.enemyType == EnemyType.BOSS)
            {
                AISystem.eDamageController.enemyGuard.bSuperArmour = false;
                AISystem.glaiveMesh.enabled = false;
            }
            AISystem.swordEffects.EndUnblockableEffect();
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
