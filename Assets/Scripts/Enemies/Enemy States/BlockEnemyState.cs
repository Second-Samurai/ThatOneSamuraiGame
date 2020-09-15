using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class BlockEnemyState : EnemyState
    {
        private Vector3 _target;
        
        //Class constructor
        public BlockEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            ResetAnimationBools();
            
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;

            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            PositionTowardsTarget(AISystem.transform, _target);
            
            AISystem.animator.SetBool("IsBlocking", true);
            
            // Wait between min and max block time before dropping guard
            yield return new WaitForSeconds(Random.Range(AISystem.enemySettings.minBlockTime, AISystem.enemySettings.maxBlockTime));
            
            EndState();
        }

        public override void Tick()
        {
            // Detect if the enemy is hit mid block
            base.Tick();
        }

        public override void EndState()
        {
            AISystem.animator.SetBool("IsBlocking", false);
            
            ChooseAfterBlockOption();
        }

        private void ChooseAfterBlockOption()
        {
            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;

            if (InRange(AISystem.transform.position, _target, AISystem.enemySettings.circleThreatenRange))
            {
                int decision = Random.Range(0, 2);
                if (decision == 0)
                {
                    AISystem.OnLightAttack();
                }
                else
                {
                    // TODO: Change to retract state
                    AISystem.OnLightAttack();
                }
            }
            else
            {
                AISystem.OnCirclePlayer();
            }
        }
    }
}
