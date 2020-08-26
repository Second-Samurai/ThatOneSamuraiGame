using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFunctions : MonoBehaviour
{
    [Header("Block Variables")]
    public bool bIsBlocking = false;
    public float blockTimer = 0f;
    public float blockCooldown;

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


    private void Start()
    {
        _IKPuppet = GetComponent<IKPuppet>();

        rb = GetComponent<Rigidbody>();

        _pDamageController = GetComponent<PDamageController>();

        _animator = GetComponent<Animator>();
    }
    public void SetBlockCooldown()
    {
        blockTimer = blockCooldown;
    }

    public void StartBlock()
    {
        if (!bIsBlocking && blockTimer == 0f)
        {
            bIsBlocking = true;
            _bDontCheckParry = false;
            _IKPuppet.EnableIK();
        }
    }

    public void EndBlock()
    {
        if (bIsBlocking)
        {
            bIsBlocking = false;
            bIsParrying = false;
            parryTimer = 0f;
            _IKPuppet.DisableIK();
            SetBlockCooldown();
        }
    }

    private void Update()
    {
        CheckBlockCooldown();
        CheckParry();
        
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
    }

    public void ApplyHit(GameObject attacker)
    {
        if (bIsParrying)
        {
            TriggerParry(attacker); 
        }
        else if (bIsBlocking)
        {
            TriggerBlock(attacker); 
        }
        else
        {
            KillPlayer();
        }

    }

    public void TriggerParry(GameObject attacker)
    {
        //rotate to face attacker
        //Damage attacker's guard meter
        Debug.LogWarning("Parried " + attacker.name);

    }
    public void TriggerBlock(GameObject attacker)
    {
        //rotate to face attacker
        //particles
        //play blockbreak anim
        bIsBlocking = false;
        _animator.SetTrigger("Guardbreak");
        Debug.LogWarning("Guard broken!");
    }

    public void KillPlayer()
    {
        //play anim
        //trigger rewind
        Debug.LogError("Player killed!");
    }
}
