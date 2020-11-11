using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class DeathEnemyState : EnemyState
    {
        private LockOnTracker _lockOnTracker;
        
        //Class constructor
        public DeathEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Debug.Log("Enemy dead");
            
            // Set enemy status to dead
            AISystem.bIsDead = true;
            
            // Finds a new target on the enemy tracker (only if the dying enemy was the locked on enemy)
            _lockOnTracker = GameManager.instance.lockOnTracker;
            _lockOnTracker.SwitchDeathTarget(AISystem.transform);
            
            // Remove enemy from enemy tracker and lock on tracker
            TempWinTracker.instance.enemyCount--;
            _lockOnTracker.RemoveEnemy(AISystem.transform);
            AISystem.enemyTracker.RemoveEnemy(AISystem.transform);

            // Enemy can no longer be damaged, enemies can no longer damage the player.
            AISystem.eDamageController.DisableDamage();
            
            // NOTE: Enemy collider is now turned off through an animation event
            // AISystem.col.enabled = false;
            
            // Disable guard meter
            AISystem.eDamageController.enemyGuard.DisableGuardMeter();

            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            // Stop unblockable if enemy was previously doing an unblockable attack
            AISystem.EndUnblockable();
            
            //Disable weapon for melee wielders
            if (AISystem.enemyType != EnemyType.ARCHER)
            {
                AISystem.meleeCollider.enabled = false;
            }

            // Set the death trigger
            Animator.SetBool("Finish", AISystem.bFinish);
            Animator.SetTrigger("TriggerDeath");
            
            yield break;
        }
    }
}
