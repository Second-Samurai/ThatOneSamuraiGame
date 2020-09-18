using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRewindEntity : RewindEntity
{

    // FUNCTIONALITY NEEDS TO BE ADDED
    [SerializeField]
    public List<CameraTimeData> cameraDataList;
    private LockOnTargetManager lockOnTargetManager;

    // Start is called before the first frame update
    protected new void Start()
    {
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        cameraDataList = new List<CameraTimeData>();
       lockOnTargetManager = GameManager.instance.playerController.gameObject.GetComponentInChildren<LockOnTargetManager>();
        _rewindInput.Reset += ResetTimeline;
        base.Start();
    }

    public override void FixedUpdate()
    {
        if (_rewindInput.isTravelling == false)
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
        cameraDataList.Insert(0, new CameraTimeData(lockOnTargetManager._bLockedOn, lockOnTargetManager._target));

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
        lockOnTargetManager._target = cameraDataList[currentIndex].target;
        // needs to set the enemy targeting
        base.SetPosition();
    }

    public override void ApplyData()
    {
        lockOnTargetManager._bLockedOn = cameraDataList[currentIndex].bIsLockedOn;

        //base.ApplyData();
    }
}
