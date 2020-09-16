using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRewindEntity : RewindEntity
{

    // FUNCTIONALITY NEEDS TO BE ADDED
    public List<CameraTimeData> cameraDataList;

    // Start is called before the first frame update
    protected new void Start()
    {
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        cameraDataList = new List<CameraTimeData>();

        _rewindInput.Reset += ResetTimeline;
        base.Start();
    }

    public override void FixedUpdate()
    {
        if (isTravelling == false)
        {
            RecordPast();

        }

        if (isTravelling) 
        { 
        
        }

    }

    public new void ResetTimeline()
    {
        for (int i = currentIndex; i > 0; i--)
        {
            cameraDataList.RemoveAt(i);
        }
        cameraDataList.TrimExcess();
    }

    public new void RecordPast()
    {
        //maybe make 10f into a global variable
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (cameraDataList.Count > _rewindInput.rewindTime)
        {
            cameraDataList.RemoveAt(cameraDataList.Count - 1);
        }

        //move to arguments need to be added rewind entity
        cameraDataList.Insert(0, new CameraTimeData());

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (cameraDataList.Count > 0)
        {
            if (currentIndex < cameraDataList.Count - 1)
            {
                SetPosition();
                currentIndex++;
            }
        }
    }

    public override void StepForward()
    {
        if (cameraDataList.Count > 0)
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
        // needs to set the enemy targeting
        base.SetPosition();
    }
}
