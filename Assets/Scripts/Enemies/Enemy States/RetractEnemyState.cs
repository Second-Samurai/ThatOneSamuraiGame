using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class RetractEnemyState : EnemyState
    {
        //Class constructor
        public RetractEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // When time rewinds to idle state, player is no longer found
            AISystem.animator.SetBool("PlayerFound", false);

            yield break;
        }
    }
}
