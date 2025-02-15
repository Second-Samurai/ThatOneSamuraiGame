using UnityEngine;
using UnityEngine.Events;

public interface IWeaponAnimationEvents
{

    #region - - - - - - Properties - - - - - -

    UnityEvent OnRevealWeapons { get; }
    
    UnityEvent OnHideWeapons { get; }

    #endregion Properties
  
}

public partial class PlayerAnimationReceiver : MonoBehaviour, IWeaponAnimationEvents
{

    #region - - - - - - Fields - - - - - -

    private readonly UnityEvent m_OnRevealWeapons = new();
    private readonly UnityEvent m_OnHideWeapons = new();

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public UnityEvent OnRevealWeapons 
        => this.m_OnRevealWeapons;

    public UnityEvent OnHideWeapons 
        => this.m_OnHideWeapons;

    #endregion Properties
  
    #region - - - - - - Methods - - - - - -

    // Directly invoked by Unity's animation control
    public void RevealWeapons() 
        => this.m_OnRevealWeapons.Invoke();

    // Directly invoked by Unity's animation control
    public void HideWeapons() 
        => this.m_OnHideWeapons.Invoke();

    #endregion Methods

}
