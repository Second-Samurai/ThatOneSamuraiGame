using Enemies;
using UnityEngine;

public class EDamageController : MonoBehaviour, IDamageable
{
    private StatHandler _enemyStats;
    private AISystem _aiSystem;

    [HideInInspector] public Guarding enemyGuard;

    private bool _isDamageDisabled = false;

    public void Init(StatHandler enemyStats) {
        _enemyStats = enemyStats;

        enemyGuard = this.gameObject.AddComponent<Guarding>();
        enemyGuard.Init(_enemyStats);
    }

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        if (_isDamageDisabled) return;
        
        if (!unblockable)
        {
            if (attacker.layer == LayerMask.NameToLayer("Player"))
            {
                // Summary: Perform an enemy parry if the enemy canParry
                // Currently canParry is only set true and false in BlockEnemyState script
                // and set to false in the ParryEnemyState script
                if (enemyGuard.canParry)
                {
                    _aiSystem.OnParry();
                    return;
                }
                
                // If enemy can guard and isn't stunned, reduce guard by damage amount and do the following
                if (enemyGuard.CheckIfEntityGuarding(damage))
                {
                    // If enemy still has left over guard meter AFTER CheckIfEntityGuarding, go to the quick block state
                    // The following 3 lines do not occur if the enemy is guard broken through the previous CheckIfEntityGuarding
                    if (enemyGuard.canGuard)
                    {
                        _aiSystem.OnQuickBlock();
                    }
                    return;
                }

                if(enemyGuard.isStunned) attacker.GetComponentInChildren<LockOnTargetManager>().EndGuardBreakCam();

                _aiSystem.ApplyHit(attacker);
            }
            else
            {
                Debug.Log(attacker.layer.ToString());
            }
        }
        else if (enemyGuard.isStunned && unblockable)
        {
            Debug.Log("FINISHER");
            attacker.GetComponentInChildren<LockOnTargetManager>().EndGuardBreakCam();
            attacker.GetComponentInChildren<FinishingMoveController>().PlayFinishingMove(gameObject);
            return;
        }
        else
        {
            Debug.LogWarning("WARNING: Attack not on player layer");
            //_aiSystem.ApplyHit(attacker);
        }
    }

    /* Summary: This disables the damage from this component.
     *          But can be only used when in a state that does
     *          not require it.*/
    //

    public void OnParried(float damage)
    {
        // Stun if enemy can guard
        if (enemyGuard.CheckIfEntityGuarding(damage))
        {
            _aiSystem.OnParryStun();
        }
        // Kill enemy if they cant
        else
        {
            _aiSystem.OnParryStun();
            //_aiSystem.OnEnemyDeath();
        }
    }

    public void DisableDamage()
    {
        _isDamageDisabled = true;
    }

    public void EnableDamage()
    {
        _isDamageDisabled = false;
    }

    private void Start()
    {
        _aiSystem = GetComponent<AISystem>();
    }

    public bool CheckCanDamage()
    {
        return _isDamageDisabled;
    }

    public EntityType GetEntityType()
    {
        return EntityType.Enemy;
    }
}
