using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class RewindEnemyState : EnemyState
    {
        //Class constructor
        public RewindEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override IEnumerator BeginState()
        {
            // Placeholder Behaviour, place actions here
            Debug.LogWarning("RewindState");

            yield break;
        }
    }
}
