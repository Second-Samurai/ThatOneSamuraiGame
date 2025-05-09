﻿using UnityEngine;
using UnityEngine.Events;

public class ArcherAnimationReciever : MonoBehaviour
{

    #region - - - - - - Properties - - - - - -

    public UnityEvent OnIdleCompletion { get; } = new();
    
    public UnityEvent OnTurnCompletion { get; } = new();

    public UnityEvent OnDrawBow { get; } = new();

    public UnityEvent OnBowRelease { get; } = new();

    public UnityEvent OnBowEquip { get; } = new();

    public UnityEvent OnBowDisarm { get; } = new();

    // public UnityEvent OnEnableRagdoll { get; } = new();
    //
    // public UnityEvent OnDeath { get; } = new();

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

    // public void EnableRagdoll()
    //     => this.OnEnableRagdoll.Invoke();
    //
    // public void KillArcher()
    //     => this.OnDeath.Invoke();

    #endregion Methods

}
