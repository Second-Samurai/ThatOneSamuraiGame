using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIAnimationTimeData
{
    public float currentFrame;
    public string currentClip;
    public bool bPlayerFound;
    public bool bIsLightAttacking;
    public bool bIsApproaching;
    public bool bIsGuardBroken;
    public bool bIsDead;
    public bool IsQuickBlocking;


    public AIAnimationTimeData(float _currentFrame, string _currentClip, bool b_PlayerFound, bool b_IsLightAttacking, bool b_IsApproaching, bool _IsGuardBroken, bool _IsDead, bool _IsQuickBlocking) 
    {
        currentFrame = _currentFrame;
        currentClip = _currentClip;
        bPlayerFound = b_PlayerFound;
        bIsLightAttacking = b_IsLightAttacking;
        bIsApproaching = b_IsApproaching;
        bIsGuardBroken = _IsGuardBroken;
        bIsDead = _IsDead;
        IsQuickBlocking = _IsQuickBlocking;
    }
}
