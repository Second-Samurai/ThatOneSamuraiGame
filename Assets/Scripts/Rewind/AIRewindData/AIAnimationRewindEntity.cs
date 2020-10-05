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
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
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
            animationDataList.RemoveAt(i);
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
        // animationDataList.Insert(0, new AIAnimationTimeData(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, animator.GetCurrentAnimatorStateInfo(0).shortNameHash,
        //                                                              animator.GetBool("PlayerFound"), animator.GetBool("IsLightAttacking"), animator.GetBool("IsApproaching"), animator.GetBool("IsGuardBroken"), animator.GetBool("IsDead"),
        //                                                              animator.GetBool("IsQuickBlocking"), animator.GetBool("IsBlocking"), animator.GetBool("IsParried"), animator.GetBool("IsStrafing"), 
        //                                                              animator.GetFloat("StrafeDirectionX"), animator.GetBool("IsDodging"), animator.GetFloat("DodgeDirectionX"), animator.GetFloat("DodgeDirectionZ"),
        //                                                              animator.GetBool("IsClosingDistance")));

        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + "   :   " + m_CurrentClipInfo[0].clip.name);

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (animationDataList.Count > 0)
        {
            if (currentIndex < animationDataList.Count - 1)
            {
              SetPosition();
                currentIndex++;
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
           // Debug.LogWarning("animStepForward");
        }
    }

    public new void SetPosition()
    {
        base.SetPosition();
       // animator.enabled = true;
        // animator.enabled = false;
     
        animator.SetBool("IsDead", animationDataList[currentIndex].bIsDead);
      
        animator.SetFloat("StrafeDirectionX", animationDataList[currentIndex].StrafeDirectionX);
        animator.SetFloat("DodgeDirectionX", animationDataList[currentIndex].dodgeDirectionX);
        animator.SetFloat("DodgeDirectionZ", animationDataList[currentIndex].dodgeDirectionZ);
        animator.Play(animationDataList[currentIndex].currentClip, 0, animationDataList[currentIndex].currentFrame);
    }
    public override void ApplyData()
    {
        animator.SetBool("PlayerFound", animationDataList[currentIndex].bPlayerFound);
        animator.SetBool("IsLightAttacking", animationDataList[currentIndex].bIsLightAttacking);
        animator.SetBool("IsApproaching", animationDataList[currentIndex].bIsApproaching);
        animator.SetBool("IsGuardBroken", animationDataList[currentIndex].bIsGuardBroken);
        animator.SetBool("IsQuickBlocking", animationDataList[currentIndex].IsQuickBlocking);
        animator.SetBool("IsBlocking", animationDataList[currentIndex].IsBlocking);
        animator.SetBool("IsParried", animationDataList[currentIndex].IsParried);
        animator.SetBool("IsStrafing", animationDataList[currentIndex].IsStrafing);
        animator.SetBool("IsDodging", animationDataList[currentIndex].IsDodging);
        animator.SetBool("IsClosingDistance", animationDataList[currentIndex].IsClosingDistance);

    }

}
