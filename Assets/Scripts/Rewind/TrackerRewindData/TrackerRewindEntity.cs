using System;
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
    }

    public override void FixedUpdate()
    { 
    } 

    [Obsolete("Deprecated", false)]
    public override void StepBack()
    {
         
    }

    [Obsolete("Deprecated", false)]
    public override void StepForward()
    {
         
    }

    [Obsolete("Deprecated", false)]
    public override void ApplyData()
    {
         
    }

    protected new void OnDestroy()
    { 
        base.OnDestroy();
    }
}
