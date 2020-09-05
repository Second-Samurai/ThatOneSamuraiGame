using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityStatData 
{
    public float baseDamage;
    public float maxHealth;
    public float maxGuard;
    public float currentGuard;
    public float moveSpeed;
}

/// <summary>
/// Defines which 'type' of combat your entity may be on.
/// </summary>
public enum EntityCombatType
{
    Attack,
    Block
}

/// <summary>
/// Used to check which entity/actor type 'this' object is.
/// </summary>
public enum EntityType
{
    Player,
    Enemy,
    Destructible
}
