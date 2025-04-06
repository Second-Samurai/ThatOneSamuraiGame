using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Archer/Idle Archer")]
public class ArcherIdle : Leaf
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private int m_RepetitionCount;
    
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
        
        // Allows for selection to occur at start
        this.m_IsIdleClipComplete = true;
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public override NodeResult Execute()
    {
        if (!this.m_IsIdleClipComplete) return NodeResult.success;

        if (this.m_PlayCount > this.m_RepetitionCount)
        {
            this.m_SelectedClipIndex = Random.Range(0, 3);
            this.m_PlayCount = 0;
            ArcherAnimationEvents.ArcherIdle.Run(animator: this.m_ArcherAnimator, intValue: this.m_SelectedClipIndex);
        }
        
        this.m_PlayCount++;
        this.m_IsIdleClipComplete = false;
        
        return NodeResult.success;
    }

    #endregion Methods
  
}