using System.Collections;
using System.Collections.Generic;
using Player.Animation;
using UnityEngine;

public class AnimationRewindEntity : RewindEntity
{
    // KNOWN BUGS animions unsyncing?

    public List<AnimationTimeData> animationDataList;
    // to be extrated
    private PlayerAnimationComponent m_PlayerAnimationComponent;
    //public AnimatorClipInfo[] m_CurrentClipInfo;
    

    [SerializeField]
    
    //needs extraction
    public PlayerFunctions func;

 
    // Start is called before the first frame update
    protected new void Start()
    {
        _rewindInput = GameManager.instance.RewindManager.GetComponent<RewindManager>();
        animationDataList = new List<AnimationTimeData>();
        m_PlayerAnimationComponent = GetComponent<PlayerAnimationComponent>();
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
    }
    
    protected void DisableEvents()
    {
        m_PlayerAnimationComponent.SetFireEvents(false);
        m_PlayerAnimationComponent.SetRootMotion(false);
    }
    
    protected void EnableEvents()
    {
        m_PlayerAnimationComponent.SetFireEvents(true);
        m_PlayerAnimationComponent.SetRootMotion(true);
    }
    
    private new void ResetTimeline()
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
        //m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);

        //move to animation rewind entity
        animationDataList.Insert(0, m_PlayerAnimationComponent.GetAnimationTimeData());
       
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

    protected new void SetPosition()
    {
        base.SetPosition();
        
        if (currentIndex <= animationDataList.Count - 1)
        {
            func.bIsDead = animationDataList[currentIndex].isDead;

            Vector2 inputDirection = new Vector2(animationDataList[currentIndex].xInput,
                animationDataList[currentIndex].yInput);
            m_PlayerAnimationComponent.SetInputSpeed(animationDataList[currentIndex].inputSpeed);
            m_PlayerAnimationComponent.SetInputDirection(inputDirection);
            m_PlayerAnimationComponent.SetDead(animationDataList[currentIndex].isDead);
            m_PlayerAnimationComponent.SetAnimationOverride(
                animationDataList[currentIndex].currentClip,
                animationDataList[currentIndex].currentFrame);
        }
    }

    public override void ApplyData()
    {
        m_PlayerAnimationComponent.SetAnimationTimeData(animationDataList[currentIndex]);
    }
}
