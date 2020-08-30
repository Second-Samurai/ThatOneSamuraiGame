using System.Collections;
using Enemy_Scripts;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class ApproachPlayerEnemyState : EnemyState
    {
        private float _stopApproachingRange;
        
        //Class constructor
        public ApproachPlayerEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            _stopApproachingRange = AISystem.enemySettings.stopApproachingRange;
            AISystem.SetPlayerFound(true);
            AISystem.SetApproaching(true);
            
            yield break;
        }

        public override void Tick()
        {
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            Vector3 target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            Transform transform = AISystem.transform;

            PositionTowardsPlayer(transform, target);
            // Enemy movement itself is handled with root motion
            
            // Change to circling state when close enough to the player
            if (InRange(transform.position, target, _stopApproachingRange))
            {
                EndState();
            }
        }

        public override void EndState()
        {
            AISystem.SetApproaching(false);
                
            // TODO: Change to circling, for demo purposes the enemy will just swing to attack
            AISystem.OnLightAttack();
        }
    }
}
