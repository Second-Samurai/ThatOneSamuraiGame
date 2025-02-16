using UnityEngine;
using UnityEngine.Events;

public interface IAttackAnimationEvents
{

    #region - - - - - - Properties - - - - - -

    UnityEvent OnAttackStart { get; }
    
    UnityEvent OnAttackEnd { get; }
    
    UnityEvent OnAttackComboReset { get; }
    
    UnityEvent OnHeavyAttack { get; }

    #endregion Properties
  
}

/// <summary>
/// Receives invocations from Unity's Animation Events.
/// </summary>
public class PlayerAnimationReceiver_Attack : MonoBehaviour, IAttackAnimationEvents
{

    #region - - - - - - Fields - - - - - -

    private readonly UnityEvent m_OnAttackStart = new();
    private readonly UnityEvent m_OnAttackEnd = new();
    private readonly UnityEvent m_OnAttackComboReset = new();
    private readonly UnityEvent m_OnHeavyAttack = new();

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public UnityEvent OnAttackStart => this.m_OnAttackStart;

    public UnityEvent OnAttackEnd => this.m_OnAttackEnd;

    public UnityEvent OnAttackComboReset => this.m_OnAttackComboReset;

    public UnityEvent OnHeavyAttack => this.m_OnHeavyAttack;

    #endregion Properties

    #region - - - - - - Methods - - - - - -

    // Directly invoked by Unity's animation control
    public void BeginAttacking()
        => this.m_OnAttackStart.Invoke();

    // Directly invoked by Unity's animation control
    public void EndAttacking()
        => this.m_OnAttackEnd.Invoke();
    
    // Directly invoked by Unity's animation control
    // TODO: This is reliant on the animation completing. This should be timer based instead.
    public void ResetAttackCombo()
        => this.m_OnAttackComboReset.Invoke();

    // Directly invoked by Unity's animation control
    public void PlayHeavySwing()
        => this.m_OnHeavyAttack.Invoke();

    #endregion Methods

}
