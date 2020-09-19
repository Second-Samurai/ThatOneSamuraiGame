using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Enemies.Enemy_States
{
    public class CircleEnemyState : EnemyState
    {
        private Vector3 _target;
        private float _circleToChaseRange;
        private float _circleThreatenRange;
        private bool _bIsThreatened = false;

        //Class constructor
        public CircleEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // For the enemy tracker, restart the impatience countdown
            // See enemy tracker for more details
            AISystem.enemyTracker.StartImpatienceCountdown();

            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;

            PickStrafeDirection();
            
            // Cache the range value so we're not always getting it in the tick function
            _circleToChaseRange = AISystem.enemySettings.longRange;
            _circleThreatenRange = AISystem.enemySettings.shortMidRange;
            
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
            if(InRange(AISystem.transform.position, _target, _circleThreatenRange))
            {
                _bIsThreatened = true;
                EndState();
            }
            // Change to chase state when too far from the player
            else if (!InRange(AISystem.transform.position, _target, _circleToChaseRange))
            {
                EndState();
            }
        }

        public override void EndState()
        {
            AISystem.animator.SetBool("IsStrafing", false);
            
            AISystem.animator.SetFloat("StrafeDirectionX", 0);
            
            // If threatened, do a threatened response (i.e. if the player is close)
            // Else approach the player again (i.e. if the player is far)
            if(_bIsThreatened)
                PickThreatenedResponse();
            else
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
        
        // Pick a random action to perform when the player approaches the enemy
        private void PickThreatenedResponse()
        {
            // Reset threatened value
            _bIsThreatened = false;
            
            int actionNumber = Random.Range(0, 10);
            if(AISystem.enemyType == EnemyType.TUTORIALENEMY)
            {
                actionNumber = 3;
            }
            
            switch(actionNumber)
            {
                case int i when (i > 4): // LIGHT ATTACK
                    AISystem.OnLightAttack();
                    break;
                case int i when (i > 2 && i <= 4): // START BLOCKING
                    AISystem.OnBlock();
                    break;
                case int i when (i > -1 && i <= 2): // RETRACT BACK
                    AISystem.dodgeDirectionZ = -1;
                    AISystem.OnDodge();
                    break;
                default:
                    Debug.LogError("Enemy action response is out of bounds");
                    break;
            }
        }
        
    }
}
