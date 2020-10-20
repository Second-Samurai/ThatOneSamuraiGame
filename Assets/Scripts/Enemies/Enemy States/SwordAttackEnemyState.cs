using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class SwordAttackEnemyState : EnemyState
    {
        private Vector3 _target;

        //Class constructor
        public SwordAttackEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            //ResetAnimationBools();

            AISystem.attackIndicator.ShowIndicator();
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            int decision = Random.Range(0, 2);
            
            if (AISystem.enemyType == EnemyType.TUTORIALENEMY || AISystem.enemyType == EnemyType.GLAIVEWIELDER || AISystem.enemyType == EnemyType.BOSS) //TUTORIAL ENEMIES CANNOT USE UNBLOCKABLE
                decision = 0;
            
            if (decision == 0) // Normal Attack
            {
                if(AISystem.enemyType == EnemyType.BOSS)
                {
                    int selector = Random.Range(0, 5);

                    if(selector == AISystem.bossAttackSelector)
                    {
                        selector++;
                        if(selector >= 5)
                        {
                            selector = 0;
                        }
                    }

                    AISystem.bossAttackSelector = selector;
                    selector = 4;
                    Animator.SetInteger("AttackSelector", selector);
                }
                // Set the attack trigger
                Animator.SetTrigger("TriggerLightAttack");
            }
            else // Thrust
            {
                Animator.SetTrigger("TriggerThrust");
                AISystem.BeginUnblockable();
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
            AISystem.attackIndicator.HideIndicator();
            // Ensure rotate to player is set back in end state
            bIsRotating = true;
            AISystem.EndUnblockable();

            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;

            // In enemy state, choose a following action based on player distance
            ChooseActionUsingDistance(_target);
        }
    }
}
