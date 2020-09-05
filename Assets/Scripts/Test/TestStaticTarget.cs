using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TestStaticTarget : MonoBehaviour, IDamageable
{
    public bool isDamageEnabled = true;

    [HideInInspector] public StatHandler testEnemyStats;
    [HideInInspector] public Guarding enemyGuard;


    void Start()
    {
        EntityStatData testData = GameManager.instance.gameSettings.enemySettings.enemyData;

        testEnemyStats = new StatHandler();
        testEnemyStats.Init(testData);

        enemyGuard = this.gameObject.AddComponent<Guarding>();
        enemyGuard.Init(testEnemyStats);
    }

    public void DisableDamage()
    {
        //Not needed
    }

    public void EnableDamage()
    {
        //Not needed
    }

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        if (!isDamageEnabled) return;

        if (enemyGuard.CheckIfEntityGuarding(damage)) return;

        //Applies damage
        CalculateDamage(damage);
    }

    void CalculateDamage(float damage)
    {
        testEnemyStats.CurrentHealth -= damage;
        Debug.Log(">> " + gameObject.name + ", Damage applied: " + damage);
    }

    public bool CheckCanDamage()
    {
        return true;
    }

    public EntityType GetEntityType()
    {
        return EntityType.Enemy;
    }
}
