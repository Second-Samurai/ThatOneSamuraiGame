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


    private void Start()
    {
        _IKPuppet = GetComponent<IKPuppet>();
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
}
