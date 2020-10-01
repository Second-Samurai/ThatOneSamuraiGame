using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class DodgeEnemyState : EnemyState
    {
        private Animator _animator;
        
        //Class constructor
        public DodgeEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            // Set the dodge trigger
            // NOTE: Dodge direction is based on MovementX and MovementZ
            // These variables are set before TriggerDodge is called
            _animator = AISystem.animator;
            _animator.SetTrigger("TriggerDodge");
            
            // Perform a dodge using the enemy dodge force and movement variables
            AISystem.DodgeImpulse(new Vector3(
                    _animator.GetFloat("MovementX"), 
                    0, 
                    _animator.GetFloat("MovementZ")), 
                AISystem.enemySettings.GetEnemyStatType(AISystem.enemyType).dodgeForce);

            // Reset trigger after frame has passed
            yield return null;
            _animator.ResetTrigger("TriggerDodge");
            
            // NOTE: End state is called through an animation event in the dodge animation
        }

        public override void EndState()
        {
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            Vector3 target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            ChooseActionUsingDistance(target);
        }
    }
}
