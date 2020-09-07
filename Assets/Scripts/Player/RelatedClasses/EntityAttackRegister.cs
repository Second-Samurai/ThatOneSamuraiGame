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
    public void RegisterAttackTarget(IDamageable attackedEntity, Collider collider, float damage, bool canEffect, bool unBlockable)
    {
        //Filters out attached entity type
        if (attackedEntity.GetEntityType() == _attachedEntityType) return;

        if (attackedEntity.GetEntityType() == EntityType.Player)
            RegisterPlayer(collider, canEffect, unBlockable);

        if (attackedEntity.GetEntityType() == EntityType.Enemy)
            RegisterEnemy(attackedEntity, collider, damage, canEffect, unBlockable);

        if (attackedEntity.GetEntityType() == EntityType.Destructible)
            RegisterDestructible(attackedEntity, damage, collider, canEffect, unBlockable);
    }

    // Summary: Registers damage to player (NOT DEVELOPED)
    //
    private void RegisterPlayer(Collider collider, bool canEffect, bool unBlockable)
    {
        if (!canEffect) return;
        _attachedSword.CreateImpactEffect(collider.transform, HitType.DamageableTarget);
    }

    // Summary: Registers damage to enemy
    //
    private void RegisterEnemy(IDamageable target, Collider collider, float damage, bool canEffect, bool unBlockable)
    {
        Debug.Log(">> EntityAttackRegister: Logged Enemy");
        target.OnEntityDamage(damage, _attachedEntity, unBlockable);

        if (!canEffect) return;
        _attachedSword.CreateImpactEffect(collider.transform, HitType.DamageableTarget);
    }

    // Summary: Registers damage to destructible object
    //
    private void RegisterDestructible(IDamageable target, float damage, Collider collider, bool canEffect, bool unBlockable)
    {
        Debug.Log(">> EntityAttackRegister: Logged Destructable");
        target.OnEntityDamage(damage, _attachedEntity, unBlockable);

        if (!canEffect) return;
        _attachedSword.CreateImpactEffect(collider.transform, HitType.DamageableTarget);
    }
}
