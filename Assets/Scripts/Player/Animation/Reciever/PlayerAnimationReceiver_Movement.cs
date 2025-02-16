using UnityEngine;
using UnityEngine.Events;

public interface IMovementAnimationEvents
{

    #region - - - - - - Properties - - - - - -

    UnityEvent OnEnableMovement { get; }
    
    UnityEvent OnDisableMovement { get; }
    
    UnityEvent OnEnableRotation { get; }
    
    UnityEvent OnDisableRotation { get; }
    
    UnityEvent OnBlockDodge { get; }
    
    UnityEvent OnStartDodge { get; }
    
    UnityEvent OnEndDodge { get; }
    
    UnityEvent OnResetDodge { get; }
    
    UnityEvent OnLockMoveInput { get; }
    
    UnityEvent OnUnlockMoveInput { get; }
    
    #endregion Properties
  
}

public class PlayerAnimationReceiver_Movement : MonoBehaviour, IMovementAnimationEvents
{

    #region - - - - - - Fields - - - - - -

    // Movement events
    private readonly UnityEvent m_OnEnableMovement = new();
    private readonly UnityEvent m_OnDisableMovement = new();
    private readonly UnityEvent m_OnEnableRotation = new();
    private readonly UnityEvent m_OnDisableRotation = new();
    
    // Block events
    private readonly UnityEvent m_OnBlockDodge = new();
    private readonly UnityEvent m_OnStartDodge = new();
    private readonly UnityEvent m_OnEndDodge = new();
    private readonly UnityEvent m_OnResetDodge = new();

    // These events are ambiguous
    private readonly UnityEvent m_OnLockMoveInput = new();
    private readonly UnityEvent m_OnUnlockMoveInput = new();

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public UnityEvent OnEnableMovement => this.m_OnEnableMovement;

    public UnityEvent OnDisableMovement => this.m_OnDisableMovement;

    public UnityEvent OnEnableRotation => this.m_OnEnableRotation;

    public UnityEvent OnDisableRotation => this.m_OnDisableRotation;

    public UnityEvent OnBlockDodge => this.m_OnBlockDodge;
    
    public UnityEvent OnStartDodge => this.m_OnStartDodge;
    
    public UnityEvent OnEndDodge => this.m_OnEndDodge;
    
    public UnityEvent OnResetDodge => this.m_OnResetDodge;

    public UnityEvent OnLockMoveInput => this.m_OnLockMoveInput;

    public UnityEvent OnUnlockMoveInput => this.m_OnUnlockMoveInput;
    
    #endregion Properties

    #region - - - - - - Methods - - - - - -

    // ----------------------------------------------
    // Directly invoked by Unity's animation control
    // ----------------------------------------------
    
    public void EnableMovement()
        => this.m_OnEnableMovement.Invoke();

    public void DisableMovement()
        => this.m_OnDisableMovement.Invoke();

    public void EnableRotation()
        => this.m_OnEnableRotation.Invoke();

    public void DisableRotation()
        => this.m_OnDisableRotation.Invoke();
    
    public void BlockDodge() 
        => this.m_OnBlockDodge.Invoke();

    public void StartDodge() 
        => this.m_OnStartDodge.Invoke();

    public void EndDodge() 
        => this.m_OnEndDodge.Invoke();

    public void ResetDodge() 
        => this.m_OnResetDodge.Invoke();

    public void LockMoveInput()
        => this.m_OnLockMoveInput.Invoke();

    public void UnlockMoveInput()
        => this.m_OnUnlockMoveInput.Invoke();

    #endregion Methods

}
