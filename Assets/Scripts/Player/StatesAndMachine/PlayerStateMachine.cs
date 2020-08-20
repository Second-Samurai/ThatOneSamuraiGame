using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour 
{
    public abstract void BeginState();
    public virtual void RunState() {}
    public virtual void EndState() {}
}

public class PlayerStateMachine : MonoBehaviour
{
    PlayerState currentState;

    //Summary: Implements new state for usage
    //
    public void AddState<T>() where T : PlayerState 
    {
        if (currentState != null) {
            this.RemoveState();
        }

        currentState = this.gameObject.AddComponent<T>();
        currentState.BeginState();
    }

    //Summary: Removes existent state attached
    //
    public void RemoveState() 
    {
        if (currentState != null) {
            currentState.EndState();
            Destroy(currentState);
        }
    }
}
