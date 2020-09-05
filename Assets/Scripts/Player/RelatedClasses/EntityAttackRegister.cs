using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Responsible for handling damage application 'to target' during combat.
/// This is typically called through the attacker and not the victim.
/// </summary>
public class EntityAttackRegister
{
    private EntityType _attachedEntityType;
    private GameObject _attachedEntity;
    private WSwordEffect _attachedSword; //TODO: May need to switch to an interface in the future

    /// <summary>
    /// The Init method for the Attack Register
    /// </summary>
    public void Init(GameObject attachedEntity, EntityType attachedType, WSwordEffect sword)
    {
        this._attachedEntityType = attachedType;
        this._attachedEntity = attachedEntity;
        this._attachedSword = sword;
    }

    /// <summary>
    /// Registers attack to the intended target after colliding.
    /// </summary>
    public void RegisterAttackTarget(IDamageable attackedEntity, Collider collider, float damage, bool canEffect)
    {
        //Filters out attached entity type
        if (attackedEntity.GetEntityType() == _attachedEntityType) return;

        if (attackedEntity.GetEntityType() == EntityType.Player)
            RegisterPlayer(collider, canEffect);

        if (attackedEntity.GetEntityType() == EntityType.Enemy)
            RegisterEnemy(attackedEntity, collider, damage, canEffect);

        if (attackedEntity.GetEntityType() == EntityType.Destructible)
            RegisterDestructible(collider, canEffect);
    }

    // Summary: Registers damage to player (NOT DEVELOPED)
    //
    private void RegisterPlayer(Collider collider, bool canEffect)
    {
        if (!canEffect) return;
        _attachedSword.CreateImpactEffect(collider.transform, HitType.DamageableTarget);
    }

    // Summary: Registers damage to enemy
    //
    private void RegisterEnemy(IDamageable target, Collider collider, float damage, bool canEffect)
    {
        Debug.Log(">> EntityAttackRegister: Logged Enemy");
        target.OnEntityDamage(damage, _attachedEntity);

        if (!canEffect) return;
        _attachedSword.CreateImpactEffect(collider.transform, HitType.DamageableTarget);
    }

    // Summary: Registers damage to destructible object
    //
    private void RegisterDestructible(Collider collider, bool canEffect)
    {
        Debug.Log(">> EntityAttackRegister: Logged Destructable");

        if (!canEffect) return;
        _attachedSword.CreateImpactEffect(collider.transform, HitType.DamageableTarget);
    }
}
