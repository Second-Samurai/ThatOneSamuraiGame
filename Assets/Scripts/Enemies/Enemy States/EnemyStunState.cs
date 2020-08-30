using System.Collections;
using UnityEngine;

public interface ICheck {
    string GetName();
}

namespace Enemies.Enemy_States
{
    public class EnemyStunState : EnemyState
    {
        //Class constructor
        public EnemyStunState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Placeholder Behaviour, place actions here
            Debug.Log("EnemyState: Enemy is stunned");
            
            // Stop the navmesh pathfinding
            AISystem.navMeshAgent.SetDestination(AISystem.transform.position);
            AISystem.navMeshAgent.enabled = false;

            yield break;
        }
    }
}
