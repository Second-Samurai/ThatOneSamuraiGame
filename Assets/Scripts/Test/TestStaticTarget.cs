using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestStaticTarget : MonoBehaviour, IDamageable
{
    public bool isDamageEnabled = true;

    [HideInInspector] public StatHandler testEnemyStats;

    void Start()
    {
        EntityStatData testData = GameManager.instance.gameSettings.enemySettings.testEnemeyData;

        testEnemyStats = new StatHandler();
        testEnemyStats.Init(testData);
    }

    public void DisableDamage()
    {
        //Not needed
    }

    public void EnableDamage()
    {
        //Not needed
    }

    public void OnEntityDamage(float damage, GameObject attacker)
    {
        if (!isDamageEnabled) return;

        Debug.Log(">> " + gameObject.name + ", Damage applied: " + damage);
    }
}
