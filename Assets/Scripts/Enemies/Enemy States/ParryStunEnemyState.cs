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
            Debug.Log("Parry end state called");
            AISystem.animator.ResetTrigger("Parried");
            
            // In enemy state, choose a following action based on player distance
            ChooseActionUsingDistance(AISystem.enemySettings.GetTarget().position + AISystem.floatOffset);
        }
    }
}
