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
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;

            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            PositionTowardsTarget(AISystem.transform, _target);
            
            AISystem.animator.SetBool("IsBlocking", true);
            
            // Wait between min and max block time before dropping guard
            yield return new WaitForSeconds(Random.Range(AISystem.enemySettings.minBlockTime, AISystem.enemySettings.maxBlockTime));
            
            EndState();
        }

        // TODO: Detect if the enemy is hit mid block here
        public override void Tick()
        {
            base.Tick();
        }

        // End state should only be performed if the enemy hasn't died or got guard broken mid block
        public override void EndState()
        {
            if (!IsDeadOrGuardBroken())
            {
                AISystem.animator.SetBool("IsBlocking", false);
            
                ChooseAfterBlockOption();
            }
        }

        private void ChooseAfterBlockOption()
        {
            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;

            if (InRange(AISystem.transform.position, _target, AISystem.enemySettings.circleThreatenRange))
            {
                int decision = Random.Range(0, 2);
                if (decision == 0) // Go for an attack
                {
                    AISystem.OnLightAttack();
                }
                else // Dodge backwards
                {
                    AISystem.dodgeDirectionZ = -1;
                    AISystem.OnDodge();
                }
            }
            else
            {
                AISystem.OnCirclePlayer();
            }
        }
    }
}
