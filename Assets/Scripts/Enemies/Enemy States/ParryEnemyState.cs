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
            
            // Make the next attack unblockable
            //AISystem.bIsUnblockable = true;
            AISystem.swordEffects.EndBlockEffect();
            //AISystem.swordEffects.BeginUnblockableEffect();

            AISystem.parryEffects.PlayParry();
            int decision = Random.Range(0, 4);

            if (AISystem.enemyType == EnemyType.TUTORIALENEMY) //TUTORIAL ENEMIES CANNOT USE UNBLOCKABLE
                decision = 0;

            if (decision == 0 || decision == 1) // Normal Attack
            {
                // Set the attack trigger
                Animator.SetTrigger("TriggerLightAttack");
            }
            else if (decision == 2) // Thrust
            {
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
            
            else if (decision == 3)
            {
                Animator.SetTrigger("TriggerCounterAttack");

            }
            // Set the parry trigger
            
            yield break;
            
            // NOTE: End state is called through an animation event in the parry attack animation
        }
        
        public override void Tick()
        {
            // Get target position and face towards it
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            PositionTowardsTarget(AISystem.transform, _target);
        }

        // End state is called by animation event
        public override void EndState()
        {
            AISystem.attackIndicator.HideIndicator();
            // Ensure rotate to player is set back in end state
            bIsRotating = true;
            AISystem.EndUnblockable();
            // Restore future attacks to be blockable
            AISystem.bIsUnblockable = false;
           // AISystem.swordEffects.EndUnblockableEffect();
            
            ChooseActionUsingDistance(AISystem.enemySettings.GetTarget().position + AISystem.floatOffset);
        }
    }
}
