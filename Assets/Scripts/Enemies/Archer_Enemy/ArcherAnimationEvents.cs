﻿using System;
using UnityEngine;

public class ArcherAnimationEvents : AnimationEventsEnum
{

    #region - - - - - - Fields - - - - - -

    // Idle Events
    public static ArcherAnimationEvents ArcherIdle = new("ArcherIdle", 0, "", (animator, _, _, index, _) =>
    {
        Vector2 _BlendDirection = Vector2.zero;
        switch (index)
        {
            case 0:
                _BlendDirection = new Vector2(-1, -1);
                break;
            case 1:
                _BlendDirection = new Vector2(-1, 1.2f);
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
    
    // Combat Events
    public static ArcherAnimationEvents EquipBow = new("Equip_Bow", 1, "equip-bow", (animator, _, _, _, _) =>
        animator.SetTrigger(EquipBow));
    public static ArcherAnimationEvents DisarmBow = new("Disarm_Bow", 2, "disarm-bow", (animator, _, _, _, _) =>
        animator.SetTrigger(DisarmBow));
    public static ArcherAnimationEvents FireBow = new("Fire_Bow", 3, "release", (animator, _, _, _, _) =>
        animator.SetTrigger(FireBow));
    public static ArcherAnimationEvents DrawBow = new("Draw_Bow", 3, "draw-reload", (animator, _, _, _, _) =>
        animator.SetTrigger(DrawBow));
    
    // Movement Events
    public static ArcherAnimationEvents TurnLeft = new("TurnLeft", 5, "archer-turn-left", (animator, _, _, _, _) =>
        animator.SetTrigger(TurnLeft));
    public static ArcherAnimationEvents TurnRight = new("TurnRight", 6, "archer-turn-right", (animator, _, _, _, _) =>
        animator.SetTrigger(TurnRight));
    public static ArcherAnimationEvents SetLegsLayerOverride = new("SetLegsLayerOverride", 7, "",
        (animator, _, weight, _, _) => animator.SetLayerWeight(1, weight));
    
    // Death Events
    public static ArcherAnimationEvents ArcherDeath = new("OnDeath", 8, "", (animator, _, _, _, _) =>
        animator.SetTrigger(ArcherDeath));

    public static ArcherAnimationEvents DeathDirection = new("DeathDirection", 9, "", (animator, _, _, index, _) =>
    {
        Vector2 _BlendDirection = Vector2.zero;
        switch (index)
        {
            case 0: // Forward
                _BlendDirection = new Vector2(0, 1);
                break;
            case 1: // Backward
                _BlendDirection = new Vector2(0, -1);
                break;
            case 2: // Left
                _BlendDirection = new Vector2(-1, 0);
                break;
            case 3: // Right
                _BlendDirection = new Vector2(1, 0);
                break;
        }

        animator.SetFloat(DeathBlendX, _BlendDirection.x);
        animator.SetFloat(DeathBlendY, _BlendDirection.y);
    });

    // Directional Blend Parameters
    private static readonly int IdleBlendX = Animator.StringToHash("Idle_Blend_X");
    private static readonly int IdleBlendY = Animator.StringToHash("Idle_Blend_Y");
    private static readonly int DeathBlendX = Animator.StringToHash("Death_Blend_X");
    private static readonly int DeathBlendY = Animator.StringToHash("Death_Blend_Y");

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
