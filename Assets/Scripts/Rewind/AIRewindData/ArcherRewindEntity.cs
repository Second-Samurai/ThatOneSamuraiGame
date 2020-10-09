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
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
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
            archerDataList.RemoveAt(i);
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
        archerDataList.Insert(0, new ArcherTimeData(basicArcher.lastDirection, basicArcher.shotDirection, basicArcher.currentState, basicArcher.shotTimer));

        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + "   :   " + m_CurrentClipInfo[0].clip.name);

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (archerDataList.Count > 0)
        {
            if (currentIndex < archerDataList.Count - 1)
            {
                SetPosition();
                currentIndex++;
            }
            //Debug.LogWarning("animStepBack");
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
            // Debug.LogWarning("animStepForward");
        }
    }

    public new void SetPosition()
    {
        basicArcher.lastDirection = archerDataList[currentIndex].lastDirection;
        basicArcher.shotDirection = archerDataList[currentIndex].shotDirection;
        basicArcher.shotTimer = archerDataList[currentIndex].shotTimer;

        base.SetPosition();

    }

    public override void ApplyData()
    {
        basicArcher.currentState = archerDataList[currentIndex].currentState;

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
