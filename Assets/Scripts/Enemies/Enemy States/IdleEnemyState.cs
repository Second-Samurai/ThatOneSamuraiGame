using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class IdleEnemyState : EnemyState
    {
        //Class constructor
        public IdleEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            yield break;
        }
    }
}
