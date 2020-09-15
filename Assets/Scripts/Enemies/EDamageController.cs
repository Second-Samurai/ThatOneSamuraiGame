using Enemies;
using UnityEngine;

public class EDamageController : MonoBehaviour, IDamageable
{
    StatHandler _enemyStats;
    AISystem aiSystem;

    [HideInInspector] public Guarding enemyGuard;

    private bool _isDamageDisabled = false;

    public void Init(StatHandler enemyStats) {
        _enemyStats = enemyStats;

        enemyGuard = this.gameObject.AddComponent<Guarding>();
        enemyGuard.Init(_enemyStats);
    }

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        if (!unblockable)
        {

            if (_isDamageDisabled) return;

            if (attacker.layer == LayerMask.NameToLayer("Player"))
            {
                if (enemyGuard.CheckIfEntityGuarding(damage))
                {
                    aiSystem.OnQuickBlock();
                    return;
                }

                if(enemyGuard.isStunned) attacker.GetComponentInChildren<LockOnTargetManager>().EndGuardBreakCam();

                aiSystem.ApplyHit(attacker);
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
            aiSystem.ApplyHit(attacker);
        }
    }

    /* Summary: This disables the damage from this component.
     *          But can be only used when in a state that does
     *          not require it.*/
    //

    public void OnParried(float damage)
    {
        enemyGuard.CheckIfEntityGuarding(damage);
        aiSystem.animator.SetTrigger("Parried");
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
        aiSystem = GetComponent<AISystem>();
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
