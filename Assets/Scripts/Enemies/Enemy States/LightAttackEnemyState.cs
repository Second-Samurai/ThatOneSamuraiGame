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
            
            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            Vector3 target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            Transform transform = AISystem.transform;
            
            PositionTowardsPlayer(transform, target);
            
            AISystem.SetLightAttacking(true);
            yield return new WaitForSeconds(2.0f);
            AISystem.SetLightAttacking(false);
            
            // Check current distance to determine next action
            target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            if (InRange(transform.position, target, AISystem.enemySettings.stopApproachingRange))
                AISystem.OnLightAttack(); // Light attack again if close enough
            else
                AISystem.OnApproachPlayer(); // Approach player if they are too far away
        }
    }
}
