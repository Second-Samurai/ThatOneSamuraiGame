using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Reveal Bow")]
public class RevealBow : Leaf
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
        ArcherAnimationEvents.EquipBow.Run(this.m_ArcherAnimator);
        return NodeResult.success;
    }

    #endregion Methods
  
}
