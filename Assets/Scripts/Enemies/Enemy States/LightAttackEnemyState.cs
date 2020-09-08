using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class LightAttackEnemyState : EnemyState
    {
        private Vector3 _target;
        private Transform _transform;
        
        // Length multiplier is used to get the real animation length time.
        // Where 1 = full length, 0.8f is used because 20% of the animation is exit time
        private float _lengthMultiplier = 0.8f; 

        //Class constructor
        public LightAttackEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            Animator anim = AISystem.animator;

            // Get the target object and current enemy transform
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            _transform = AISystem.transform;
            
            PositionTowardsTarget(_transform, _target);
            
            // Set enemy to light attack and store animator state
            anim.SetBool("IsLightAttacking", true);

            // Need to wait until next frame before the state switches
            yield return null;
            AnimatorStateInfo animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);

            // Run end state after TRUE animation length minus last frame deltaTime
            yield return new WaitForSeconds(animatorStateInfo.length * _lengthMultiplier - Time.deltaTime);
            
            // Only run the end state of light attack if the enemy isn't stunned or dead
            if (anim.GetBool("IsGuardBroken") || anim.GetBool("IsDead"))
                yield break;
            
            EndState();
        }

        public override void EndState()
        {
            //Debug.Log("swing is finished");
            
            AISystem.animator.SetBool("IsLightAttacking", false);
            
            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;

            if (InRange(_transform.position, _target, AISystem.enemySettings.followUpAttackRange))
            {
                AISystem.OnLightAttack(); // Light attack again if close enough
            }
            else
            {
                AISystem.OnApproachPlayer(); // Approach player if they are too far away
            }
        }
    }
}
