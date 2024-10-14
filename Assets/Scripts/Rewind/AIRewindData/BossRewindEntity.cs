using Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using UnityEngine;

public class BossRewindEntity : EnemyRewindEntity
{

    public List<BossTimeData> bossTimeList;

    private AISystem aISystem;

    private EnemyTracker _enemyTracker;


    // Start is called before the first frame update
    protected new void Start()
    { 
        _enemyTracker = GameManager.instance.EnemyTracker;
        bossTimeList = new List<BossTimeData>();
        base.Start();
         
        aISystem = gameObject.GetComponent<AISystem>();
          
        gameObjectRigidbody = gameObject.GetComponent<Rigidbody>(); 
    }

    public override void FixedUpdate()
    {
         
    }

    //setting rigidbodys to kinimatic

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
