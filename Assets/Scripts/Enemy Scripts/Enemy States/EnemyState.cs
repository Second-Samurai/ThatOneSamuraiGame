using System.Collections;
using UnityEngine;

namespace Enemy_Scripts.Enemy_States
{
    // ENEMY STATE INFO
    // This is where we can store basic enemy state data. All enemy states inherit from this class.
    // Other states will override it's functions, most commonly, Start(). Other functions can be
    // added and overriden here too and we can define basic enemy behaviours here too.
    
    public abstract class EnemyState
    {
        // Breaking standard naming conventions for the sake of state naming
        protected AISystem AISystem;
        
        // Class constructor that takes in the AISystem
        public EnemyState(AISystem aiSystem)
        {
            AISystem = aiSystem;
        }

        public virtual IEnumerator Start()
        {
            yield break;
        }
        
        // Define any other basic enemy behaviours in this script here
    }
}
