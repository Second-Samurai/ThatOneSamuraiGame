using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class CircleEnemyState : EnemyState
    {
        private Vector3 _target;
        private float _circleToChaseRange;

        //Class constructor
        public CircleEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            ResetAnimationBools();

            PickStrafeDirection();
            
            // Cache the range value so we're not always getting it in the tick function
            _circleToChaseRange = AISystem.enemySettings.circleToChaseRange;
            
            AISystem.animator.SetBool("IsStrafing", true);

            yield break;
        }
        
        public override void Tick()
        {
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            // Set the rotation of the enemy
            PositionTowardsTarget(AISystem.transform, _target);
            
            // Change to chase state when too far from the player
            if (!InRange(AISystem.transform.position, _target, _circleToChaseRange))
            {
                Debug.Log("Called");
                EndState();
            }
        }

        public override void EndState()
        {
            AISystem.animator.SetBool("IsStrafing", false);
            
            AISystem.animator.SetFloat("StrafeDirectionX", 0);
            
            AISystem.OnApproachPlayer();
        }

        // Set the strafe direction
        private void PickStrafeDirection()
        {
            // Random.Range is non-inclusive for it's max value for ints
            if (Random.Range(0, 2) == 0)
            {
                AISystem.animator.SetFloat("StrafeDirectionX", -1.0f);
            }
            else
            {
                AISystem.animator.SetFloat("StrafeDirectionX", 1.0f);
            }
        }
    }
}
