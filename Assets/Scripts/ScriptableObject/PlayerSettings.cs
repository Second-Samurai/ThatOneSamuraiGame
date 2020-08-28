using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Player Data")]
    public EntityStatData platerStats;
}

[System.Serializable]
public class EntityStatData 
{
    public float baseDamage;
    public float maxHealth;
}
