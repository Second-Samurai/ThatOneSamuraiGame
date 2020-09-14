using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class DeathEnemyState : EnemyState
    {
        private float _despawnTimer = 4.0f;
        
        //Class constructor
        public DeathEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Debug.Log("Enemy dead");
            
            // Set enemy status to dead
            AISystem.bIsDead = true;
            AISystem.animator.SetBool("IsDead", true);
            
            // Remove enemy from temp enemy count and enemy tracker
            TempWinTracker.instance.enemyCount--;
            AISystem.enemyTracker.RemoveEnemy(AISystem.transform);
            
            // Enemy can no longer be damaged, enemies can no longer damage the player
            AISystem.eDamageController.DisableDamage();
            
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            //Disable weapon for melee wielders
            if (AISystem.enemyType == EnemyType.SWORDSMAN || AISystem.enemyType == EnemyType.GLAIVEWIELDER)
            {
                AISystem.meleeCollider.enabled = false;
            }
            
            // Wait for de spawn seconds then de spawn the enemy
            yield return new WaitForSeconds(_despawnTimer);
            AISystem.gameObject.SetActive(false);
        }
    }
}
