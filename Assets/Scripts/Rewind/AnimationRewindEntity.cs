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

    // to be extracted;
    private PlayerInput playerInput;

    // Start is called before the first frame update
    protected new void Start()
    {
        animationDataList = new List<AnimationTimeData>();
        animator = gameObject.GetComponent<Animator>();

        // to be extracted
        playerInput = gameObject.GetComponent<PlayerInput>();
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (isTravelling == false)
        {
            RecordPast();
            animator.applyRootMotion = true;
            animator.enabled = true;
        }
        else 
        {
            animator.applyRootMotion = false;
          
        }
       

    }

    public new void RecordPast()
    {
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (animationDataList.Count > Mathf.Round(10f * (1f / Time.fixedDeltaTime)))
        {
            animationDataList.RemoveAt(animationDataList.Count - 1);
        }
        m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);

        //move to animation rewind entity
        animationDataList.Insert(0, new AnimationTimeData(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, m_CurrentClipInfo[0].clip.name, 
                                                                    animator.GetFloat("InputSpeed"), animator.GetFloat("XInput"), animator.GetFloat("XInput"), animator.GetBool("LockedOn")));
       
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + "   :   " + m_CurrentClipInfo[0].clip.name);
        
        base.RecordPast();
    }

    public override void StepBack()
    {

        if (animationDataList.Count > 0)
        {
            SetPosition();
            if (currentIndex < animationDataList.Count - 1)
            {
                currentIndex++;
            }
            Debug.LogWarning("animStepBack");
        }
    }

    public override void StepForward()
    {
        if (animationDataList.Count > 0)
        {
            SetPosition();
            if (currentIndex > 0)
            {
                currentIndex--;
            }
            Debug.LogWarning("animStepForward");
        }
    }

    public new void SetPosition()
    {
        animator.enabled = true;
        animator.Play(animationDataList[currentIndex].currentClip, 0, animationDataList[currentIndex].currentFrame);
       // animator.enabled = false;
        animator.SetFloat("InputSpeed", animationDataList[currentIndex].inputSpeed);
        animator.SetFloat("XInput", animationDataList[currentIndex].xInput);
        animator.SetFloat("YInput", animationDataList[currentIndex].yInput);
        animator.SetBool("LockedOn", animationDataList[currentIndex].lockedOn);
        
        //to be extracted

        if (playerInput.bLockedOn != animationDataList[currentIndex].lockedOn)
        {
            playerInput.bLockedOn = animationDataList[currentIndex].lockedOn;
            if (playerInput.bLockedOn)
                playerInput._camControl.LockOn();
            else 
            {
                playerInput._camControl.UnlockCam();
            }
        }
        base.SetPosition();
    }
}
