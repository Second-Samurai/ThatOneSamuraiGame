using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class DeathEnemyState : EnemyState
    {
        //Class constructor
        public DeathEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Set enemy status to dead
            Debug.Log("Enemy dead");
            AISystem.bIsDead = true;
            AISystem.animator.SetBool("IsDead", true);
            
            // Remove enemy from temp enemy count and enemy tracker
            TempWinTracker.instance.enemyCount--;
            AISystem.enemyTracker.RemoveEnemy(AISystem.transform);
            
            // Enemy can no longer be damaged
            AISystem.eDamageController.DisableDamage();
            
            // Wait 2.0 seconds then de spawn the enemy
            yield return new WaitForSeconds(2.0f);
            AISystem.gameObject.SetActive(false);
        }
    }
}
