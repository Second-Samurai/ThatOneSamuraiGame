using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class BlockEnemyState : EnemyState
    {
        //Class constructor
        public BlockEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Placeholder Behaviour, place actions here
            Debug.Log("is blocking");

            yield break;
        }
    }
}
