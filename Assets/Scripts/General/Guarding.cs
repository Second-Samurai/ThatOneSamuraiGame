using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Guarding : MonoBehaviour
{
    public bool canGuard = true;
    public bool isStunned = false;

    public IEnemyStates enemyStates; //Might need to rename the interface better
    [HideInInspector] public StatHandler statHandler;
    [HideInInspector] public UnityEvent OnGuardEvent = new UnityEvent();

    public void Init(StatHandler statHandler)
    {
        this.statHandler = statHandler;

        GameManager gameManager = GameManager.instance;
        UIGuardMeter guardMeter = gameManager.CreateEntityGuardMeter(this.transform, statHandler);
        OnGuardEvent.AddListener(guardMeter.UpdateGuideMeter);

        enemyStates = this.GetComponent<IEnemyStates>();
    }

    //Summary: Runs guard and checks if it can guard
    //
    public bool CheckIfEntityGuarding(float damage)
    {
        if (canGuard && !isStunned)
        {
            StopAllCoroutines();
            CalculateGuard(damage);
            return true;
        }

        return false;
    }

    //Summary: This calculates the damage and checks for guard break
    //
    private void CalculateGuard(float damage)
    {
        statHandler.CurrentGuard -= damage;

        if (statHandler.CurrentGuard <= 0)
        {
            BreakGuard();
            OnGuardEvent.Invoke();
            return;
        }

        StartCoroutine(AwaitNextDamage(3));
        OnGuardEvent.Invoke();
    }

    //Summary: When breaks guard begin cooldown and disable guard.
    //
    private void BreakGuard()
    {
        Debug.Log("Guard has been BROKEN");
        isStunned = true;
        canGuard = false;
        StartCoroutine(AwaitNextDamage(6));

        //Create state
        Debug.Log(">> Guarding: Now in enemy stun state");
        //enemyStates.OnEnemyStun();
    }

    private IEnumerator AwaitNextDamage(float time)
    {
        float timer = time;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        StartCoroutine(GuardCoolDown(8));
    }

    private IEnumerator GuardCoolDown(float time)
    {
        float coolVal = (statHandler.maxGuard - statHandler.CurrentGuard / time) * Time.deltaTime;

        while (statHandler.CurrentGuard < statHandler.maxGuard)
        {
            statHandler.CurrentGuard += coolVal;
            OnGuardEvent.Invoke();
            yield return null;
        }

        Debug.Log("Guard has been RESTORED");
        statHandler.CurrentGuard = statHandler.maxGuard;
        OnGuardEvent.Invoke();

        if(canGuard == false)
        {
            //Switch back to active attack state
            //Debug.Log(">> Guarding: Now in approach player state");
            //enemyStates.OnApproachPlayer();
        }

        isStunned = false;
        canGuard = true;
    }
}
