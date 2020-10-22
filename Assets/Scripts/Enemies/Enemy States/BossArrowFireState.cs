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

        private bool hasfired = false;

        //Class constructor
        public BossArrowFireState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            //if (AISystem.shotCount < 1)
            //{
            //    AISystem.shotCount = 3;
            //    AISystem.bHasBowDrawn = false;
            //    Animator.SetBool("BowDrawn", false);
            //    Animator.SetTrigger("SheathBow");
            //    Animator.SetLayerWeight(1, 0);
            //    AISystem.OnApproachPlayer();
            //    AISystem.weaponSwitcher.EnableBow(false);
            //    AISystem.weaponSwitcher.EnableSword(true);
            //    yield break;
            //}
            Animator.SetLayerWeight(1, 1);
            // For the enemy tracker, restart the impatience countdown
            // See enemy tracker for more details
            //AISystem.enemyTracker.StartImpatienceCountdown();
            AISystem.weaponSwitcher.EnableSword(false);
            AISystem.weaponSwitcher.EnableGlaive(false);
            AISystem.weaponSwitcher.EnableBow(true);
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            AISystem.bHasBowDrawn = true;
            // Cache the range value so we're not always getting it in the tick function
            _longRange = AISystem.enemySettings.longRange;
            _shortRange = AISystem.enemySettings.shortRange;
            // Pick a strafe direction and trigger movement animator
            PickStrafeDirection();

            yield break;
        }

        public override void Tick()
        {
            if (AISystem.shotTimer <= 0 && hasfired == false)
            {
                Animator.SetTrigger("TriggerArrowShot");
                Fire();
            }
            else AISystem.shotTimer -= Time.deltaTime; 
            
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;


            // Set the rotation of the enemy
            PositionTowardsTarget(AISystem.transform, _target);

            // If player approaches circling enemy, trigger threatened bool and end state
            if (InRange(AISystem.transform.position, _target, AISystem.enemySettings.veryShortRange))
            {
                _bIsThreatened = true;
                Animator.SetFloat("MovementZ", -1.0f);
            }
            else if (InRange(AISystem.transform.position, _target, AISystem.enemySettings.shortRange))
            {
                Animator.SetFloat("MovementZ", -1.0f);
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
            Debug.LogWarning(AISystem.shotCount);
            if (AISystem.shotCount > 1)
            {
               // AISystem.shotCount--;
                AISystem.OnBossArrowFire();
            }
            else
            {
                AISystem.shotCount = 4;
                AISystem.bHasBowDrawn = false;
                Animator.SetBool("BowDrawn", false);
                Animator.SetTrigger("SheathBow");
                Animator.SetLayerWeight(1, 0);
                AISystem.OnApproachPlayer();
                AISystem.weaponSwitcher.EnableBow(false);
                AISystem.weaponSwitcher.EnableSword(true);
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
            AISystem.weaponSwitcher.EnableBow(false);
            AISystem.weaponSwitcher.EnableSword(true);
            Animator.SetTrigger("SheathBow");
            AISystem.bHasBowDrawn = false;
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
            AISystem.enemyAudio.Release();
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
            AISystem.shotTimer = 1;
            hasfired = true;
            AISystem.shotCount--;
           // Debug.LogError(AISystem.shotCount);
            if (AISystem.shotCount <= 1) 
            {
               // Debug.LogError("exit");
                AISystem.shotCount = 4;
                AISystem.bHasBowDrawn = false;
                Animator.SetBool("BowDrawn", false);
                Animator.SetTrigger("SheathBow");
                Animator.SetLayerWeight(1, 0);
                AISystem.OnApproachPlayer();
                AISystem.weaponSwitcher.EnableBow(false);
                AISystem.weaponSwitcher.EnableSword(true);
            }

            
        } 

    }

    
}
