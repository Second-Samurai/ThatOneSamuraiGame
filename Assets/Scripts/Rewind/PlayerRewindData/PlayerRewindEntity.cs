using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewindEntity : AnimationRewindEntity
{

    public List<PlayerTimeData> playerDataList;

    private PlayerInputScript playerInput;

    public Collider swordCollider;

    public Rigidbody gameObjectRigidbody;

    // Start is called before the first frame update
    protected new void Start()
    {
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        playerDataList = new List<PlayerTimeData>();

        playerInput = gameObject.GetComponent<PlayerInputScript>();
        _rewindInput.Reset += ResetTimeline;


        _rewindInput.OnEndRewind += EnableEvents;
        _rewindInput.OnStartRewind += DisableEvents;
        _rewindInput.OnEndRewind += ApplyData;

        gameObjectRigidbody = gameObject.GetComponent<Rigidbody>();
        base.Start();

    }

    public override void FixedUpdate()
    {
        if (_rewindInput.isTravelling == false)
        {
            RecordPast();

        }
        DisableCollider();

    }
    //setting rigidbodys to kinimatic
    public new void DisableEvents()
    {
        gameObjectRigidbody.isKinematic = true;
        base.DisableEvents();
    }

    public new void EnableEvents()
    {
        gameObjectRigidbody.isKinematic = false;

        base.EnableEvents();
    }

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


    public new void RecordPast()
    {
        //maybe make 10f into a global variable
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (playerDataList.Count > _rewindInput.rewindTime)
        {
            playerDataList.RemoveAt(playerDataList.Count - 1);
        }

        //move to arguments need to be added rewind entity
        playerDataList.Insert(0, new PlayerTimeData(playerInput.camControl.bLockedOn, swordCollider.enabled));

        base.RecordPast();
    }

    public override void StepBack()
    {

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
    }

    public override void StepForward()
    {
        if (playerDataList.Count > 0)
        {
            if (currentIndex > 0)
            {
                SetPosition();
                currentIndex--;
            }
        }
    }

    // can be called in rewind input if needed incase jaiden yells at me for using update
    public void DisableCollider()
    {
        if (_rewindInput.isTravelling == true)
        {
            swordCollider.enabled = false;
        }
 
    }

    public new void SetPosition()
    {
        if (currentIndex <= playerDataList.Count - 1)
        {

        if (playerInput.camControl.bLockedOn != animationDataList[currentIndex].lockedOn)
        {
            playerInput.camControl.bLockedOn = animationDataList[currentIndex].lockedOn;
            playerInput.camControl.ToggleLockOn();
            Debug.Log("switching");
        }
        else
        {
            Debug.Log("you good");
        }
       
        // needs to set the enemy targeting
        base.SetPosition();
    }

    public override void ApplyData()
    {
        if (playerInput._functions.bIsDead) playerInput._functions._inputComponent.SwitchCurrentActionMap("Dead");
        else playerInput._functions._inputComponent.SwitchCurrentActionMap("Gameplay");
        swordCollider.enabled = playerDataList[currentIndex].swordCollider;
    }
}
