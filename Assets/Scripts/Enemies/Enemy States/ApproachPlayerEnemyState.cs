using System.Collections;
using Enemy_Scripts;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class ApproachPlayerEnemyState : EnemyState
    {
        //Class constructor
        public ApproachPlayerEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // TODO: Remove placeholder data
            Debug.Log("is approching");

            AISystem.bPlayerFound = true;

            yield break;
        }

        public override void Tick()
        {
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            Vector3 target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            // Get enemy speed data
            float step = AISystem.enemySettings.enemyData.moveSpeed * Time.deltaTime;

            if (AISystem.bPlayerFound)
            {
                AISystem.transform.position = Vector3.MoveTowards(AISystem.transform.position, target, step);
                Debug.Log("Enemy is approaching player");
            }
        }
    }
}
