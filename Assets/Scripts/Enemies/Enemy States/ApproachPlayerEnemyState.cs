using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class ApproachPlayerEnemyState : EnemyState
    {
        private Vector3 _target;
        private float _chaseToCircleRange;

        //Class constructor
        public ApproachPlayerEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {

            if(AISystem.enemyType == EnemyType.BOSS)
            {
                if (AISystem.armourManager.armourCount <= 3)
                {
                    int decision = Random.Range(0, 2);
                    if (decision == 0)
                    {
                        AISystem.OnBossArrowMove();
                    }
                    else
                    {
                        AISystem.OnBossTaunt();

                    }
                }
                else if(AISystem.armourManager.armourCount <= 5)
                {
                    int decision = Random.Range(0, 2);
                    if(decision == 0)
                    {
                        AISystem.OnBossArrowMove();
                    }
                }
            }

            AISystem.bPlayerFound = true;
            AISystem.bHasBowDrawn = false;
            // Start the navMeshAgent tracking
            AISystem.navMeshAgent.isStopped = false;
            
            // Cache the range value so we're not always getting it in the tick function
            _chaseToCircleRange = AISystem.enemySettings.midRange;
            
            // Trigger the movement blend tree with a forward approach
            Animator.SetFloat("MovementZ", 1.0f);
            Animator.SetTrigger("TriggerMovement");
            
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
            if (InRange(AISystem.transform.position, _target, _chaseToCircleRange))
            {
                EndState();
            }
        }

        public override void EndState()
        {
            // Reset animation variables
            Animator.SetFloat("MovementZ", 0.0f);

            if (AISystem.enemyType != EnemyType.BOSS) AISystem.OnCirclePlayer();
            else AISystem.OnJumpAttack();
        }
    }
}
