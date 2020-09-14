using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    // ENEMY STATE INFO
    // This acts as the connection between the state machine and states. All enemy states inherit from this class. Other
    // states will override it's functions, most commonly, Start(). Other functions can be added and overriden here too.
    
    public abstract class EnemyState
    {
        // Breaking standard naming conventions for the sake of state naming
        protected AISystem AISystem;

        private float _rotationSpeed = 4.0f;

        // Class constructor that takes in the AISystem
        protected EnemyState(AISystem aiSystem)
        {
            AISystem = aiSystem;
        }
        
        // State start, update and end methods
        // These are to be overriden in the enemy states
        public virtual IEnumerator BeginState()
        {
            yield break;
        }

        public virtual void Tick()
        {
            
        }

        public virtual void EndState()
        {
            
        }

        protected void PositionTowardsTarget(Transform transform, Vector3 target)
        {
            if (AISystem.bPlayerFound)
            {
                Vector3 lookDir = target - transform.position;
                Quaternion lookRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, _rotationSpeed);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
        }

        protected bool InRange(Vector3 position, Vector3 targetPosition, float stopApproachingRange)
        {
            return Vector3.Distance(position, targetPosition) < stopApproachingRange;
        }

        protected void ChooseActionUsingDistance(Vector3 target)
        {
            if (InRange(AISystem.transform.position, target, AISystem.enemySettings.followUpAttackRange))
            {
                AISystem.OnLightAttack(); // Light attack again if close enough
            }
            else if(InRange(AISystem.transform.position, target, AISystem.enemySettings.chaseToCircleRange))
            {
                AISystem.OnCirclePlayer(); // Start circling if in close enough range
            }
            else
            {
                AISystem.OnApproachPlayer(); // Approach player if they are too far away
            }
        }

        protected void ResetAnimationBools()
        {
            Animator anim = AISystem.animator;
            
            // Set all suitable animation bools to false
            anim.SetBool("IsLightAttacking", false);
            anim.SetBool("IsApproaching", false);
            anim.SetBool("IsQuickBlocking", false);
            
            // NOTE: Anims like PlayerFound, IsDead and IsGuardBroken should be treated separately to this function
        }
    }
}
