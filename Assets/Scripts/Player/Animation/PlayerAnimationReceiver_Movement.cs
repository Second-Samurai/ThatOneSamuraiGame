using UnityEngine;using UnityEngine.Events;

public interface IMovementAnimationEvents
{

    #region - - - - - - Properties - - - - - -

    UnityEvent OnEnableMovement { get; }
    
    UnityEvent OnDisableMovement { get; }

    #endregion Properties
  
}

public class PlayerAnimationReceiver_Movement : MonoBehaviour, IMovementAnimationEvents
{

    #region - - - - - - Fields - - - - - -

    private readonly UnityEvent m_OnEnableMovement = new();
    private readonly UnityEvent m_OnDisableMovement = new();

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public UnityEvent OnEnableMovement => this.m_OnEnableMovement;

    public UnityEvent OnDisableMovement => this.m_OnDisableMovement;
    
    #endregion Properties

    #region - - - - - - Methods - - - - - -

    // Directly invoked by Unity's animation control
    public void EnableMovement()
        => this.m_OnEnableMovement.Invoke();

    // Directly invoked by Unity's animation control
    public void DisableMovement()
        => this.m_OnDisableMovement.Invoke();

    #endregion Methods

}
