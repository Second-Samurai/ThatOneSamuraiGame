using System.Collections;
using System.Runtime.CompilerServices;
using Enemy_Scripts;
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
                // Look at the target to move into their direction
                transform.LookAt(target);
                //Ignore the X and Z rotations
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
        }

        protected bool InRange(Vector3 position, Vector3 targetPosition, float stopApproachingRange)
        {
            return Vector3.Distance(position, targetPosition) < stopApproachingRange;
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
