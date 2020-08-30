using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class EnemyStunState : EnemyState
    {
        //Class constructor
        public EnemyStunState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Placeholder Behaviour, place actions here
            Debug.Log("is stunned");

            yield break;
        }
    }
}
