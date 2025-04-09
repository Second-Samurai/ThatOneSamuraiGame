using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float m_LayerWeightLerpTime;

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

        Vector3 _Forward = this.m_ArcherTransform.forward;
        Vector3 _Direction = (this.m_PlayerTransform.Value.position - this.m_ArcherTransform.position).normalized;
        _Direction.y = 0;

        Quaternion _TargetRotation = Quaternion.LookRotation(_Direction);
        // this.m_ArcherTransform.rotation = Quaternion.RotateTowards(
        //     this.m_ArcherTransform.rotation,
        //     _TargetRotation,
        //     this.m_RotationSpeed.Value * Time.deltaTime);

        float _AngleToPlayer = Quaternion.Angle(this.m_ArcherTransform.rotation, _TargetRotation);
        if (_AngleToPlayer <= this.m_AimDetectionThreshold.Value)
        {
            this.m_HasDrawnBow = false;
            this.StartCoroutine(this.RampDownLegAnimatorLayerWeight());
            return NodeResult.success;
        }

        this.StartCoroutine(this.RampUpLegAnimatorLayerWeight());
        
        float _SignedAngle = Vector3.SignedAngle(_Forward, _Direction, Vector3.up);
        if (_SignedAngle > 0f)
            ArcherAnimationEvents.TurnRight.Run(this.m_ArcherAnimator);
        else if (_SignedAngle < 0f)
            ArcherAnimationEvents.TurnLeft.Run(this.m_ArcherAnimator);
        
        return NodeResult.running;
    }

    private IEnumerator RampUpLegAnimatorLayerWeight()
    {
        float _TotalTime = 0;
        while (_TotalTime < this.m_LayerWeightLerpTime)
        {
            ArcherAnimationEvents.SetLegsLayerOverride.Run(
                animator: this.m_ArcherAnimator,
                floatValue: _TotalTime / this.m_LayerWeightLerpTime);
            _TotalTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator RampDownLegAnimatorLayerWeight()
    {
        float _TotalTime = 0;
        while (_TotalTime < this.m_LayerWeightLerpTime)
        {
            ArcherAnimationEvents.SetLegsLayerOverride.Run(
                animator: this.m_ArcherAnimator,
                floatValue: (_TotalTime / this.m_LayerWeightLerpTime) + 1);
            _TotalTime -= Time.deltaTime;
            yield return null;
        }
    }


    #endregion Methods
  
}
