using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public EntityStats platerStats;
}

[System.Serializable]
public class EntityStats {
    public float baseDamage;
}
