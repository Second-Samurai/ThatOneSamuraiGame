using System;
using System.Collections;
using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Camera;
using ThatOneSamuraiGame.Scripts.Input;
using UnityEngine;

public class PlayerRewindEntity : AnimationRewindEntity
{
    
    public List<PlayerTimeData> playerDataList;
    public Collider swordCollider;
    public Rigidbody gameObjectRigidbody;

    private ICameraController m_CameraController;
    private PlayerFunctions m_PlayerFunctions;

    // Start is called before the first frame update
    protected new void Start()
    {
        /*
        _rewindInput = GameManager.instance.RewindManager.GetComponent<RewindManager>();
        playerDataList = new List<PlayerTimeData>();

        this.m_CameraController = this.GetComponent<ICameraController>();
        this.m_PlayerFunctions = this.GetComponent<PlayerFunctions>();
        _rewindInput.Reset += ResetTimeline;

        _rewindInput.OnEndRewind += EnableEvents;
        _rewindInput.OnStartRewind += DisableEvents;
        _rewindInput.OnEndRewind += ApplyData;

        gameObjectRigidbody = gameObject.GetComponent<Rigidbody>();
        base.Start();
        
    }

    public override void FixedUpdate()
    {
        /*
        if (_rewindInput.isTravelling == false)
        {
            RecordPast();

        }
        DisableCollider();
        */
    }

    //setting rigidbodys to kinimatic
    [Obsolete("Deprecated", false)]
    public new void DisableEvents()
    {
        gameObjectRigidbody.isKinematic = true;
        base.DisableEvents();
    }

    [Obsolete("Deprecated", false)]
    public new void EnableEvents()
    {
        gameObjectRigidbody.isKinematic = false;

        base.EnableEvents();
        //StartCoroutine(_rewindInput.BecomeInvincible());
    }

    [Obsolete("Deprecated", false)]
    public new void ResetTimeline()
    {

        for (int i = currentIndex; i > 0; i--)
        {
            if (currentIndex <= playerDataList.Count - 1)
            {
                playerDataList.RemoveAt(i);
            }
        }

        playerDataList.TrimExcess();
    }

    [Obsolete("Deprecated", false)]
    public new void RecordPast()
    {
        //maybe make 10f into a global variable
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (playerDataList.Count > _rewindInput.rewindTime)
        {
            playerDataList.RemoveAt(playerDataList.Count - 1);
        }

        //move to arguments need to be added rewind entity
        playerDataList.Insert(0, new PlayerTimeData(this.m_CameraController.IsLockedOn, swordCollider.enabled));

        base.RecordPast();
    }

    [Obsolete("Deprecated", false)]
    public override void StepBack()
    {
        /*
        if (playerDataList.Count > 0)
        {
            if (currentIndex < playerDataList.Count - 1)
            {
                //Debug.Log("COUNT " + playerDataList.Count);
                currentIndex++;
                if (currentIndex >= playerDataList.Count - 1)
                {
                   //Debug.Log("CashMoney");
                    currentIndex = playerDataList.Count - 1;
                }
                //Debug.Log(currentIndex);
                SetPosition();
            }
        }
        */
    }

    [Obsolete("Deprecated", false)]
    public override void StepForward()
    {
        /*
        if (playerDataList.Count > 0)
        {
            if (currentIndex > 0)
            {
                SetPosition();
                currentIndex--;
            }
        }
        */
    }

    // can be called in rewind input if needed incase jaiden yells at me for using update
    [Obsolete("Deprecated", false)]
    public void DisableCollider()
    {
        /*
        if (_rewindInput.isTravelling == true)
        {
            swordCollider.enabled = false;
        }
         */
    }

    [Obsolete("Deprecated", false)]
    public new void SetPosition()
    {
        if (currentIndex <= playerDataList.Count - 1)
        {
            // TODO: FIX THIS TO WORK
            // if (playerInput.camControl.bLockedOn != animationDataList[currentIndex].lockedOn)
            // {
            //     playerInput.camControl.bLockedOn = animationDataList[currentIndex].lockedOn;
            //     if (playerInput.camControl.bLockedOn)
            //         playerInput.camControl.LockOn();
            //     else
            //     {
            //         playerInput.camControl.UnlockCam();
            //     }
            // }
        }
       
        // needs to set the enemy targeting
        base.SetPosition();
    }

    [Obsolete("Deprecated", false)]
    public override void ApplyData()
    {
        /*
        IInputManager _InputManager = GameManager.instance.InputManager;
        
        if (this.m_PlayerFunctions.bIsDead) 
            _InputManager.SwitchToMenuControls();
        else 
            _InputManager.SwitchToGameplayControls();
        
        swordCollider.enabled = playerDataList[currentIndex].swordCollider;
        */
     //   playerInput.camControl.bLockedOn = false;
      //  playerInput.camControl.UnlockCam();
    }
    
}
