using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class SwordsmanSwingTrigger : MonoBehaviour
{
    private AISystem _aiSystem;

    private void Start()
    {
        _aiSystem = GetComponentInParent<AISystem>();
    }

    //sees if the object is damagable
    private void OnTriggerEnter(Collider other)
    {
        DamageCheck(other);
    }

    //if the object is damagable, apply damage
    private void DamageCheck(Collider other)
    {
        IDamageable damagable = other.gameObject.GetComponent<IDamageable>();
        if (other.GetComponent<PlayerController>())
        {
            damagable.OnEntityDamage(_aiSystem.enemySettings.GetEnemyStatType(_aiSystem.enemyType).enemyData.baseDamage, _aiSystem.gameObject, _aiSystem.bIsUnblockable);
        }
    }
}
