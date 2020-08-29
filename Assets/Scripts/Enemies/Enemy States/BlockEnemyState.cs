using System.Collections;
using UnityEngine;

namespace Enemy_Scripts.Enemy_States
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
            AISystem.enemyMaterial.color = Color.blue;
            
            yield break;
        }
    }
}
