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
    public UIGuardMeter uiGuardMeter;
    
    //Await next damage cooldown variables
    public bool bRunCooldownTimer = false; //Tracked by Rewind
    private float _guardCooldownTime;
    public float remainingCooldownTime; //Tracked by Rewind
    private float _stunTime = 8.0f;
    
    //Recovery variables
    public bool bRunRecoveryTimer = false; //Tracked by Rewind
    
    //NOTE: Current Guard should also be tracked by the rewind system

    public bool bSuperArmour = false;
    
    private Transform _player;

    public void Init(StatHandler statHandler)
    {
        this.statHandler = statHandler;

        GameManager gameManager = GameManager.instance;

        _guardMeterCanvas = Instantiate(gameManager.gameSettings.guardCanvasPrefab, transform);
        uiGuardMeter = Instantiate(gameManager.gameSettings.guardMeterPrefab, _guardMeterCanvas.transform).GetComponent<UIGuardMeter>();

        uiGuardMeter.Init(transform, statHandler, gameManager.MainCamera, _guardMeterCanvas.GetComponent<RectTransform>());
        OnGuardEvent.AddListener(uiGuardMeter.UpdateGuideMeter);

        _aiSystem = GetComponent<AISystem>();
        _guardCooldownTime = _aiSystem.enemySettings.GetEnemyStatType(_aiSystem.enemyType).guardCooldown;

        _player = gameManager.PlayerController.gameObject.transform;
    }


    private void Update()
    {
        RunGuardCooldown();
        RunRecoveryCooldown();

        if (isStunned) 
        {
            if (Vector3.Magnitude(gameObject.transform.position - _player.transform.position) < 3.2)
            {
                if (!uiGuardMeter.finisherKey.enabled)
                {
                    uiGuardMeter.ShowFinisherKey();
                }
            }
            else
            {
                uiGuardMeter.HideFinisherKey();
            }
        }
        else if(uiGuardMeter.finisherKey.enabled)
        {
            uiGuardMeter.HideFinisherKey();
        }
    }

    // After taking damage, run the guard cooldown timer
    // Once it reaches 0, RunRecoveryCooldown is called
    // Resets if hit again through AwaitNextDamage(new cooldown time)
    private void RunGuardCooldown()
    {
        if (bRunCooldownTimer)
        {
            //Debug.Log("Running Guard Cooldown Timer");
            remainingCooldownTime -= Time.deltaTime;
            if (remainingCooldownTime <= 0)
            {
                bRunCooldownTimer = false;
                bRunRecoveryTimer = true;
            }
        }
    }
    
    // After cooldown timer is up, RunRecoveryCooldown
    // Guard will continue to increase until it's back to mac guard
    // Stops completely if hit again through AwaitNextDamage(new cooldown time)
    private void RunRecoveryCooldown()
    {
        if(_aiSystem.bIsDead && bRunRecoveryTimer)
        {
            bRunRecoveryTimer = false;
        }
        else if (bRunRecoveryTimer)
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
        //Debug.Log("Guard has been BROKEN");
        //GameManager.instance.gameObject.GetComponent<HitstopController>().Hitstop(.1f);
        //GameManager.instance.mainCamera.gameObject.GetComponent<CameraShakeController>().ShakeCamera(.7f);
        //GameManager.instance.playerController.gameObject.GetComponentInChildren<LockOnTargetManager>().GuardBreakCam(this.transform);
        isStunned = true;
        canGuard = false;
        AwaitNextDamage(_stunTime);
        //camImpulse.FireImpulse();
        //Switch States
        _aiSystem.OnEnemyStun();
    }

    private void AwaitNextDamage(float time)
    {
        remainingCooldownTime = time;
        bRunRecoveryTimer = false;
        bRunCooldownTimer = true;
    }

    public void ResetGuard()
    {
        bRunRecoveryTimer = false;
        statHandler.CurrentGuard = statHandler.maxGuard;
        OnGuardEvent.Invoke();

        if (isStunned)
        {
            _aiSystem.OnEnemyRecovery();
        }
        isStunned = false;
        canGuard = true;
    }
}
