using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    public class RewindEnemyState : EnemyState
    {
        //Class constructor
        public RewindEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override IEnumerator BeginState()
        {
            Animator anim = AISystem.animator;
            
            // Set the player to no longer being found
            AISystem.bPlayerFound = false;
            // AISystem.animator.SetBool("PlayerFound", false);
            
            ResetAnimationVariables();
            
            // anim.SetBool("IsGuardBroken", false);
            
            // Set enemy to no longer being dead
            AISystem.bIsDead = false;

            yield break;
        }
    }
}
