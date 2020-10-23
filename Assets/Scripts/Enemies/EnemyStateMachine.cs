using Enemies.Enemy_States;
using UnityEngine;

namespace Enemy_Scripts
{
    // ENEMY STATE MACHINE INFO
    // This exists for 3 reasons
    // 1. To store the current enemies' state
    // 2. To be responsible for setting to a new state
    // 3. To give AI system monobehaviour and it's functions
    // If we're looking to expand on state management, this is the place to do it

    public class EnemyStateMachine : MonoBehaviour
    {
        // Breaking standard naming conventions for the sake of state naming
        public EnemyState EnemyState; // Holds the current enemy state
        public bool PrintStates = false;

        public virtual void SetState(EnemyState newEnemyState)
        {
            EnemyState = newEnemyState;
            if (!gameObject.activeInHierarchy) return;
            StartCoroutine(EnemyState.BeginState());

            if (PrintStates) Debug.Log(gameObject.name + " Switching States: " + newEnemyState);
        }

        protected void FixedUpdate()
        {
            // Only run Tick() if enemy state is not null
            EnemyState?.Tick();
        }

       

    }
}
