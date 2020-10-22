using System.Collections;
using Enemy_Scripts;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class CloseDistanceEnemyState : EnemyState
    {
        private Vector3 _target;
        private float _longRange;
        private float _shortRange;
        
        //Class constructor
        public CloseDistanceEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Set the enemy to isClosingDistance (used for avoiding heavy attacks)
            AISystem.bIsClosingDistance = true;
            
            // Start the navMeshAgent tracking
            AISystem.navMeshAgent.isStopped = false;
            AISystem.attackIndicator.ShowIndicator();
            // Cache the range value so we're not always getting it in the tick function
            _longRange = AISystem.enemySettings.longRange;
            _shortRange = AISystem.enemySettings.shortRange;
            
            // Trigger the movement blend tree with a forward approach.
            // Since CloseDistance is called by the enemy tracker we have to reset
            // the MovementX value which is set in the previous circling state
            Animator.SetFloat("MovementX", 0.0f);
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
            if (InRange(AISystem.transform.position, _target, _shortRange))
            {
                EndState();
            }
            // Change to chase state when too far from the player
            else if (!InRange(AISystem.transform.position, _target, _longRange))
            {
                EndState();
            }
        }

        public override void EndState()
        {
            // Set the enemy to isClosingDistance (used for avoiding heavy attacks)
            AISystem.bIsClosingDistance = false;
            
            // Reset animation variables
            Animator.SetFloat("MovementZ", 0.0f);

            // Change to circling state when close enough to the player
            if (InRange(AISystem.transform.position, _target, _shortRange))
            {
                if (AISystem.enemyType == EnemyType.GLAIVEWIELDER)
                {
                    AISystem.OnGlaiveAttack();
                }
                else
                {
                    AISystem.OnSwordAttack();
                }
            }
            // Change to chase state when too far from the player
            else if (!InRange(AISystem.transform.position, _target, _longRange))
            {
                AISystem.OnApproachPlayer();
            }
        }
    }
}
