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
            AISystem.animator.SetBool("IsGuardBroken", true);

            yield break;
        }
    }
}
