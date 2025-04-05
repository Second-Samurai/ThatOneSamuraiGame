using MBT;
using UnityEngine;

[MBTNode(name = "Tasks/Idle Archer")]
public class ArcherIdle : Leaf
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private IntReference m_RepetitionCount = new();
    
    private Animator m_ArcherAnimator;
    private bool m_IsIdleClipComplete;
    private int m_PlayCount;
    private int m_SelectedClipIndex;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_ArcherAnimator = this.GetComponentInParent<Animator>();
        
        ArcherAnimationReciever _AnimationEventReceiver = this.GetComponentInParent<ArcherAnimationReciever>();
        _AnimationEventReceiver.OnIdleCompletion.AddListener(() => this.m_IsIdleClipComplete = true);
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public override NodeResult Execute()
    {
        if (!this.m_IsIdleClipComplete) return NodeResult.success;
        
        if (this.m_PlayCount > this.m_RepetitionCount.Value)
            this.m_SelectedClipIndex = Random.Range(0, 3);
        
        ArcherAnimationEvents.ArcherIdle.Run(animator: this.m_ArcherAnimator, intValue: this.m_SelectedClipIndex);
        return NodeResult.success;
    }

    #endregion Methods
  
}