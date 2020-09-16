using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Enemies;

public class Guarding : MonoBehaviour
{
    public bool canGuard = true;
    public bool isStunned = false;

    private float _guardCooldownTime = 8;
    
    [HideInInspector] public StatHandler statHandler;
    [HideInInspector] public UnityEvent OnGuardEvent = new UnityEvent();
    private AISystem _aiSystem;
    

    public void Init(StatHandler statHandler)
    {
        this.statHandler = statHandler;

        GameManager gameManager = GameManager.instance;
        UIGuardMeter guardMeter = gameManager.CreateEntityGuardMeter(this.transform, statHandler);
        OnGuardEvent.AddListener(guardMeter.UpdateGuideMeter);
        _aiSystem = GetComponent<AISystem>();
    }
    
    // Called in animation events to open the enemy's guard
    public void DropGuard()
    {
        canGuard = false;
    }
    
    // Called in animation events to return the enemy's guard option
    public void RaiseGuard()
    {
        canGuard = true;
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
        GameManager.instance.gameObject.GetComponent<HitstopController>().Hitstop(.1f);
        GameManager.instance.mainCamera.gameObject.GetComponent<CameraShakeController>().ShakeCamera(.7f);
        GameManager.instance.playerController.gameObject.GetComponentInChildren<LockOnTargetManager>().GuardBreakCam(this.transform);
        isStunned = true;
        canGuard = false;
        StartCoroutine(AwaitNextDamage(6));

        //Switch States
        _aiSystem.OnEnemyStun();
    }

    private IEnumerator AwaitNextDamage(float time)
    {
        float timer = time;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        StartCoroutine(GuardCoolDown(_guardCooldownTime));
    }

    // Count down the remaining guard cooldown time through the GuardCoolDown co-routine
    // Stops the co-routine if the enemy is dead
    private IEnumerator GuardCoolDown(float time)
    {
        if (_aiSystem.bIsDead)
        {
            StopCoroutine(GuardCoolDown(_guardCooldownTime));
        }
        else
        {
            float coolVal = (statHandler.maxGuard - statHandler.CurrentGuard / time) * Time.deltaTime;

            while (statHandler.CurrentGuard < statHandler.maxGuard)
            {
                statHandler.CurrentGuard += coolVal;
                OnGuardEvent.Invoke();
                yield return null;
            }

            statHandler.CurrentGuard = statHandler.maxGuard;
            OnGuardEvent.Invoke();

            
            isStunned = false;
            canGuard = true;
            _aiSystem.OnEnemyRecovery();
        }
    }
}
