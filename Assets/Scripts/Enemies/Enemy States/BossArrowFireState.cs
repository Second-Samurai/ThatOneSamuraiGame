using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class BossArrowFireState : EnemyState
    {
        private Vector3 _target;
        private float _longRange;
        private float _shortRange;

        private bool _bIsThreatened = false; 

        //Class constructor
        public BossArrowFireState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // For the enemy tracker, restart the impatience countdown
            // See enemy tracker for more details
            //AISystem.enemyTracker.StartImpatienceCountdown();

            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;

            // Cache the range value so we're not always getting it in the tick function
            _longRange = AISystem.enemySettings.longRange;
            _shortRange = AISystem.enemySettings.shortRange;
            Animator.SetTrigger("TriggerArrowShot");
            Fire();
            // Pick a strafe direction and trigger movement animator
            PickStrafeDirection();

            yield break;
        }

        public override void Tick()
        {
            
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            Debug.Log(_target);

            // Set the rotation of the enemy
            PositionTowardsTarget(AISystem.transform, _target);

            // If player approaches circling enemy, trigger threatened bool and end state
            if (InRange(AISystem.transform.position, _target, AISystem.enemySettings.veryShortRange))
            {
                _bIsThreatened = true;
                EndState();
            }
            else if (InRange(AISystem.transform.position, _target, AISystem.enemySettings.midRange))
            {
                Animator.SetFloat("MovementZ", -1.0f);
            }
            // If player runs from circling enemy, trigger end state
            else if (InRange(AISystem.transform.position, _target, _longRange))
            {
                Animator.SetFloat("MovementZ", 1.0f);
            }
        }

        public override void EndState()
        {
            if (AISystem.shotCount > 1)
            {
                AISystem.shotCount--;
                AISystem.OnBossArrowFire();
            }
            else
            {
                AISystem.shotCount = 3;
                Animator.SetBool("BowDrawn", false);
                Animator.SetTrigger("SheathBow");
                Animator.SetLayerWeight(1, 0);
                AISystem.OnApproachPlayer();
            }
        }

        // Set the strafe direction
        private void PickStrafeDirection()
        {
            // Random.Range is non-inclusive for it's max value for ints
            if (Random.Range(0, 2) == 0)
            {
                Animator.SetFloat("MovementX", -1.0f);
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
            Animator.SetBool("BowDrawn", false);
            Animator.SetTrigger("SheathBow");
            Animator.SetLayerWeight(1, 0);
            // Reset threatened value
            _bIsThreatened = false;

            int actionNumber = Random.Range(0, 10);
            if (AISystem.enemyType == EnemyType.TUTORIALENEMY)
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

        public void Fire()
        {
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            Debug.Log(_target);
            Vector3 shotDirection = _target - AISystem.firePoint.position;
            shotDirection.y = 0;
            shotDirection = shotDirection.normalized;
            Debug.Log(shotDirection);
            GameObject _arrow = ObjectPooler.instance.ReturnObject("Arrow");
            //GameObject _arrow = Instantiate(arrow, shotOrigin.position, Quaternion.identity);
            _arrow.transform.position = AISystem.firePoint.position;
            _arrow.GetComponent<Projectile>().Launch(shotDirection, _target);
            
        } 

    }

    
}
