using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class DodgeEnemyState : EnemyState
    {
        //Class constructor
        public DodgeEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            // Set dodge direction then play animation
            AISystem.animator.SetFloat("DodgeDirectionX", AISystem.dodgeDirectionX);
            AISystem.animator.SetFloat("DodgeDirectionZ", AISystem.dodgeDirectionZ);
            AISystem.animator.SetBool("IsDodging", true);
            
            AISystem.DodgeImpulse(new Vector3(AISystem.dodgeDirectionX, 0, AISystem.dodgeDirectionZ),
                AISystem.enemySettings.GetEnemyStatType(AISystem.enemyType).dodgeForce);

            yield break;
            
            // NOTE: End state is called through an animation event in the light attack animation
        }

        public override void EndState()
        {
            AISystem.animator.SetBool("IsDodging", false);
            
            // Reset the dodge direction
            AISystem.dodgeDirectionX = 0;
            AISystem.dodgeDirectionZ = 0;
            
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            Vector3 target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            ChooseActionUsingDistance(target);
        }
    }
}
