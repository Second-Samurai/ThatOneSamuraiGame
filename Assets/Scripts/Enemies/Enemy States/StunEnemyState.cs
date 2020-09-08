using System.Collections;
using UnityEngine;

public interface ICheck {
    string GetName();
}

namespace Enemies.Enemy_States
{
    public class StunEnemyState : EnemyState
    {
        //Class constructor
        public StunEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Set the guard broken animator bool to true, to ideally play the animation
            AISystem.animator.SetBool("IsGuardBroken", true);

            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;

            yield break;
        }
    }
}
