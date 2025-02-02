using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherRewindEntity : ArcherAnimationRewindEntity
{
    public List<ArcherTimeData> archerDataList;
    public Rigidbody gameObjectRigidbody;

    private BasicArcher basicArcher;


    protected new void Start()
    {  
        base.Start();
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
