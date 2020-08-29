using System.Collections;
using Enemy_Scripts;

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

        public virtual IEnumerator EndState()
        {
            yield break;
        }
    }
}
