using Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using UnityEngine;

public class EnemyRewindEntity : AIAnimationRewindEntity
{

    public List<EnemyRewindData> enemyDataList;

    private AISystem aISystem;
    public Collider swordCollider;

    public Rigidbody gameObjectRigidbody;


    // Start is called before the first frame update
    protected new void Start()
    { 
        enemyDataList = new List<EnemyRewindData>();
        base.Start();
         
        aISystem = gameObject.GetComponent<AISystem>();
         
        gameObjectRigidbody = gameObject.GetComponent<Rigidbody>(); 
    }

    public override void FixedUpdate()
    { 
    }
      
    [Obsolete("Deprecated", false)]
    public new void DisableEvents()
    {
        gameObjectRigidbody.isKinematic = true;
        base.DisableEvents();
    }

    [Obsolete("Deprecated", false)]
    public new void EnableEvents()
    {
        gameObjectRigidbody.isKinematic = false;

        base.EnableEvents();
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
