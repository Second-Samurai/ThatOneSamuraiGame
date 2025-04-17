using UnityEngine;
using UnityEngine.Events;

// TODO: Convert users to instead use an interface implementation containing its properties
public class ArcherAnimationReciever : MonoBehaviour
{

    #region - - - - - - Properties - - - - - -

    public UnityEvent OnIdleCompletion { get; } = new();
    
    public UnityEvent OnTurnCompletion { get; } = new();

    public UnityEvent OnDrawBow { get; } = new();

    public UnityEvent OnBowRelease { get; } = new();

    public UnityEvent OnBowEquip { get; } = new();

    public UnityEvent OnBowDisarm { get; } = new();

    #endregion Properties

    #region - - - - - - Methods - - - - - -

    public void CompleteIdleClip()
        => this.OnIdleCompletion.Invoke();

    public void CompleteTurnClip()
        => this.OnTurnCompletion.Invoke();

    public void DrawBow()
        => this.OnDrawBow.Invoke();

    public void ReleaseBow() 
        => this.OnBowRelease.Invoke();

    public void EquipBow()
        => this.OnBowEquip.Invoke();

    public void DisarmBow()
        => this.OnBowDisarm.Invoke();

    #endregion Methods

}
