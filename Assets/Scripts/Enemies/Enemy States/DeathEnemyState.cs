using System;
using System.Collections;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;

namespace Enemies.Enemy_States
{
    public class DeathEnemyState : EnemyState
    {

        private readonly ILockOnObserver m_LockOnObserver;
        private readonly ILockOnSystem m_LockOnSystem;
        
        public DeathEnemyState(AISystem aiSystem) : base(aiSystem)
        {
            this.m_LockOnObserver = SceneManager.Instance.LockOnObserver 
                ?? throw new ArgumentNullException(nameof(SceneManager.Instance.LockOnObserver));
            this.m_LockOnSystem = SceneManager.Instance.LockOnSystem 
                ?? throw new ArgumentNullException(nameof(SceneManager.Instance.LockOnSystem));
        }

        public override IEnumerator BeginState()
        {
            // Debug.Log("Enemy dead");
            
            // Set enemy status to dead
            AISystem.bIsDead = true;
            
            // Finds a new target on the enemy tracker (only if the dying enemy was the locked on enemy)
            // _lockOnTracker = GameManager.instance.LockOnTracker;
            // _lockOnTracker.SwitchDeathTarget(AISystem.transform);
            this.m_LockOnSystem.SelectNewTarget();
            
            // Remove enemy from enemy tracker and lock on tracker
            TempWinTracker.instance.enemyCount--;
            // _lockOnTracker.RemoveEnemy(AISystem.transform);
            AISystem.enemyTracker.RemoveEnemy(AISystem.transform);
            this.m_LockOnObserver.OnRemoveLockOnTarget.Invoke(AISystem.transform);

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

            if (AISystem.particleSpawn) AISystem.particleSpawn.SpawnParticles();

            yield break;
        }
    }
}
