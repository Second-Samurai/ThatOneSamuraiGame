using System.Collections;
using Enemy_Scripts;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class CloseDistanceEnemyState : EnemyState
    {
        private Vector3 _target;
        private float _followUpAttackRange;
        
        //Class constructor
        public CloseDistanceEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Start the navMeshAgent tracking
            AISystem.navMeshAgent.isStopped = false;
            
            // Cache the range value so we're not always getting it in the tick function
            _followUpAttackRange = AISystem.enemySettings.shortRange;
            
            AISystem.animator.SetBool("IsClosingDistance", true);

            yield break;
        }

        public override void Tick()
        {
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            PositionTowardsTarget(AISystem.transform, _target);
            
            // Enemy movement itself is handled with root motion and navMeshAgent set destination
            AISystem.navMeshAgent.SetDestination(_target);
            
            // Change to circling state when close enough to the player
            if (InRange(AISystem.transform.position, _target, _followUpAttackRange))
            {
                EndState();
            }
        }

        public override void EndState()
        {
            AISystem.animator.SetBool("IsClosingDistance", false);
            
            AISystem.OnLightAttack();
        }
    }
}
