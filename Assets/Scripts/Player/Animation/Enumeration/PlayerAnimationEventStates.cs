using System;
using ThatOneSamuraiGame.Scripts.Enumeration;
using UnityEngine;

public class PlayerAnimationEventStates : SmartEnum
{

    #region - - - - - - Fields - - - - - -
    
    // Weapon animation states
    public static PlayerAnimationEventStates DrawSword = new("DrawSword", 0,
        (animator, _, _, state) => animator.SetTrigger(state.ToString()));
    
    // Attach animation states
    public static PlayerAnimationEventStates ResetLightAttack = new("AttackLight", 0,
        (animator, _, _, state) => animator.ResetTrigger(state.ToString()));
    
    // LockOn animation states
    public static PlayerAnimationEventStates StartLockOn = new("LockedOn", 0, 
        (animator, _, _, state) => animator.SetBool(state.ToString(), true));
    public static PlayerAnimationEventStates EndLockOn = new("LockOn", 1,
        (animator, _, _, state) => animator.SetBool(state.ToString(), false));

    // Finisher animation states
    public static PlayerAnimationEventStates StartHeavyAttackHeld = new("HeavyAttackHeld", 1, 
        (animator, _, _, state) => animator.SetBool(state.ToString(), true));
    public static PlayerAnimationEventStates EndHeavyAttachHeld = new("HeavyAttackHeld", 2, 
        (animator, _, _, state) => animator.SetBool(state.ToString(), false));

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    private PlayerAnimationEventStates(
        string name,
        int value,
        Action<Animator, bool, float, PlayerAnimationEventStates> action)
        : base(name, value) 
        => this.Action = action;

    #endregion Constructors

    #region - - - - - - Properties - - - - - -

    private Action<Animator, bool, float, PlayerAnimationEventStates> Action { get; set; }

    #endregion Properties


    #region - - - - - - Methods - - - - - -

    public void Run(Animator animator, bool boolValue, float floatValue)
        => this.Action.Invoke(animator, boolValue, floatValue, this);

    #endregion Methods


}
