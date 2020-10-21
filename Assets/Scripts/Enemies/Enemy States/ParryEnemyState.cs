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
            AISystem.attackIndicator.ShowIndicator();
            
            // Disable canParry
            AISystem.eDamageController.enemyGuard.canParry = false;
            
            // Stop the enemy block sword effect and play the parry effect
            AISystem.swordEffects.EndBlockEffect();
            AISystem.parryEffects.PlayParry();
            
            // Make a decision to determine the next move
            int decision = Random.Range(0, 4);

            if (AISystem.enemyType == EnemyType.TUTORIALENEMY) //TUTORIAL ENEMIES CANNOT USE UNBLOCKABLE
                decision = 0;
            if (AISystem.enemyType == EnemyType.BOSS)
            {
                int selector = Random.Range(0, 5);

                if (selector == AISystem.bossAttackSelector)
                {
                    selector++;
                    if (selector >= 5)
                    {
                        selector = 0;
                    }
                }

                AISystem.bossAttackSelector = selector;
                Animator.SetInteger("AttackSelector", selector);
                decision = 0;
            }

            if (decision == 0 || decision == 1) // Normal Attack
            {
                //Increase the speed of the next attack
                if(AISystem.enemyType != EnemyType.BOSS) AISystem.IncreaseAttackSpeed(0.3f);
                
                // Set the attack trigger
                Animator.SetTrigger("TriggerLightAttack");
            }
            else if (decision == 2) // Heavy attack
            {
                //Increase the speed of the next attack
                AISystem.IncreaseAttackSpeed(0.4f);
                
                if (AISystem.enemyType == EnemyType.GLAIVEWIELDER)
                {
                    Animator.SetTrigger("HeavyAttack");
                    AISystem.BeginUnblockable();
                }
                else
                {
                    Animator.SetTrigger("TriggerThrust");
                    AISystem.BeginUnblockable();
                }
            }
            
            else if (decision == 3) // Counter attack
            {
                Animator.SetTrigger("TriggerCounterAttack");

            }

            yield break;
            
            // NOTE: End state is called through an animation event at the end of the attack animation
        }
        
        public override void Tick()
        {
            if (bIsRotating)
            {
                // Get target position and face towards it
                _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
                PositionTowardsTarget(AISystem.transform, _target);
            }
        }

        // End state is called by animation event
        public override void EndState()
        {
            //Return to the previous speed for future attacks
            AISystem.ReturnPreviousAttackSpeed();
            
            AISystem.attackIndicator.HideIndicator();
            
            // Ensure rotate to player is set back in end state
            bIsRotating = true;
            
            // End the unblockable sword and effect
            AISystem.EndUnblockable();

            ChooseActionUsingDistance(AISystem.enemySettings.GetTarget().position + AISystem.floatOffset);
        }
    }
}
