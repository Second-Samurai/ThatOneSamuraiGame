using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationRewindEntity : RewindEntity
{
    public List<AIAnimationTimeData> animationDataList;

    [SerializeField]
    private Animator animator;
    public AnimatorClipInfo[] m_CurrentClipInfo;



    // Start is called before the first frame update
    protected new void Start()
    {
         
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
         
    }

    [Obsolete("Deprecated", false)]
    public  void DisableEvents()
    {
        
  
    }

    [Obsolete("Deprecated", false)]
    public  void EnableEvents()
    {
         

    } 

    [Obsolete("Deprecated", false)]
    public new void RecordPast()
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
    public new void SetPosition()
    { 
        base.SetPosition(); 
    }

    [Obsolete("Deprecated", false)]
    public override void ApplyData()
    {
        
    } 
}
