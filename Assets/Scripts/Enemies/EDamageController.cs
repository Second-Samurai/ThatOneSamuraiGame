using Enemies;
using UnityEngine;

public class EDamageController : MonoBehaviour, IDamageable
{
    StatHandler _enemyStats;
    AISystem aiSystem;

    private bool _isDamageDisabled = false;

    public void Init(StatHandler enemyStats) {
        _enemyStats = enemyStats;
    }

    public void OnEntityDamage(float damage, GameObject attacker)
    {
        if (_isDamageDisabled) return;
        
        if (attacker.layer == LayerMask.NameToLayer("Player"))
        {
            aiSystem.ApplyHit(attacker);
        }
        else
        {
            Debug.Log(attacker.layer.ToString());
        }
    }

    /* Summary: This disables the damage from this component.
     *          But can be only used when in a state that does
     *          not require it.*/
    //
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
}
