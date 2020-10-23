using System;
using Enemies;
using UnityEngine;

public class EDamageController : MonoBehaviour, IDamageable
{
    private StatHandler _enemyStats;
    private AISystem _aiSystem;
    HitstopController hitstopController;

    public Guarding enemyGuard;

    private bool _bIsDamageDisabled = false;

    private bool _bRunInvincibilityTimer = false;
    private float _remainingInvincibilityTime;
    private float _invincibilityTime = 0.4f;

    public void Init(StatHandler enemyStats) {
        _enemyStats = enemyStats;
        if (!enemyGuard) Debug.LogError("NO GUARD");
        enemyGuard = GetComponent<Guarding>();
        enemyGuard.Init(_enemyStats);
        hitstopController = GameManager.instance.gameObject.GetComponent<HitstopController>();
    }

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        if (_bIsDamageDisabled)
        {
            Debug.LogWarning("Enemy got damaged but has their damage disabled");
            return;
        }

        if (attacker.layer == LayerMask.NameToLayer("Player"))
        {
            EnableInvincibleFrames();
        }

        Vector3 dir = Vector3.back;
        if (!unblockable)
        {
            if (attacker.layer == LayerMask.NameToLayer("Player"))
            {
                //_aiSystem.ImpulseWithDirection(damage, transform.position - attacker.transform.position, .3f);
                _aiSystem.ImpulseWithDirection(damage*2, dir, .15f);

                // Summary: Perform an enemy parry if the enemy canParry
                // Currently canParry is only set true and false in BlockEnemyState script
                // and set to false in the ParryEnemyState script
                if (enemyGuard.canParry)
                {
                    _aiSystem.OnParry();
                    attacker.GetComponent<PCombatController>().IsParried();
                    return;
                }
                
                // If enemy can guard and isn't stunned, reduce guard by damage amount and do the following
                if (enemyGuard.CheckIfEntityGuarding(damage))
                {
                    // If enemy still has left over guard meter AFTER CheckIfEntityGuarding, go to the quick block state
                    // The following 3 lines do not occur if the enemy is guard broken through the previous CheckIfEntityGuarding
                    if (enemyGuard.canGuard)
                    {
                        _aiSystem.parryEffects.PlayBlock();
                        if(!enemyGuard.bSuperArmour) _aiSystem.OnQuickBlock();
                    }
                    return;
                }

                if(enemyGuard.isStunned) attacker.GetComponentInChildren<LockOnTargetManager>().EndGuardBreakCam();

                _aiSystem.ApplyHit(attacker);
            }
            else
            {
                Debug.Log(attacker.layer.ToString());
                _aiSystem.ApplyHit(attacker);
            }
        }
        else if (enemyGuard.isStunned && unblockable)
        {
            //Debug.Log("FINISHER");
            attacker.GetComponentInChildren<LockOnTargetManager>().EndGuardBreakCam();
            attacker.GetComponentInChildren<FinishingMoveController>().PlayFinishingMove(gameObject);
            return;
        }
        else if (unblockable)
        {
            //Debug.LogWarning("WARNING: Attack not on player layer");
            _aiSystem.ApplyHit(attacker);
            _aiSystem.ImpulseWithDirection(damage*5, dir, .3f);
        }
    }

    /* Summary: This disables the damage from this component.
     *          But can be only used when in a state that does
     *          not require it.*/
    //

    private void Update()
    {
        if (_bRunInvincibilityTimer)
        {
            _remainingInvincibilityTime -= Time.deltaTime;
            if (_remainingInvincibilityTime <= 0)
            {
                DisableInvincibleFrames();
            }
        }
    }

    private void EnableInvincibleFrames()
    {
        //Debug.Log("1");
        DisableDamage();
        _remainingInvincibilityTime = _invincibilityTime;
        _bRunInvincibilityTimer = true;
    }
    
    private void DisableInvincibleFrames()
    {
        //Debug.Log("2");
        EnableDamage();
        _remainingInvincibilityTime = 0;
        _bRunInvincibilityTimer = false;
    }

    public void OnParried(float damage)
    {
        enemyGuard.RaiseGuard();
        // Stun if enemy can guard
        if (enemyGuard.CheckIfEntityGuarding(damage))
        {

            // DO NOT TRIGGER PARRY STUN IF THE ENEMY IS ALREADY STUNNED
            if (!enemyGuard.isStunned && _aiSystem.bCanBeStunned)
            {
                _aiSystem.OnParryStun();
            }
        }
    }

    public void DisableDamage()
    {
        _bIsDamageDisabled = true;
    }

    public void EnableDamage()
    {
        _bIsDamageDisabled = false;
    }

    private void Start()
    {
        _aiSystem = GetComponent<AISystem>();
    }

    public bool CheckCanDamage()
    {
        return _bIsDamageDisabled;
    }

    public EntityType GetEntityType()
    {
        return EntityType.Enemy;
    }

    public void TriggerSlowdown(float f)
    {
        hitstopController.SlowTime(.5f, f);
    }
}
