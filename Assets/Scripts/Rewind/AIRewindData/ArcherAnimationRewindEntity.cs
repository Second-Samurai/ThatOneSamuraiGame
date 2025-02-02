using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimationRewindEntity : RewindEntity
{
    public List<ArcherAnimationTimeData> archerAnimationDataList;

    [SerializeField]
    private Animator animator;
    public AnimatorClipInfo[] m_CurrentClipInfo;



    // Start is called before the first frame update
    protected new void Start()
    { 
        archerAnimationDataList = new List<ArcherAnimationTimeData>();
        animator = gameObject.GetComponent<Animator>();
          
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
         
    }

    public void DisableEvents()
    {
        animator.fireEvents = false;
        animator.applyRootMotion = false; 
    }

    public void EnableEvents()
    {
        animator.fireEvents = true;
        animator.applyRootMotion = true; 
    }

    public new void ResetTimeline()
    {
        
    }

    [Obsolete("Deprecated", false)]
    public new void RecordPast()
    { 
        base.RecordPast();
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
    public new void SetPosition()
    { 
        base.SetPosition();
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
