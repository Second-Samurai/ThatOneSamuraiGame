using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public interface IDamageable {
    EntityType GetEntityType();
    bool CheckCanDamage();

    void OnEntityDamage(float damage, GameObject attacker, bool unblockable);
    void DisableDamage();
    void EnableDamage();
}

public class PDamageController : MonoBehaviour, IDamageable
{
    StatHandler playerStats;
    PlayerFunctions _functions;

    private bool _isDamageDisabled = false;

    public void Init(StatHandler playerStats) {
        this.playerStats = playerStats;
    }

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        if (_isDamageDisabled) return;
        _functions.ApplyHit(attacker, unblockable);
        Debug.Log("Player is Damaged");
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
        _functions = GetComponent<PlayerFunctions>();
    }

    public bool CheckCanDamage()
    {
        return _isDamageDisabled;
    }

    public EntityType GetEntityType()
    {
        return EntityType.Player;
    }
}
