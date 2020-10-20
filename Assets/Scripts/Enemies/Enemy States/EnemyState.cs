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
            return Vector3.Magnitude(targetPosition - position) < stopApproachingRange;
        }

        protected void ChooseActionUsingDistance(Vector3 target)
        {
            // If close enough, attack again
            if (InRange(AISystem.transform.position, target, AISystem.enemySettings.shortRange))
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
                if (AISystem.enemyType == EnemyType.BOSS)
                {
                    if (AISystem.armourManager.armourCount <= 6)
                    {
                        int decision = Random.Range(0, 3);

                        if (decision == 0) // Fire
                        {
                            AISystem.OnBossArrowMove();
                        }
                        else // Approach Player
                        {
                            AISystem.OnCirclePlayer();
                        }
                    }
                }
                else
                {
                    AISystem.OnCirclePlayer(); // Start circling if in close enough range

                }
            }
            else
            {
                // Approach player if they are too far away
                
                if (AISystem.enemyType == EnemyType.BOSS)
                {
                    if(AISystem.armourManager.armourCount <= 5)
                    {
                        int decision = Random.Range(0, 2);

                        if (decision == 0) // Fire
                        {
                            AISystem.OnBossArrowMove();
                        }
                        else // Approach Player
                        {
                            AISystem.OnApproachPlayer();
                        }
                    }
                }
                else if (AISystem.enemyType != EnemyType.GLAIVEWIELDER)
                {
                    AISystem.OnApproachPlayer();
                }
                else
                {
                    int decision = Random.Range(0, 3);

                    if (decision == 0) // Jump Attack (currently disabled)
                    {
                        AISystem.OnChargePlayer();
                    }
                    else // Approach Player
                    {
                        AISystem.OnApproachPlayer();
                    }
                }
            }
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
