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

            AISystem.SetPlayerFound(true);
            
            yield break;
        }

        public override void Tick()
        {
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            Vector3 target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            Transform transform = AISystem.transform;

            if (AISystem.GetPlayerFound())
            {
                // Look at the target to move into their direction
                transform.LookAt(target);
                //Ignore the X and Z rotations
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                
                // Enemy movement is handled with root motion
                
            }
        }
    }
}
