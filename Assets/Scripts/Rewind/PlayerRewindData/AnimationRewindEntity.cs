using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRewindEntity : RewindEntity
{
    // KNOWN BUGS animions unsyncing?

    public List<AnimationTimeData> animationDataList;
    // to be extrated
    [SerializeField]
    private Animator animator;
    public AnimatorClipInfo[] m_CurrentClipInfo;
    

    [SerializeField]
    
    //meeds extraction
    public PlayerFunctions func;

 
    // Start is called before the first frame update
    protected new void Start()
    {  
        animationDataList = new List<AnimationTimeData>();
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
}
