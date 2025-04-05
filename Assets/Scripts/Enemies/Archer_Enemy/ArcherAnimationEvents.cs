using System;
using UnityEngine;

public class ArcherAnimationEvents : AnimationEventsEnum
{

    #region - - - - - - Fields - - - - - -

    public static ArcherAnimationEvents ArcherIdle = new("ArcherIdle", 0, "", (animator, _, _, index, _) =>
    {
        Vector2 _BlendDirection = Vector2.zero;
        switch (index)
        {
            case 0:
                _BlendDirection = new Vector2(-1, -1);
                break;
            case 1:
                _BlendDirection = new Vector2(-1, 1);
                break;
            case 2:
                _BlendDirection = new Vector2(1, -1);
                break;
            case 3:
                _BlendDirection = new Vector2(1, 1);
                break;
        }
        
        animator.SetFloat(IdleBlendX, _BlendDirection.x);
        animator.SetFloat(IdleBlendY, _BlendDirection.y);
    });
    public static ArcherAnimationEvents EquipBow = new("Equip_Bow", 1, "equip-bow", (animator, _, _, _, _) =>
        animator.SetTrigger(EquipBow));
    public static ArcherAnimationEvents DisarmBow = new("Disarm_Bow", 2, "disarm-bow", (animator, _, _, _, _) =>
        animator.SetTrigger(DisarmBow));
    public static ArcherAnimationEvents FireBow = new("Fire_Bow", 3, "release", (animator, _, _, _, _) =>
        animator.SetTrigger(FireBow));
    public static ArcherAnimationEvents DrawBow = new("Draw_Bow", 3, "draw-reload", (animator, _, _, _, _) =>
        animator.SetTrigger(DrawBow));

    private static readonly int IdleBlendX = Animator.StringToHash("Idle_Blend_X");
    private static readonly int IdleBlendY = Animator.StringToHash("Idle_Blend_Y");

    #endregion Fields
    
    #region - - - - - - Constructors - - - - - -

    public ArcherAnimationEvents(
        string name,
        int value,
        string clipName,
        Action<Animator, bool, float, int, Vector2> action)
        : base(name, value, clipName, action) { }

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    public static implicit operator string(ArcherAnimationEvents animationEvent)
        => animationEvent.ToString();

    #endregion Methods

}
