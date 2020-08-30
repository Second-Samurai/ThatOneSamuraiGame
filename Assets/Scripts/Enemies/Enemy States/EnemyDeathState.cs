using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class EnemyDeathState : EnemyState
    {
        //Class constructor
        public EnemyDeathState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Placeholder Behaviour, place actions here
            Debug.Log("is dead");

            yield break;
        }
    }
}
