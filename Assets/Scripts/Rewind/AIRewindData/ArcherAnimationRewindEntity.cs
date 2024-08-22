using System.Collections;
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
        _rewindInput = GameManager.instance.RewindManager.GetComponent<RewindManager>();
        archerAnimationDataList = new List<ArcherAnimationTimeData>();
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
        for (int i = currentIndex; i >= 0; i--)
        {
            if (currentIndex <= archerAnimationDataList.Count - 1)
            {
                archerAnimationDataList.RemoveAt(i);
            }
        }
        archerAnimationDataList.TrimExcess();
    }

    public new void RecordPast()
    {
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (archerAnimationDataList.Count > _rewindInput.rewindTime)
        {
            archerAnimationDataList.RemoveAt(archerAnimationDataList.Count - 1);
        }
        m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);

        //move to animation rewind entity
        archerAnimationDataList.Insert(0, new ArcherAnimationTimeData(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, animator.GetCurrentAnimatorStateInfo(0).shortNameHash));

        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + "   :   " + m_CurrentClipInfo[0].clip.name);

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (archerAnimationDataList.Count > 0)
        {
            if (currentIndex < archerAnimationDataList.Count - 1)
            {
                currentIndex++;
                if (currentIndex >= archerAnimationDataList.Count - 1)
                {
                    currentIndex = archerAnimationDataList.Count - 1;
                }
                SetPosition();
            }
            //Debug.LogWarning("animStepBack");
        }
    }

    public override void StepForward()
    {
        if (archerAnimationDataList.Count > 0)
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
        if (currentIndex <= archerAnimationDataList.Count - 1)
        {
            animator.Play(archerAnimationDataList[currentIndex].currentClip, 0, archerAnimationDataList[currentIndex].currentFrame);
        }
        base.SetPosition();


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
