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
            //if (other.gameObject.GetComponent<PlayerFunctions>().bIsParrying)
            //{
            //    Debug.Log("Attack Parried");
            //}
            //else
            //{
            damagable.OnEntityDamage(_aiSystem.enemySettings.enemyData.baseDamage, _aiSystem.gameObject, false);
            //
        }
    }
}
