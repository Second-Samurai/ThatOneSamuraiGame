using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Archer/Aim to Player")]
public class AimToPlayer : Leaf
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private TransformReference m_PlayerTransform = new();
    [SerializeField] private FloatReference m_RotationSpeed = new();
    [SerializeField] private FloatReference m_AimDetectionThreshold = new();

    private Animator m_ArcherAnimator;
    private Transform m_ArcherTransform;
    private bool m_HasDrawnBow;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_ArcherAnimator = this.GetComponentInParent<Animator>();
        this.m_ArcherTransform = this.transform.root.transform;
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public override NodeResult Execute()
    {
        if (!this.m_HasDrawnBow)
        {
            this.m_HasDrawnBow = true;
            ArcherAnimationEvents.DrawBow.Run(this.m_ArcherAnimator);
        }
        
        // TODO: We need to implement IK to the archer to allow for vertical aiming
        Vector3 _Direction = (this.m_PlayerTransform.Value.position - this.m_ArcherTransform.position).normalized;
        _Direction.y = 0;

        Quaternion _TargetRotation = Quaternion.LookRotation(_Direction);
        this.m_ArcherTransform.rotation = Quaternion.RotateTowards(
            this.m_ArcherTransform.rotation,
            _TargetRotation,
            this.m_RotationSpeed.Value * Time.deltaTime);

        float _AngleToPlayer = Quaternion.Angle(this.m_ArcherTransform.rotation, _TargetRotation);
        if (_AngleToPlayer <= this.m_AimDetectionThreshold.Value)
        {
            this.m_HasDrawnBow = false;
            return NodeResult.success;
        }
        
        return NodeResult.running;
    }

    #endregion Methods
  
}
