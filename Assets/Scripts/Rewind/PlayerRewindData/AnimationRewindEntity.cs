using System.Collections;
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
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        animationDataList = new List<AnimationTimeData>();
        animator = gameObject.GetComponent<Animator>();
        _rewindInput.Reset += ResetTimeline;
        _rewindInput.OnEndRewind += EnableEvents;
        _rewindInput.OnStartRewind += DisableEvents;
        _rewindInput.OnEndRewind += ApplyData;
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (_rewindInput.isTravelling == false)
        {
            RecordPast();
            
            //animator.enabled = true;
        }
        else 
        {
            
          
        }
 
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
        for (int i = currentIndex; i > 0; i--)
        {
            if (currentIndex <= animationDataList.Count - 1)
            {
                animationDataList.RemoveAt(i);
            }
        }
        animationDataList.TrimExcess();

        //base.ResetTimeline();
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
        animationDataList.Insert(0, new AnimationTimeData(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 
                                                                    animator.GetFloat("InputSpeed"), animator.GetFloat("XInput"), animator.GetFloat("YInput"), animator.GetBool("LockedOn"), 
                                                                        animator.GetBool("VGuard"), animator.GetInteger("ComboCount"), animator.GetBool("FirstAttack"), animator.GetBool("SecondAttack"), 
                                                                            animator.GetBool("LoopAttack"), animator.GetBool("isDead"), animator.GetBool("HeavyAttackHeld"), animator.GetBool("FinisherSetup")));
       
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
           // Debug.LogWarning("animStepBack");
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
           // Debug.LogWarning("animStepForward");
        }
    }

    public new void SetPosition()
    {
        base.SetPosition();
        //animator.enabled = true;
        // animator.enabled = false;
        if (currentIndex <= animationDataList.Count - 1)
        {
            func.bIsDead = animationDataList[currentIndex].isDead;

            animator.SetFloat("InputSpeed", animationDataList[currentIndex].inputSpeed);
            animator.SetFloat("XInput", animationDataList[currentIndex].xInput);
            animator.SetFloat("YInput", animationDataList[currentIndex].yInput);
            animator.SetBool("isDead", animationDataList[currentIndex].isDead);


            animator.Play(animationDataList[currentIndex].currentClip, 0, animationDataList[currentIndex].currentFrame);
        }
    }

    public override void ApplyData()
    {

        animator.SetBool("LockedOn", animationDataList[currentIndex].lockedOn);
        animator.SetBool("VGuard", animationDataList[currentIndex].vGuard);
        animator.SetInteger("ComboCount", animationDataList[currentIndex].comboCount);
        animator.SetBool("FirstAttack", animationDataList[currentIndex].firstAttack);
        animator.SetBool("SecondAttack", animationDataList[currentIndex].secondAttack);
        animator.SetBool("LoopAttack", animationDataList[currentIndex].loopAttack);
        animator.SetBool("HeavyAttackHeld", animationDataList[currentIndex].HeavyAttackHeld);
        animator.SetBool("FinisherSetup", animationDataList[currentIndex].FinisherSetup);
    }
}
