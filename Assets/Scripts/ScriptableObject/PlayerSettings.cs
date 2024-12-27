using UnityEngine;

[CreateAssetMenu(menuName = "Settings/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Player Data")]
    public EntityStatData playerStats;

    [Header("Movement Modifiers")]
    public float gravity;
    public float stepDown;

    [Space]
    public float slideSpeed;
    [Range(0.1f, 0.5f)]
    public float slideDuration;

    [Header("Enemy Guide Sensors")]
    public float detectionRadius = 10;
    public float minimumAttackDist = 2f;
    public float forwardDotLimit = 0.8f;
}
