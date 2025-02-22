using System;
using ThatOneSamuraiGame.Scripts.Enumeration;
using UnityEngine;

/// <summary>
/// Represents the state and animation behaviour of the Player.
/// </summary>
public class PlayerAnimationEventStates : SmartEnum
{

    #region - - - - - - Fields - - - - - -
    
    // Weapon animation states
    public static PlayerAnimationEventStates DrawSword = new("DrawSword", 0, "Sword_Draw",
        (animator, _, _, _, state) => animator.SetTrigger(state.ToString()));
    public static PlayerAnimationEventStates SheathSword = new("SheathSword", 0, "Sword_Sheathing",
        (animator, _, _, _, state) => animator.SetTrigger(state.ToString()));
    
    // Attack animation states
    public static PlayerAnimationEventStates SprintAttack = new("SprintAttack", 0, "JumpSlashEDIT",
        (animator, _, _, _, state) => animator.SetTrigger(state.ToString()));
    public static PlayerAnimationEventStates ResetLightAttack = new("AttackLight", 0, "",
        (animator, _, _, _, state) => animator.ResetTrigger(state.ToString()));
    public static PlayerAnimationEventStates AttackLight = new("AttackLight", 0, "AttackEDIT", // clip-name as two clips belong to same attack.
        (animator, _, _, attackVariant, state) =>
        {
            animator.SetTrigger(state.ToString());

            if (attackVariant == 1)
            {
                FirstAttack.Run(animator, boolValue: true);
                SecondAttack.Run(animator, boolValue: false);
            } 
            else if (attackVariant == 2)
            {
                FirstAttack.Run(animator, boolValue: false);
                SecondAttack.Run(animator, boolValue: true);
            }
        });
    public static PlayerAnimationEventStates FirstAttack = new("FirstAttack", 0, "",
        (animator, enabled, _, _, state) => animator.SetBool(state.ToString(), enabled));
    public static PlayerAnimationEventStates SecondAttack = new("SecondAttack", 1, "",
        (animator, enabled, _, _, state) => animator.SetBool(state.ToString(), enabled));
    
    // LockOn animation states
    public static PlayerAnimationEventStates StartLockOn = new("LockedOn", 0, "",
        (animator, _, _, _, state) => animator.SetBool(state.ToString(), true));
    public static PlayerAnimationEventStates EndLockOn = new("LockOn", 1, "",
        (animator, _, _, _, state) => animator.SetBool(state.ToString(), false));

    // Finisher animation states
    public static PlayerAnimationEventStates StartHeavyAttackHeld = new("HeavyAttackHeld", 1, "",
        (animator, _, _, _, state) => animator.SetBool(state.ToString(), true));
    public static PlayerAnimationEventStates EndHeavyAttachHeld = new("HeavyAttackHeld", 2, "",
        (animator, _, _, _, state) => animator.SetBool(state.ToString(), false));

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    private PlayerAnimationEventStates(
        string name,
        int value,
        string clipName,
        Action<Animator, bool, float, int, PlayerAnimationEventStates> action)
        : base(name, value)
    {
        this.Action = action;
        this.ClipName = clipName;
    }

    #endregion Constructors

    #region - - - - - - Properties - - - - - -

    private Action<Animator, bool, float, int, PlayerAnimationEventStates> Action { get; set; }

    public string ClipName { get; private set; }

    #endregion Properties

    #region - - - - - - Methods - - - - - -

    public void Run(Animator animator, bool boolValue = false, float floatValue = 0f, int intValue = 0)
        => this.Action.Invoke(animator, boolValue, floatValue, intValue, this);

    #endregion Methods


}
