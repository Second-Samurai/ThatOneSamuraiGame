using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Archer/Fire Arrow")]
public class FireArrow : Leaf
{

    #region - - - - - - Fields - - - - - -

    private Animator m_ArcherAnimator;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start() 
        => this.m_ArcherAnimator = this.GetComponentInParent<Animator>();

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    public override NodeResult Execute()
    {
        Debug.Log("Arrow Fired");
        ArcherAnimationEvents.FireBow.Run(this.m_ArcherAnimator);
        
        return NodeResult.success;
    }

    #endregion Methods
  
}
