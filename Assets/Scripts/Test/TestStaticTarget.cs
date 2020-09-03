using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TestStaticTarget : MonoBehaviour, IDamageable
{
    public bool isDamageEnabled = true;
    //public bool canGuard = true;
    //public bool isStunned = false; //This is temporary make state when possible

    //A stunned state is probably required 

    [HideInInspector] public StatHandler testEnemyStats;
    [HideInInspector] public Guarding enemyGuard;
    //[HideInInspector] public UnityEvent OnGuardEvent = new UnityEvent();


    void Start()
    {
        EntityStatData testData = GameManager.instance.gameSettings.enemySettings.enemyData;

        testEnemyStats = new StatHandler();
        testEnemyStats.Init(testData);

        enemyGuard = this.gameObject.AddComponent<Guarding>();
        enemyGuard.Init(testEnemyStats);

        //GameManager gameManager = GameManager.instance;
        //UIGuardMeter guardMeter = gameManager.CreateEntityGuardMeter(this.transform, testEnemyStats);
        //OnGuardEvent.AddListener(guardMeter.UpdateGuideMeter);
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
        /*if (canGuard && !isStunned)
        {
            StopAllCoroutines();

            //if(testEnemyStats.CurrentGuard )

            CalculateGuard(damage);
            return;
        }*/
        if (enemyGuard.CheckIfEntityGuarding(damage)) return;

        //Applies damage
        CalculateDamage(damage);
    }

    void CalculateDamage(float damage)
    {
        testEnemyStats.CurrentHealth -= damage;
        Debug.Log(">> " + gameObject.name + ", Damage applied: " + damage);
    }


    /*void CalculateGuard(float damage)
    {
        testEnemyStats.CurrentGuard -= damage;

        if(testEnemyStats.CurrentGuard <= 0)
        {
            BreakGuard();
            return;
        }

        StartCoroutine(AwaitNextDamage());
        OnGuardEvent.Invoke();
    }

    void BreakGuard()
    {
        Debug.Log("Guard has been BROKEN");
        isStunned = true;
        canGuard = false;
        StartCoroutine(GuardCoolDown(8));
    }

    void RestoreGuard(float lerpTime)
    {
        testEnemyStats.CurrentGuard = Mathf.Lerp(testEnemyStats.CurrentGuard, testEnemyStats.maxGuard, lerpTime);
    }

    IEnumerator AwaitNextDamage()
    {
        float timer = 3f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        StartCoroutine(GuardCoolDown(5));
    }

    IEnumerator GuardCoolDown(float time)
    {
        float timer = time;
        float lerpTime = (testEnemyStats.CurrentGuard / time) * Time.deltaTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            RestoreGuard(lerpTime);
            OnGuardEvent.Invoke();
            Debug.Log("is Fixing");
            yield return null;
        }

        Debug.Log("Guard has been RESTORED");
        testEnemyStats.CurrentGuard = testEnemyStats.maxGuard;
        OnGuardEvent.Invoke();
        isStunned = false; //Switch back to active attack state
        canGuard = true;
    }*/
}
