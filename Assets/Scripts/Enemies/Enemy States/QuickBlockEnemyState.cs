using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class QuickBlockEnemyState : EnemyState
    {
        private Animator anim;
        private Vector3 _target;
        
        // Length multiplier is used to get the real animation length time.
        // Where 1 = full length, 0.6f is used because 40% of the animation is exit time
        private float _lengthMultiplier = 0.6f; 
        
        //Class constructor
        public QuickBlockEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;

            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            PositionTowardsTarget(AISystem.transform, _target);
            
            anim = AISystem.animator;
            
            anim.SetBool("IsQuickBlocking", true);

            // Need to wait until next frame before the state switches
            yield return null;
            AnimatorStateInfo animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
            
            Debug.Log(animatorStateInfo.length);

            // Run end state after TRUE animation length minus last frame deltaTime
            yield return new WaitForSeconds(animatorStateInfo.length * _lengthMultiplier - Time.deltaTime);
            
            // Only run the end state of light attack if the enemy isn't stunned or dead
            if (IsGuardBrokenOrDead())
                yield break;
            
            EndState();
        }

        public override void EndState()
        {
            anim.SetBool("IsQuickBlocking", false);
            
            // Check current distance to determine next action
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            // TODO: Remove this. Temporary state switch until blocking is introduced
            if (InRange(AISystem.transform.position, _target, AISystem.enemySettings.followUpAttackRange))
            {
                AISystem.OnLightAttack(); // Light attack again if close enough
            }
            else
            {
                AISystem.OnApproachPlayer(); // Approach player if they are too far away
            }
            
            // TODO: Replace with this
            //AISystem.OnBlock();
        }
    }
}
