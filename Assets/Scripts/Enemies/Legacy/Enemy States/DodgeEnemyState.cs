﻿using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class DodgeEnemyState : EnemyState
    {
        //Class constructor
        public DodgeEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            if(AISystem.enemyType == EnemyType.BOSS)
            {
                Animator.SetLayerWeight(1, 0);
                Animator.SetTrigger("TriggerDodge");
                AISystem.StartIntangibility();
                yield break;
            }

            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            // Set the dodge trigger
            // NOTE: Dodge direction is based on MovementX and MovementZ
            // These variables are set before TriggerDodge is called

            Animator.SetTrigger("TriggerDodge");
            
            // Perform a dodge using the enemy dodge force and movement variables
            AISystem.DodgeImpulse(new Vector3(
                    Animator.GetFloat("MovementX"), 
                    0, 
                    Animator.GetFloat("MovementZ")), 
                AISystem.enemySettings.GetEnemyStatType(AISystem.enemyType).dodgeForce);

            yield break;
            
            // NOTE: End state is called through an animation event in the dodge animation
        }

        public override void EndState()
        {
            if (AISystem.enemyType == EnemyType.BOSS)
            {
                if (AISystem.armourManager.armourCount <= 3)
                {
                    AISystem.StopIntangibility();

                    int decision = Random.Range(0, 2);
                    if(decision == 0)
                        AISystem.OnBossTaunt();
                    else 
                        AISystem.OnBossArrowMove();

                }
                else
                {
                    AISystem.StopIntangibility();
                    AISystem.OnBossArrowMove();

                }
            }
            else
            {
                // Get the true target point (float offset is added to get a more accurate player-enemy target point)
                Vector3 target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
                ChooseActionUsingDistance(target);

            }
        }
    }
}
