﻿using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class CircleEnemyState : EnemyState
    {
        private Vector3 _target;
        private float _longRange;
        private float _shortRange;
        
        private bool _bIsThreatened = false;
        
        //Cooldown variables
        private bool _bRunCooldown;
        private float _remainingTime;
        private float _maxCooldownTime = 1.5f;

        //Class constructor
        public CircleEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;

            AISystem.bHasBowDrawn = false;
            // Cache the range value so we're not always getting it in the tick function
            _longRange = AISystem.enemySettings.longRange;
            _shortRange = AISystem.enemySettings.shortRange;
            
            // Set the enemy to be circling (useful for the enemy tracker)
            AISystem.bIsCircling = true;
            
            // Give a random circling speed
            AISystem.SetCircleSpeed(Random.Range(0.8f, 1.2f));

            // Pick a strafe direction and trigger movement animator
            PickStrafeDirection();

            yield break;
        }
        
        public override void Tick()
        {
            if (_bRunCooldown)
            {
                _remainingTime -= Time.deltaTime;
                if (_remainingTime <= 0)
                {
                    StopDirChangeCooldown();
                }
            }

            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            // Set the rotation of the enemy
            PositionTowardsTarget(AISystem.transform, _target);
            
            // If player approaches circling enemy, trigger threatened bool and end state
            if(InRange(AISystem.transform.position, _target, _shortRange))
            {
                _bIsThreatened = true;
                EndState();
            }
            // If player runs from circling enemy, trigger end state
            else if (!InRange(AISystem.transform.position, _target, _longRange))
            {
                EndState();
            }
        }

        public override void EndState()
        {
            // Stop circling behaviour in AISystem (this is also called in enemy tracker)
            AISystem.StopCircling();

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
            int direction = Random.Range(0, 2);
            if (direction == 0)
            {
                Animator.SetFloat("MovementX", 0f);
            }
            else
            {
                Animator.SetFloat("MovementX", 1.0f);
            }
            
            Animator.SetTrigger("TriggerMovement");
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
            
            if (AISystem.enemyType != EnemyType.GLAIVEWIELDER)
            {
                if (actionNumber >= 4)
                {
                    AISystem.OnSwordAttack();
                }
                else
                {
                    AISystem.OnSwordAttack();
                    //AISystem.OnBlock();
                }
            }
            else
            {
                AISystem.OnGlaiveAttack();
            }
        }
    
        //Called from OnCollisionEnter in AISystem
        //but only when enemy is circling
        public void StartDirChangeCooldown()
        {
            if (!_bRunCooldown)
            {
                Debug.LogWarning("Starting new cooldown, enemy to switch directions now");
                _bRunCooldown = true;
                _remainingTime = _maxCooldownTime;
                
                //Change direction logic
                float newMovementX = 0;
                if (Animator.GetFloat("MovementX") == 0) newMovementX = 1;
                //Change direction in the animator
                Animator.SetFloat("MovementX", newMovementX);
            }
        }
        
        private void StopDirChangeCooldown()
        {
            Debug.LogWarning("Stopping cooldown");
            _bRunCooldown = false;
            _remainingTime = 0;
        }
        
    }
}
