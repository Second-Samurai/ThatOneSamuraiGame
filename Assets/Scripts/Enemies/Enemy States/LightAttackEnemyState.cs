using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class LightAttackEnemyState : EnemyState
    {
        //Class constructor
        public LightAttackEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Placeholder Behaviour, place actions here
            Debug.Log("is light attacking");

            yield break;
        }
    }
}
