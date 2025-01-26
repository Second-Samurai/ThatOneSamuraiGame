using System;
using ThatOneSamuraiGame.Scripts.Enumeration;
using UnityEngine;

public class PlayerAnimationCheckState : SmartEnum
{

    #region - - - - - - Fields - - - - - -

    // Player LockOn checks
    public static PlayerAnimationCheckState IsLockedOn = new("LockedOn", 0, animator => animator.GetBool("LockedOn"));

    // Player Heavy Attack checks
    public static PlayerAnimationCheckState HeavyAttackHeld =
        new("HeavyAttackHeld", 1, animator => animator.GetBool("HeavyAttackHeld"));
    
    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    private PlayerAnimationCheckState(
        string name, 
        int value, 
        Func<Animator, bool> checkAction = null) 
        : base(name, value) 
        => this.CheckAction = checkAction;

    #endregion Constructors

    #region - - - - - - Properties - - - - - -

    public Func<Animator, bool> CheckAction { get; private set; } 

    #endregion Properties
  
}
