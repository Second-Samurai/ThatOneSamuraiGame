using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRewindEntity : RewindEntity
{

    // FUNCTIONALITY NEEDS TO BE ADDED
    [SerializeField]
    public List<CameraTimeData> cameraDataList;
    private LockOnTargetManager lockOnTargetManager;
    private CameraControl cameraControl;
    private int camPriority;
    // Start is called before the first frame update
    protected new void Start()
    {
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        cameraDataList = new List<CameraTimeData>();
        lockOnTargetManager = GameManager.instance.playerController.gameObject.GetComponentInChildren<LockOnTargetManager>();
        cameraControl = GameManager.instance.playerController.gameObject.GetComponent<CameraControl>();
        _rewindInput.Reset += ResetTimeline;
        _rewindInput.OnEndRewind += ApplyData;
        _rewindInput.OnEndRewind += EnableEvents;
        _rewindInput.OnStartRewind += DisableEvents;

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
        cameraDataList.Insert(0, new CameraTimeData(lockOnTargetManager._bLockedOn, lockOnTargetManager._target, lockOnTargetManager._player, lockOnTargetManager.cam.m_Priority));

        base.RecordPast();
    }
    public void DisableEvents()
    {
        camPriority = lockOnTargetManager.cam.m_Priority;
       // lockOnTargetManager.cam.m_Priority = 1;

    }

    public void EnableEvents()
    {
        lockOnTargetManager.cam.m_Priority = cameraDataList[currentIndex].priority;
    }

    public override void StepBack()
    {

        if (cameraDataList.Count > 0)
        {
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
            if (currentIndex > 0)
            {
                SetPosition();
                currentIndex--;
            }
        }
    }

    public new void SetPosition()
    {
        if (currentIndex <= cameraDataList.Count - 1)
        {
            if (cameraDataList[currentIndex].target != null && cameraDataList[currentIndex].player != null)
            {
                lockOnTargetManager._target = cameraDataList[currentIndex].target;
                lockOnTargetManager._player = cameraDataList[currentIndex].player;
                lockOnTargetManager.SetTarget(cameraDataList[currentIndex].target, cameraDataList[currentIndex].player);
            }
        }
        // needs to set the enemy targeting
        base.SetPosition();
    }

    public override void ApplyData()
    {
        lockOnTargetManager._bLockedOn = false;
        cameraControl.UnlockCam();
        
        // lockOnTargetManager._bLockedOn = cameraDataList[currentIndex].bIsLockedOn;
        // if (lockOnTargetManager._bLockedOn == false) 
        // {
        //     cameraControl.UnlockCam();
        // }

        //base.ApplyData();
    }
}
