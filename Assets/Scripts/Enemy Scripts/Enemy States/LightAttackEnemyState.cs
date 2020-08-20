using System.Collections;
using UnityEngine;

namespace Enemy_Scripts.Enemy_States
{
    public class LightAttackEnemyState : EnemyState
    {
        //Class constructor
        public LightAttackEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator Start()
        {
            // Placeholder Behaviour, place actions here
            Debug.Log("is light attacking");
            AISystem.enemyMaterial.color = Color.red;
            
            yield break;
        }
    }
}
