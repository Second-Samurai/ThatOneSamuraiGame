using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewindEntity : AnimationRewindEntity
{

    public List<PlayerTimeData> playerDataList;

    private PlayerInputScript playerInput;


    // Start is called before the first frame update
    protected new void Start()
    {
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        playerDataList = new List<PlayerTimeData>();

        playerInput = gameObject.GetComponent<PlayerInputScript>();
        _rewindInput.Reset += ResetTimeline;
        base.Start();

    }

    public override void FixedUpdate()
    {
        if (_rewindInput.isTravelling == false)
        {
            RecordPast();

        }

    }

    public new void ResetTimeline()
    {

        for (int i = currentIndex; i > 0; i--)
        {
            playerDataList.RemoveAt(i);
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
        playerDataList.Insert(0, new PlayerTimeData(playerInput.bLockedOn));

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (playerDataList.Count > 0)
        {
            if (currentIndex < playerDataList.Count - 1)
            {
                Debug.Log("COUNT " + playerDataList.Count);
                currentIndex++;
                if (currentIndex >= playerDataList.Count - 1)
                {
                    Debug.Log("CashMoney");
                    currentIndex = playerDataList.Count - 1;
                }
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

    public new void SetPosition()
    {

        if (playerInput.bLockedOn != animationDataList[currentIndex].lockedOn)
        {
            playerInput.bLockedOn = animationDataList[currentIndex].lockedOn;
            if (playerInput.bLockedOn)
                playerInput._camControl.LockOn();
            else
            {
                playerInput._camControl.UnlockCam();
            }
        }
       
        // needs to set the enemy targeting
        base.SetPosition();
    }

    public override void ApplyData()
    {
        if (playerInput._functions.bIsDead) playerInput._functions._inputComponent.SwitchCurrentActionMap("Dead");
        else playerInput._functions._inputComponent.SwitchCurrentActionMap("Gameplay");
    }
}
