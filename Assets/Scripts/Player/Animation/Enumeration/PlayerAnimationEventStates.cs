using System;
using ThatOneSamuraiGame.Scripts.Enumeration;
using UnityEngine;

public class PlayerAnimationEventStates : SmartEnum
{

    #region - - - - - - Fields - - - - - -
    
    // LockOn animation states
    public static PlayerAnimationEventStates StartLockOn = new("LockedOn", 0, 
        (animator, _, _) => animator.SetBool("LockedOn", true));
    public static PlayerAnimationEventStates EndLockOn = new("LockOn", 1,
        (animator, _, _) => animator.SetBool("LockedOn", false));

    // Finisher animation states
    public static PlayerAnimationEventStates StartHeavyAttackHeld = new("HeavyAttackHeld", 1, 
        (animator, _, _) => animator.SetBool("HeavyAttackHeld", true));
    public static PlayerAnimationEventStates EndHeavyAttachHeld = new("HeavyAttackHeld", 2, 
        (animator, _, _) => animator.SetBool("HeavyAttackHeld", false));

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    private PlayerAnimationEventStates(
        string name,
        int value,
        Action<Animator, bool, float> action)
        : base(name, value) 
        => this.Action = action;

    #endregion Constructors

    #region - - - - - - Properties - - - - - -

    public Action<Animator, bool, float> Action { get; private set; }

    #endregion Properties

}
