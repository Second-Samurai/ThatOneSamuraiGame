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
    [SerializeField] private bool GodMode = false;
    [SerializeField] private bool _canDamage = false; //Change to private later on

    private StatHandler playerStats;
    private PlayerFunctions _functions;

    public void Init(StatHandler playerStats) {
        this.playerStats = playerStats;
    }

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        if (!_canDamage) return;
        _functions.ApplyHit(attacker, unblockable, damage);
    }

    /* Summary: This disables the damage from this component.
     *          But can be only used when in a state that does
     *          not require it.*/
    //
    public void DisableDamage()
    {
        _canDamage = false;
    }

    public void EnableDamage()
    {
        if (GodMode) return;
        _canDamage = true;
    }

    private void Start()
    {
        _functions = GetComponent<PlayerFunctions>();
    }

    public bool CheckCanDamage()
    {
        return _canDamage;
    }

    public EntityType GetEntityType()
    {
        return EntityType.Player;
    }
}
