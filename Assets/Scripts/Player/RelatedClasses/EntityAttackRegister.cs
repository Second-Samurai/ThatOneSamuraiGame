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

    /// <summary>
    /// The Init method for the Attack Register
    /// </summary>
    public void Init(GameObject attachedEntity, EntityType attachedType)
    {
        this._attachedEntityType = attachedType;
        this._attachedEntity = attachedEntity;
    }

    /// <summary>
    /// Registers attack to the intended target after colliding.
    /// </summary>
    public void RegisterAttackTarget(IDamageable attackedEntity, WSwordEffect swordEffect, Collider collider, float damage, bool canEffect, bool unBlockable)
    {
        //Filters out attached entity type
        if (attackedEntity.GetEntityType() == _attachedEntityType) return;

        if (attackedEntity.GetEntityType() == EntityType.Player)
            RegisterPlayer(collider, swordEffect, canEffect, unBlockable);

        if (attackedEntity.GetEntityType() == EntityType.Enemy)
            RegisterEnemy(attackedEntity, swordEffect, collider, damage, canEffect, unBlockable);

        if (attackedEntity.GetEntityType() == EntityType.Destructible)
            RegisterDestructible(attackedEntity, swordEffect, damage, collider, canEffect, unBlockable);
    }

    // Summary: Registers damage to player (NOT DEVELOPED)
    //
    private void RegisterPlayer(Collider collider, WSwordEffect swordEffect, bool canEffect, bool unBlockable)
    {
        if (!canEffect) return;
        swordEffect.CreateImpactEffect(collider.transform, HitType.DamageableTarget);
    }

    // Summary: Registers damage to enemy
    //
    private void RegisterEnemy(IDamageable target, WSwordEffect swordEffect, Collider collider, float damage, bool canEffect, bool unBlockable)
    {
        target.OnEntityDamage(damage, _attachedEntity, unBlockable);

        if (!canEffect) return;
        swordEffect.CreateImpactEffect(collider.transform, HitType.DamageableTarget);
    }

    // Summary: Registers damage to destructible object
    //
    private void RegisterDestructible(IDamageable target, WSwordEffect swordEffect, float damage, Collider collider, bool canEffect, bool unBlockable)
    {
        target.OnEntityDamage(damage, _attachedEntity, unBlockable);

        if (!canEffect) return;
        swordEffect.CreateImpactEffect(collider.transform, HitType.DamageableTarget);
    }
}
