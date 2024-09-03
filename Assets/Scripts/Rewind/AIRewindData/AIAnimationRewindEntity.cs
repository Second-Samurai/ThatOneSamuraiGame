﻿using System.Collections;
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
        _rewindInput = GameManager.instance.RewindManager.GetComponent<RewindManager>();
        animationDataList = new List<AIAnimationTimeData>();
        animator = gameObject.GetComponent<Animator>();

        _rewindInput.Reset += ResetTimeline;
        _rewindInput.OnStartRewind += DisableEvents;
        _rewindInput.OnEndRewind += EnableEvents;
        _rewindInput.OnEndRewind += ApplyData;

        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (_rewindInput.isTravelling == false)
        {
            RecordPast();
            
           // animator.enabled = true;
        }
        else
        {
           

        }


    }

    public  void DisableEvents()
    {
       
        animator.fireEvents = false;
        animator.applyRootMotion = false;
  
    }

    public  void EnableEvents()
    {
        animator.fireEvents = true;
        animator.applyRootMotion = true;

    }

    public new void ResetTimeline()
    {
        for (int i = currentIndex; i >= 0; i--)
        {
            if (currentIndex <= animationDataList.Count - 1)
            {
                animationDataList.RemoveAt(i);
            }
        }
        animationDataList.TrimExcess();
    }

    public new void RecordPast()
    {
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (animationDataList.Count > _rewindInput.rewindTime)
        {
            animationDataList.RemoveAt(animationDataList.Count - 1);
        }
        m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);

        //move to animation rewind entity
         animationDataList.Insert(0, new AIAnimationTimeData(
             animator.GetCurrentAnimatorStateInfo(0).normalizedTime,
             animator.GetCurrentAnimatorStateInfo(0).shortNameHash,
             animator.GetFloat("MovementX"),
             animator.GetFloat("MovementZ")));

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (animationDataList.Count > 0)
        {
            if (currentIndex < animationDataList.Count - 1)
            {
                currentIndex++;
                if (currentIndex >= animationDataList.Count - 1)
                {
                    currentIndex = animationDataList.Count - 1;
                }
                SetPosition();
            }
            //Debug.LogWarning("animStepBack");
        }
    }

    public override void StepForward()
    {
        if (animationDataList.Count > 0)
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
        
        base.SetPosition();
        // animator.enabled = true;
        // animator.enabled = false;
        if (currentIndex <= animationDataList.Count - 1)
        {
            animator.SetFloat("MovementX", animationDataList[currentIndex].movementX);
            animator.SetFloat("MovementZ", animationDataList[currentIndex].movementZ);

            animator.Play(animationDataList[currentIndex].currentClip, 0, animationDataList[currentIndex].currentFrame);
        }
    }
    public override void ApplyData()
    {
        
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
