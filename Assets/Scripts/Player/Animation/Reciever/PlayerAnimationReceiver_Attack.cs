using UnityEngine;
using UnityEngine.Events;

public interface IAttackAnimationEvents
{

    #region - - - - - - Properties - - - - - -

    UnityEvent OnAttackStart { get; }
    
    UnityEvent OnAttackEnd { get; }
    
    UnityEvent OnAttackComboReset { get; }
    
    UnityEvent OnHeavyAttack { get; }
    
    UnityEvent<float> OnForwardImpulse { get; }
    
    UnityEvent<float> OnForwardJumpImpulse { get; }
    
    UnityEvent OnParryStunStateStart { get; }
    
    UnityEvent OnParryStunStateEnd { get; }

    #endregion Properties
  
}

/// <summary>
/// Receives invocations from Unity's Animation Events.
/// </summary>
public class PlayerAnimationReceiver_Attack : MonoBehaviour, IAttackAnimationEvents
{

    #region - - - - - - Fields - - - - - -

    // Main attack events
    private readonly UnityEvent m_OnAttackStart = new();
    private readonly UnityEvent m_OnAttackEnd = new();
    private readonly UnityEvent m_OnAttackComboReset = new();
    private readonly UnityEvent m_OnHeavyAttack = new();
    
    // TODO: Both these events are the same, should be combined
    private readonly UnityEvent<float> m_OnForwardImpulse = new();
    private readonly UnityEvent<float> m_OnForwardJumpImpulse = new();
    
    // Stun State events
    private readonly UnityEvent m_OnParryStunStateStart = new();
    private readonly UnityEvent m_OnParryStunStateEnd = new();

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public UnityEvent OnAttackStart => this.m_OnAttackStart;

    public UnityEvent OnAttackEnd => this.m_OnAttackEnd;

    public UnityEvent OnAttackComboReset => this.m_OnAttackComboReset;

    public UnityEvent OnHeavyAttack => this.m_OnHeavyAttack;

    public UnityEvent<float> OnForwardImpulse => this.m_OnForwardImpulse;

    public UnityEvent<float> OnForwardJumpImpulse => this.m_OnForwardJumpImpulse;

    public UnityEvent OnParryStunStateStart => this.m_OnParryStunStateStart;

    public UnityEvent OnParryStunStateEnd => this.m_OnParryStunStateEnd;

    #endregion Properties

    #region - - - - - - Methods - - - - - -

    // ----------------------------------------------
    // Directly invoked by Unity's animation control
    // ----------------------------------------------
    
    public void BeginAttacking()
        => this.m_OnAttackStart.Invoke();

    public void EndAttacking()
        => this.m_OnAttackEnd.Invoke();
    
    // TODO: This is reliant on the animation completing. This should be timer based instead.
    public void ResetAttackCombo()
        => this.m_OnAttackComboReset.Invoke();

    public void PlayHeavySwing()
        => this.m_OnHeavyAttack.Invoke();

    // ---------------------------------------------------------------
    // TODO: Both these events are the same, should be combined
    // ---------------------------------------------------------------
    public void ForwardImpulse(float force)
        => this.m_OnForwardImpulse.Invoke(force);

    public void JumpImpulseWithTimer(float timer)
        => this.m_OnForwardJumpImpulse.Invoke(timer);

    public void StartParryStunState()
        => this.m_OnParryStunStateStart.Invoke();
    
    public void EndParryStunState()
        => this.m_OnParryStunStateEnd.Invoke();

    public void CheckCombo() { } // Included to provide a receiver for the animation event. Event is no longer needed.

    #endregion Methods

}
