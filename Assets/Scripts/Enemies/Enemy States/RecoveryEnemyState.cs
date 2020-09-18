using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class RecoveryEnemyState : EnemyState
    {
        //Class constructor
        public RecoveryEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Stop guard broken animation stop
            AISystem.animator.SetBool("IsGuardBroken", false);
            
            

            yield break;
        }
        
        // End state is called at the end of the recovery animation through animation events
        public override void EndState()
        {
            AISystem.animator.SetBool("IsApproaching", true);
            AISystem.OnApproachPlayer();
        }
    }
}
