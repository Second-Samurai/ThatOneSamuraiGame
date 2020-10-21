using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Enemies;

public class Guarding : MonoBehaviour
{
    public bool canParry = false;
    public bool canGuard = true;
    public bool isStunned = false;

    public TriggerImpulse camImpulse;
    [HideInInspector] public StatHandler statHandler;
    [HideInInspector] public UnityEvent OnGuardEvent = new UnityEvent();
    public AISystem _aiSystem;

    private GameObject _guardMeterCanvas;
    private UIGuardMeter _guardMeter;

    private float _guardCooldownTime;

    public bool bSuperArmour = false;

    public void Init(StatHandler statHandler)
    {
        this.statHandler = statHandler;

        GameManager gameManager = GameManager.instance;

        _guardMeterCanvas = Instantiate(gameManager.gameSettings.guardCanvasPrefab, transform);
        _guardMeter = Instantiate(gameManager.gameSettings.guardMeterPrefab, _guardMeterCanvas.transform).GetComponent<UIGuardMeter>();

        _guardMeter.Init(transform, statHandler, gameManager.mainCamera, _guardMeterCanvas.GetComponent<RectTransform>());
        OnGuardEvent.AddListener(_guardMeter.UpdateGuideMeter);

        _aiSystem = GetComponent<AISystem>();
        _guardCooldownTime = _aiSystem.enemySettings.GetEnemyStatType(_aiSystem.enemyType).guardCooldown;
    }
    
    // Called in animation events to open the enemy's guard
    public void DropGuard()
    {
       if(_aiSystem.enemyType != EnemyType.BOSS) canGuard = false;
    }
    
    // Called in animation events to return the enemy's guard option
    public void RaiseGuard()
    {
        canGuard = true;
    }

    public void EnableGuardMeter()
    {
        _guardMeterCanvas.SetActive(true);
    }
    
    public void DisableGuardMeter()
    {
        _guardMeterCanvas.SetActive(false);
    }

    //Summary: Runs guard and checks if it can guard
    //
    public bool CheckIfEntityGuarding(float damage)
    {
        // This is only used for the swordsman parrying. For when they're parring AND they get parried by the player.
        // This exists because canGuard is set to false while the enemy is parring so they can be killed while
        // doing the parry animation.
        // if (_aiSystem.animator.GetBool("IsParried"))
        // {
        //     StopAllCoroutines();
        //     CalculateGuard(damage);
        //     return true;
        // }
        
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
        
        // if(_aiSystem.animator.GetBool("IsQuickBlocking"))
        //     _aiSystem.EndState();
        
        StartCoroutine(AwaitNextDamage(_guardCooldownTime));
        OnGuardEvent.Invoke();
    }

    //Summary: When breaks guard begin cooldown and disable guard.
    //
    private void BreakGuard()
    {
        Debug.Log("Guard has been BROKEN");
        //GameManager.instance.gameObject.GetComponent<HitstopController>().Hitstop(.1f);
        //GameManager.instance.mainCamera.gameObject.GetComponent<CameraShakeController>().ShakeCamera(.7f);
        //GameManager.instance.playerController.gameObject.GetComponentInChildren<LockOnTargetManager>().GuardBreakCam(this.transform);
        isStunned = true;
        canGuard = false;
        StartCoroutine(AwaitNextDamage(6));
        //camImpulse.FireImpulse();
        //Switch States
        _guardMeter.ShowFinisherKey();
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

        StartCoroutine(GuardCoolDown(8));
    }

    // Count down the remaining guard cooldown time through the GuardCoolDown co-routine
    // Stops the co-routine if the enemy is dead
    private IEnumerator GuardCoolDown(float time)
    {
        if (_aiSystem.bIsDead)
        {
            StopCoroutine(GuardCoolDown(8));
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

            ResetGuard();
        }
    }

    public void ResetGuard()
    {
        StopCoroutine(GuardCoolDown(8));
        statHandler.CurrentGuard = statHandler.maxGuard;
        OnGuardEvent.Invoke();

        if (isStunned)
        {
            _guardMeter.HideFinisherKey();
            _aiSystem.OnEnemyRecovery();
        }
        isStunned = false;
        canGuard = true;
    }
}
