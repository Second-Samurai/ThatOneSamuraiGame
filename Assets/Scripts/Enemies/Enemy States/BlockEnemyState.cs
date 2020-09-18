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

            AISystem.animator.SetBool("IsBlocking", true);
            
            // While blocking, set the enemy to be able to parry
            AISystem.eDamageController.enemyGuard.canParry = true;
            
            // Wait between min and max block time before dropping guard
            yield return new WaitForSeconds(Random.Range(AISystem.enemySettings.minBlockTime, AISystem.enemySettings.maxBlockTime));
            
            EndState();
        }

        public override void Tick()
        {
            // Get target position and face towards it
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            PositionTowardsTarget(AISystem.transform, _target);
        }

        // End state should only be performed if the following hasn't occurend mid block
        //    1. Enemy hasn't died
        //    2. Hasn't got guard broken 
        //    3. Been attacked (triggering a parry)
        public override void EndState()
        {
            if (!IsDeadOrGuardBroken() && !AISystem.animator.GetBool("IsParried"))
            {
                AISystem.eDamageController.enemyGuard.canParry = false;
                
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
                if(AISystem.enemyType == EnemyType.TUTORIALENEMY)
                {
                    AISystem.OnCirclePlayer();
                }
                else if (decision == 0) // Go for an attack
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
