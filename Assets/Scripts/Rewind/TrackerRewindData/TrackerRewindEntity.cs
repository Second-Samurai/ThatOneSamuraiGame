using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;

public class TrackerRewindEntity : RewindEntity
{
    private EnemyTracker _enemyTracker;
    private LockOnTracker _lockOnTracker;
    
    public List<TrackerRewindData> trackerDataList;
    
    // Start is called before the first frame update
    void Start()
    {
        _enemyTracker = GameManager.instance.EnemyTracker;
        _lockOnTracker = GameManager.instance.LockOnTracker;
        
        trackerDataList = new List<TrackerRewindData>();
        base.Start();
            
        _rewindInput.Reset += ResetTimeline;
        _rewindInput.OnEndRewind += EnableEvents;
        _rewindInput.OnStartRewind += DisableEvents;
        _rewindInput.OnEndRewind += ApplyData;
    }

    public override void FixedUpdate()
    {
        if (_rewindInput.isTravelling == false)
        {
            RecordPast();
        }
    }
    
    public void DisableEvents()
    {
        
    }

    public void EnableEvents()
    {
        
    }
    
    public new void RecordPast()
    {
        //maybe make 10f into a global variable
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (trackerDataList.Count > _rewindInput.rewindTime)
        {
            trackerDataList.RemoveAt(trackerDataList.Count - 1);
        }

        //move to arguments need to be added rewind entity
        trackerDataList.Insert(0, new TrackerRewindData(_enemyTracker.currentEnemies, _lockOnTracker.currentEnemies));

        //base.RecordPast();
    }
    
    public override void StepBack()
    {
        if (trackerDataList.Count > 0)
        {
            if (currentIndex < trackerDataList.Count - 1)
            {
                currentIndex++;
                if (currentIndex >= trackerDataList.Count - 1)
                {
                    currentIndex = trackerDataList.Count - 1;
                }
                SetPosition();
            }
        }
    }
    
    public override void StepForward()
    {
        if (trackerDataList.Count > 0)
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
        //base.SetPosition();
    }
    
    public override void ApplyData()
    {
        _lockOnTracker.targetableEnemies.Clear();
        _enemyTracker.currentEnemies = trackerDataList[currentIndex].trackedCurrentEnemies.ToList<Transform>();
        _lockOnTracker.currentEnemies = trackerDataList[currentIndex].lockOnEnemies.ToList<Transform>();
        
        //_lockOnTracker.targetableEnemies = _lockOnTracker.currentEnemies;
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
