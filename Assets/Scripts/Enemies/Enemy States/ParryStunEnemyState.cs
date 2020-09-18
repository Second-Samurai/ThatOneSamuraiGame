using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class ParryStunEnemyState : EnemyState
    {
        //Class constructor
        public ParryStunEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            //NOTE: Damage handling can be found in EDamageController
            
            ResetAnimationBools();
            
            AISystem.animator.SetTrigger("Parried");
            
            yield break;
        }
        
        // End state is called through an animation event at the end of the animation
        public override void EndState()
        {
            AISystem.animator.ResetTrigger("Parried");
            
            if(AISystem.enemyType == EnemyType.TUTORIALENEMY)
            {
                AISystem.dodgeDirectionZ = -1;
                AISystem.OnDodge();
            }
            else 
            {
                // In enemy state, choose a following action based on player distance
                ChooseActionUsingDistance(AISystem.enemySettings.GetTarget().position + AISystem.floatOffset);
            }
            
            
        }
    }
}
