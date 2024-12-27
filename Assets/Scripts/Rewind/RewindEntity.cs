using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindEntity : MonoBehaviour
{
    public bool isTravelling = false;
    public int currentIndex = 0;
    [SerializeField]
    public List<PositionalTimeData> transformDataList;
    public Transform thisTransform;
      
    protected void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        
    }
     
    [Obsolete("Deprecated", false)]
    public void RecordPast() 
    {
         
    }

    [Obsolete("Deprecated", false)]
    public void ResetTimeline() 
    {
        
    }

    [Obsolete("Deprecated", false)]
    public virtual void StepBack() 
    {
 
    }

    [Obsolete("Deprecated", false)]
    public virtual void StepForward()
    {
        
    }

    [Obsolete("Deprecated", false)]
    public void SetPosition() 
    {
        
    }

    [Obsolete("Deprecated", false)]
    public virtual void ApplyData()
    {

    }
    protected void OnDestroy()
    { 

    }
}
