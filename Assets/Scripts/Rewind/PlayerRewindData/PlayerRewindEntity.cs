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
        playerDataList = new List<PlayerTimeData>();

        playerInput = gameObject.GetComponent<PlayerInputScript>();
        base.Start();

    }

    public override void FixedUpdate()
    {
        if (isTravelling == false)
        {
            RecordPast();

        }

    }

    public new void RecordPast()
    {
        //maybe make 10f into a global variable
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (playerDataList.Count > Mathf.Round(10f * (1f / Time.fixedDeltaTime)))
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
            SetPosition();
            if (currentIndex < playerDataList.Count - 1)
            {
                currentIndex++;
            }
        }
    }

    public override void StepForward()
    {
        if (playerDataList.Count > 0)
        {
            SetPosition();
            if (currentIndex > 0)
            {
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
}
