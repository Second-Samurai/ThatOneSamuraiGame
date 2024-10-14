using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRewindEntity : RewindEntity
{
    public List<ProjectileTimeData> ProjectileDataList;
    public Rigidbody gameObjectRigidbody;

    public Projectile projectile;


    protected new void Start()
    { 
        ProjectileDataList = new List<ProjectileTimeData>();

        projectile = gameObject.GetComponent<Projectile>();
          
        gameObjectRigidbody = gameObject.GetComponent<Rigidbody>();
         
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        
    }

    public void DisableEvents()
    {
        gameObjectRigidbody.isKinematic = true; 

    }

    public void EnableEvents()
    {
        gameObjectRigidbody.isKinematic = false; 
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
}
