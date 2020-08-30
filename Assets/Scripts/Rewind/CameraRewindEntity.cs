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
        cameraDataList = new List<CameraTimeData>();
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
        if (cameraDataList.Count > Mathf.Round(10f * (1f / Time.fixedDeltaTime)))
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
            SetPosition();
            if (currentIndex < cameraDataList.Count - 1)
            {
                currentIndex++;
            }
        }
    }

    public override void StepForward()
    {
        if (cameraDataList.Count > 0)
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
        // needs to set the enemy targeting
        base.SetPosition();
    }
}
