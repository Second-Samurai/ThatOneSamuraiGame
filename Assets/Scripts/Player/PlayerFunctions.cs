using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerFunctions : MonoBehaviour
{
    [Header("Block Variables")]
    public bool bIsBlocking = false;
    public float blockTimer = 0f;
    public float blockCooldown;
    public bool bCanBlock = true;

    [Header("Parry Variables")]
    public bool bIsParrying = false;
    public float parryTimer = 0f; 
    public float parryTimerTarget;
    bool _bDontCheckParry = false;

    [Header("IK Functions")]
    IKPuppet _IKPuppet;

    Animator _animator;
    PDamageController _pDamageController;
    Rigidbody rb;

    HitstopController hitstopController;

    public bool bIsDead = false;

    public RectTransform screenCenter;

    public ParryEffects parryEffects;

    public GameObject pauseMenu;

    public PlayerInput _inputComponent;

    public PlayerInputScript playerInputScript;

    public GameObject lSword, rSword;
    private void Start()
    {
        _IKPuppet = GetComponent<IKPuppet>();

        rb = GetComponent<Rigidbody>();

        _pDamageController = GetComponent<PDamageController>();

        _animator = GetComponent<Animator>();

        _inputComponent = GetComponent<PlayerInput>();

        playerInputScript = GetComponent<PlayerInputScript>();

        hitstopController = GameManager.instance.GetComponent<HitstopController>();
    }
    public void SetBlockCooldown()
    {
        blockTimer = blockCooldown;
    }

    public void StartBlock()
    {
        if (!bIsBlocking && blockTimer == 0f && bCanBlock)
        {
            bIsBlocking = true;
            _bDontCheckParry = false;
            parryEffects.PlayGleam();
            _IKPuppet.EnableIK();
        }
    }

    public void EndBlock()
    {
       // if (bIsBlocking)
       // {
            bIsBlocking = false;
            bIsParrying = false;
            parryTimer = 0f;
            _IKPuppet.DisableIK();
            SetBlockCooldown();
       // }
    }

    private void Update()
    {
        CheckBlockCooldown();
        CheckParry();
        //remove this

        if(bIsDead && playerInputScript.bCanMove) 
            playerInputScript.DisableMovement();
        else if(!bIsDead && !playerInputScript.bCanMove) 
            playerInputScript.EnableMovement();



    }

    private void CheckParry()
    {
        if (!_bDontCheckParry)
        {
            if (bIsBlocking && parryTimer < parryTimerTarget)
            {
                parryTimer += Time.deltaTime;
                bIsParrying = true;
            }
            if (parryTimer > parryTimerTarget)
                parryTimer = parryTimerTarget;
            if (parryTimer == parryTimerTarget)
            {
                bIsParrying = false;
                _bDontCheckParry = true;
            }
        }
    }
    private void CheckBlockCooldown()
    {
        if (blockTimer != 0f)
        {
            if (blockTimer > 0f)
            {
                blockTimer -= Time.deltaTime;
            }
            if (blockTimer < 0f)
                blockTimer = 0f;
        }
    }

    public IEnumerator DodgeImpulse(Vector3 lastDir, float force)
    {
        float dodgeTimer = .15f;
        while (dodgeTimer > 0f)
        {
            // if(bLockedOn)
            transform.Translate(lastDir.normalized * force * Time.deltaTime);
            //else
            //    transform.position += lastDir.normalized * force * Time.deltaTime;
            dodgeTimer -= Time.deltaTime;
            yield return null;
        }
        EnableBlock();
    }

    public void ApplyHit(GameObject attacker, bool unblockable, float damage)
    {
        if (bIsParrying)
        {
            TriggerParry(attacker, damage);
        }
        else if (!unblockable)
        {
            if (bIsBlocking)
            {
                TriggerBlock(attacker);
            }
            else
            {
                
                KillPlayer();
            }
        }
        else KillPlayer();

    }

    public void TriggerParry(GameObject attacker, float damage)
    {
        parryEffects.PlayParry();
        hitstopController.SlowTime(.5f, 1);
        if(attacker != null)
        {
            attacker.GetComponent<EDamageController>().OnParried(damage); //Damage attacker's guard meter

        }
        //GameManager.instance.mainCamera.gameObject.GetComponent<CameraShakeController>().ShakeCamera(.7f);
        //Debug.LogWarning("Parried " + attacker.name);

    }
    public void TriggerBlock(GameObject attacker)
    {
        //rotate to face attacker
        parryEffects.PlayBlock();
        //GameManager.instance.mainCamera.gameObject.GetComponent<CameraShakeController>().ShakeCamera(1);
        bIsBlocking = false;
        _animator.SetTrigger("GuardBreak");
        //Debug.LogWarning("Guard broken!");
        _IKPuppet.DisableIK();
    }

    public void KillPlayer()
    {
        if (!bIsDead)
        {
            //play anim
            _animator.SetTrigger("Death");
            _animator.SetBool("isDead", true);
            //trigger rewind
            bIsDead = true;
            
            _inputComponent.SwitchCurrentActionMap("Rewind");
            //Debug.LogError("Player killed!");
            //GameManager.instance.mainCamera.gameObject.GetComponent<CameraShakeController>().ShakeCamera(1);
            //GameManager.instance.gameObject.GetComponent<HitstopController>().Hitstop(.3f);

        }
    }

    public void DisableBlock()
    {
        bCanBlock = false;
        //Debug.LogWarning("off");

        _IKPuppet.DisableIK();
    }

    public void EnableBlock()
    {
        bCanBlock = true;
        //Debug.LogWarning("on");
    }
 
    public void SnapToEnemy()
    {
        //Vector3 CenterPos = GetMousePosition(screenCenter.position, Camera.main);
        //Vector3 attackDir = 
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
    }
}
