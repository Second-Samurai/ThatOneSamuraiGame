using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [Header("Enemy Class Stats")]
    public EnemyStats swordsmanStats;
    public EnemyStats archerStats;
    public EnemyStats glaiveWielderStats;
    public EnemyStats tutorialEnemyStats;
    public EnemyStats bossStats;
    
    [Header("Range Checks")]
    public float longRange;
    public float midRange;
    public float shortRange;
    public float veryShortRange;
    
    [Header("Impatience Time intervals")]
    public float minImpatienceTime;
    public float maxImpatienceTime;

    // Scriptable object items saved in runtime are to be made private
    private Transform _target;

    public Transform GetTarget()
    {
        return _target;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
    
    // Assign stats based on the enemy type
    public EnemyStats GetEnemyStatType(EnemyType enemyType)
    {
        // enemySettings.enemyData = initial scriptable objects values
            
        switch (enemyType)
        {
            case EnemyType.SWORDSMAN:
                return swordsmanStats;
            case EnemyType.ARCHER:
                return archerStats;
            case EnemyType.GLAIVEWIELDER:
                return glaiveWielderStats;
            case EnemyType.TUTORIALENEMY:
                return tutorialEnemyStats;
            case EnemyType.BOSS:
                return bossStats;
            case EnemyType.MINIBOSS:
                return bossStats;
            default:
                Debug.LogError("Error: Could not find suitable enemy type");
                break;
        }

        return null;
    }
}
