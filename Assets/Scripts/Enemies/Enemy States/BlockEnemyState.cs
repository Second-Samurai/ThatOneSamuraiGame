using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class BlockEnemyState : EnemyState
    {
        private Vector3 _target;

        private float _remainingBlockTime;
        private bool _bDecreaseBlockTime = false;
        
        //Class constructor
        public BlockEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            AISystem.parryEffects.PlayGleam();
            
            StartBlockTimer();
            
            // While blocking, set the enemy to be able to parry
            AISystem.eDamageController.enemyGuard.canParry = true;
            
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            // Set the block trigger
            Animator.SetTrigger("TriggerBlock");
            
            // Reset trigger after frame has passed
            yield return null;
            Animator.ResetTrigger("TriggerBlock");
            
            // NOTE: BlockEnemyState can be interrupted to enter ParryEnemyState
            // if the player attacks mid block. This will make it so the timer never
            // finishes and EndState is never called 
        }

        public override void Tick()
        {
            // Get target position and face towards it
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            PositionTowardsTarget(AISystem.transform, _target);

            // Decrease block time if bool is true and there is time remaining
            if (_bDecreaseBlockTime && _remainingBlockTime > 0)
            {
                _remainingBlockTime -= Time.deltaTime;
            }
            else if(_bDecreaseBlockTime) // Stop block timer if bool is true and there is no time remaining
            {
                _bDecreaseBlockTime = false;
                _remainingBlockTime = 0;
                EndState();
            }
        }
        
        public override void EndState()
        {
            AISystem.eDamageController.enemyGuard.canParry = false;

            ChooseAfterBlockOption();
        }

        private void StartBlockTimer()
        {
            // Set remaining block time to a value between min and max block time
            _remainingBlockTime = Random.Range(
                AISystem.enemySettings.GetEnemyStatType(AISystem.enemyType).minBlockTime,
                AISystem.enemySettings.GetEnemyStatType(AISystem.enemyType).maxBlockTime);
            
            _bDecreaseBlockTime = true;
        }

        private void ChooseAfterBlockOption()
        {
            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;

            if (InRange(AISystem.transform.position, _target, AISystem.enemySettings.shortMidRange))
            {
                // Tutorial enemies will always circle after block state
                if(AISystem.enemyType == EnemyType.TUTORIALENEMY)
                {
                    AISystem.OnCirclePlayer();
                }
                
                // Make a 50/50 decision to attack or dodge
                int decision = Random.Range(0, 2);
                if (decision == 0) // Go for an attack
                {
                    AISystem.OnLightAttack();
                }
                else // Dodge backwards
                {
                    // Dodge direction is set in the state before OnDodge is called
                    // This is so we can choose a dodge direction based on the previous state
                    Animator.SetFloat("MovementZ", -1);
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
