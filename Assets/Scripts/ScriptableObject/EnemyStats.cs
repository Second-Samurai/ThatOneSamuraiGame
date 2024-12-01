using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Space]
    public EntityStatData enemyData;
    public float circleSpeed;
    public float dodgeForce;
    public float guardCooldown;
    
    [Header("Blocking Time intervals")]
    public float minBlockTime;
    public float maxBlockTime;
}
