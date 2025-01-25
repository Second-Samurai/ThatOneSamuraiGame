using System;
using ThatOneSamuraiGame.Scripts.Enumeration;
using UnityEngine;

public class PlayerAnimationEventStates : SmartEnum
{

    #region - - - - - - Fields - - - - - -
    
    // LockOn animation events
    public static PlayerAnimationEventStates LockOn = new("LockedOn", 1, 
        (animator, boolValue, _) => animator.SetBool("LockedOn", boolValue));

    // Finisher animation events
    public static PlayerAnimationEventStates StartHeavyAttackHeld = new("HeavyAttackHeld", 0, 
        (animator, _, _) => animator.SetBool("HeavyAttackHeld", true));
    public static PlayerAnimationEventStates EndHeavyAttachHeld = new("HeavyAttackHeld", 0, 
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
