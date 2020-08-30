using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TestStaticTarget : MonoBehaviour, IDamageable
{
    public bool isDamageEnabled = true;
    public bool isGuarding = true;

    [HideInInspector] public StatHandler testEnemyStats;
    [HideInInspector] public UnityEvent OnGuardEvent = new UnityEvent();


    void Start()
    {
        EntityStatData testData = GameManager.instance.gameSettings.enemySettings.enemyData;

        testEnemyStats = new StatHandler();
        testEnemyStats.Init(testData);

        GameManager gameManager = GameManager.instance;
        UIGuardMeter guardMeter = gameManager.CreateEntityGuardMeter(this.transform, testEnemyStats);
        OnGuardEvent.AddListener(guardMeter.UpdateMeter);

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
        if (testEnemyStats.CurrentGuard > 0 && isGuarding)
        {
            CalculateGuard(damage);
            return;
        }

        //Applies damage
        testEnemyStats.CurrentHealth -= damage;

        Debug.Log(">> " + gameObject.name + ", Damage applied: " + damage);
    }

    void CalculateGuard(float damage)
    {
        testEnemyStats.CurrentGuard -= damage;
        OnGuardEvent.Invoke();
    }
}
