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
    
    //Await next damage cooldown variables
    private bool _bRunCooldownTimer = false;
    private float _guardCooldownTime;
    private float _remainingCooldownTime;
    private float _stunTime = 8.0f;
    
    //Recovery variables
    private bool _bRunRecoveryTimer = false;

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

    private void Update()
    {
        RunGuardCooldown();
        RunRecoveryCooldown();
    }

    // After taking damage, run the guard cooldown timer
    // Once it reaches 0, RunRecoveryCooldown is called
    // Resets if hit again through AwaitNextDamage(new cooldown time)
    private void RunGuardCooldown()
    {
        if (_bRunCooldownTimer)
        {
            //Debug.Log("Running Guard Cooldown Timer");
            _remainingCooldownTime -= Time.deltaTime;
            if (_remainingCooldownTime <= 0)
            {
                _bRunCooldownTimer = false;
                _bRunRecoveryTimer = true;
            }
        }
    }
    
    // After cooldown timer is up, RunRecoveryCooldown
    // Guard will continue to increase until it's back to mac guard
    // Stops completely if hit again through AwaitNextDamage(new cooldown time)
    private void RunRecoveryCooldown()
    {
        if(_aiSystem.bIsDead && _bRunRecoveryTimer)
        {
            _bRunRecoveryTimer = false;
        }
        else if (_bRunRecoveryTimer)
        {
            float coolVal = (statHandler.maxGuard - statHandler.CurrentGuard / _guardCooldownTime) * Time.deltaTime;

            if (statHandler.CurrentGuard < statHandler.maxGuard)
            {
                //Debug.Log("Running Recovery");
                statHandler.CurrentGuard += coolVal;
                OnGuardEvent.Invoke();
            }
            else
            {
                ResetGuard();
            }
        }
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
        
        AwaitNextDamage(_guardCooldownTime);
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
        AwaitNextDamage(_stunTime);
        //camImpulse.FireImpulse();
        //Switch States
        _guardMeter.ShowFinisherKey();
        _aiSystem.OnEnemyStun();
    }

    private void AwaitNextDamage(float time)
    {
        _remainingCooldownTime = time;
        _bRunRecoveryTimer = false;
        _bRunCooldownTimer = true;
    }

    public void ResetGuard()
    {
        _bRunRecoveryTimer = false;
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
