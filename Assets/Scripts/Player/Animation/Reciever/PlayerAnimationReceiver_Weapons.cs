using UnityEngine;
using UnityEngine.Events;

public interface IWeaponAnimationEvents
{

    #region - - - - - - Properties - - - - - -

    UnityEvent OnRevealWeapons { get; }
    
    UnityEvent OnHideWeapons { get; }
    
    UnityEvent<float> OnPlayWeaponEffect { get; }

    #endregion Properties
  
}

public class PlayerAnimationReceiver_Weapons: MonoBehaviour, IWeaponAnimationEvents
{

    #region - - - - - - Fields - - - - - -

    private readonly UnityEvent m_OnRevealWeapons = new();
    private readonly UnityEvent m_OnHideWeapons = new();
    private readonly UnityEvent<float> m_OnPlayWeaponEffect = new();

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public UnityEvent OnRevealWeapons 
        => this.m_OnRevealWeapons;

    public UnityEvent OnHideWeapons 
        => this.m_OnHideWeapons;

    public UnityEvent<float> OnPlayWeaponEffect
        => this.m_OnPlayWeaponEffect;

    #endregion Properties
  
    #region - - - - - - Methods - - - - - -

    // ----------------------------------------------
    // Directly invoked by Unity's animation control
    // ----------------------------------------------
    
    public void RevealWeapons() 
        => this.m_OnRevealWeapons.Invoke();

    public void HideWeapons() 
        => this.m_OnHideWeapons.Invoke();

    public void BeginWeaponEffect(float attackAngle)
        => this.m_OnPlayWeaponEffect.Invoke(attackAngle);

    public void BeginSwordEffect(float attackAngle)
        => this.m_OnPlayWeaponEffect.Invoke(attackAngle);

    #endregion Methods

}
