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
            // Placeholder Behaviour, place actions here
            Debug.Log("is dead");

            yield break;
        }
    }
}
