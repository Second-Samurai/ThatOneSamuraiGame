using System.Collections;
using UnityEngine;

namespace Enemy_Scripts.Enemy_States
{
    public class IdleEnemyState : EnemyState
    {
        //Class constructor
        public IdleEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator Start()
        {
            // Placeholder Behaviour, place actions here
            Debug.Log("is idle");
            AISystem.enemyMaterial.color = Color.yellow;
            
            yield break;
        }
    }
}
