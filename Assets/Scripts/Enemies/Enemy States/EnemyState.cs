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
        protected Animator Animator;

        private float _rotationSpeed = 4.0f;
        
        // TODO: Currently only used for light attack rotation, use in other states
        public bool bIsRotating = true;

        // Class constructor that takes in the AISystem
        protected EnemyState(AISystem aiSystem)
        {
            AISystem = aiSystem;
            Animator = aiSystem.animator;
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
            else
            {
                Debug.LogWarning("Warning: Enemy cannot find player to rotate to");
            }
        }

        protected bool InRange(Vector3 position, Vector3 targetPosition, float stopApproachingRange)
        {
            return Vector3.Distance(position, targetPosition) < stopApproachingRange;
        }

        protected void ChooseActionUsingDistance(Vector3 target)
        {
            // If close enough, attack again
            if (InRange(AISystem.transform.position, target, AISystem.enemySettings.shortMidRange))
            {
                if (AISystem.enemyType != EnemyType.GLAIVEWIELDER)
                {
                    AISystem.OnSwordAttack();
                }
                else
                {
                    AISystem.OnGlaiveAttack();
                }
            }
            else if(InRange(AISystem.transform.position, target, AISystem.enemySettings.midRange))
            {
                AISystem.OnCirclePlayer(); // Start circling if in close enough range
            }
            else
            {
                // Approach player if they are too far away
                if (AISystem.enemyType != EnemyType.GLAIVEWIELDER)
                {
                    AISystem.OnApproachPlayer();
                } 
                else
                {
                    int decision = Random.Range(1, 3);

                    if (decision == 0) // Jump Attack (currently disabled)
                    {
                        AISystem.OnJumpAttack();
                    }
                    else // Approach Player
                    {
                        AISystem.OnApproachPlayer();
                    }
                }
            }
        }

        protected void ResetAnimationVariables()
        {
            Animator anim = AISystem.animator;
            
            // Set all suitable animation bools to false
            anim.ResetTrigger("TriggerMovement");
            anim.ResetTrigger("TriggerGuardBreak");
            anim.ResetTrigger("TriggerDeath");
            anim.ResetTrigger("TriggerRecovery");
            anim.ResetTrigger("TriggerLightAttack");
            anim.ResetTrigger("TriggerCounterAttack");
            anim.ResetTrigger("TriggerDodge");
            anim.ResetTrigger("TriggerParryStun");
            anim.ResetTrigger("TriggerQuickBlock");
            anim.ResetTrigger("TriggerBlock");
            
            // Set all movement variables to 0
            anim.SetFloat("MovementX", 0);
            anim.SetFloat("MovementZ", 0);
            
            // anim.SetBool("IsLightAttacking", false);
            // anim.SetBool("IsApproaching", false);
            // anim.SetBool("IsBlocking", false);
            // anim.SetBool("IsQuickBlocking", false);
            // anim.SetBool("IsParried", false);
            // anim.SetBool("IsStrafing", false);
            // anim.SetFloat("StrafeDirectionX", 0);
            // anim.SetBool("IsDodging", false);
            // anim.ResetTrigger("Parried");
        }
        
        public void StopRotating()
        {
            bIsRotating = false;
        }
        public void StartRotating()
        {
            bIsRotating = true;
        }
    }
}
