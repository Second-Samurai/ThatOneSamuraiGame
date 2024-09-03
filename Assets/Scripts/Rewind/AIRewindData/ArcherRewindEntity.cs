using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherRewindEntity : ArcherAnimationRewindEntity
{
    public List<ArcherTimeData> archerDataList;
    public Rigidbody gameObjectRigidbody;

    private BasicArcher basicArcher;


    protected new void Start()
    {
        _rewindInput = GameManager.instance.RewindManager.GetComponent<RewindManager>();
        archerDataList = new List<ArcherTimeData>();
        basicArcher = gameObject.GetComponent<BasicArcher>();

        _rewindInput.Reset += ResetTimeline;
        _rewindInput.OnStartRewind += DisableEvents;
        _rewindInput.OnEndRewind += EnableEvents;

        _rewindInput.OnEndRewind += ApplyData;

        gameObjectRigidbody = gameObject.GetComponent<Rigidbody>();


        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (_rewindInput.isTravelling == false)
        {
            RecordPast();

            // animator.enabled = true;
        }
        else
        {


        }
  

    }

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
        for (int i = currentIndex; i >= 0; i--)
        {
            //archerAnimationDataList.Count > 0 added due to an argument out of range exception
            if (currentIndex <= archerAnimationDataList.Count - 1 && archerAnimationDataList.Count > 0)
            {
                archerDataList.RemoveAt(i);
            }
        }
        archerDataList.TrimExcess();
    }

    public new void RecordPast()
    {
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (archerDataList.Count > _rewindInput.rewindTime)
        {
            archerDataList.RemoveAt(archerDataList.Count - 1);
        }

        //move to animation rewind entity
        archerDataList.Insert(0, new ArcherTimeData(basicArcher.lastDirection, basicArcher.shotDirection, basicArcher.currentState, basicArcher.shotTimer, basicArcher.col.enabled));

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (archerDataList.Count > 0)
        {
            if (currentIndex < archerDataList.Count - 1)
            {
                currentIndex++;
                if (currentIndex >= archerDataList.Count - 1)
                {
                    currentIndex = archerDataList.Count - 1;
                }
                SetPosition();
            }
        }
    }

    public override void StepForward()
    {
        if (archerDataList.Count > 0)
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
        if (currentIndex <= archerAnimationDataList.Count - 1)
        {
            basicArcher.lastDirection = archerDataList[currentIndex].lastDirection;
            basicArcher.shotDirection = archerDataList[currentIndex].shotDirection;
            basicArcher.shotTimer = archerDataList[currentIndex].shotTimer;
        }
        base.SetPosition();

    }

    public override void ApplyData()
    {
        if (currentIndex <= archerAnimationDataList.Count - 1)
        {
            try
            {
                basicArcher.currentState = archerDataList[currentIndex].currentState;
                basicArcher.col.enabled = archerDataList[currentIndex].bColEnabled;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Debug.LogError(exception);
                Debug.LogWarning(archerDataList.Count + " || " + currentIndex);
            }

            
        }
    }
    protected new void OnDestroy()
    {
        _rewindInput.Reset -= ResetTimeline;
        _rewindInput.OnEndRewind -= EnableEvents;
        _rewindInput.OnStartRewind -= DisableEvents;
        _rewindInput.OnEndRewind -= ApplyData;
        base.OnDestroy();
    }
}
