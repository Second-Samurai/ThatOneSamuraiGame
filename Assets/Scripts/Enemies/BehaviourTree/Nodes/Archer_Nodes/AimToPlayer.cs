using System.Collections;
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

    [Space]
    [SerializeField] private AnimationCurve m_EnableWeightCurves;
    [SerializeField] private AnimationCurve m_DisableWeightCurves;

    private Animator m_ArcherAnimator;
    private AnimationRigControl m_RigControl;
    private Transform m_ArcherTransform;
    private bool m_CanMakeTurnMotion = true;
    private bool m_HasDrawnBow;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_ArcherAnimator = this.GetComponentInParent<Animator>();
        this.m_RigControl = this.GetComponentInParent<AnimationRigControl>();
        this.m_ArcherTransform = this.transform.root.transform;

        ArcherAnimationReciever _AnimationReceiver = this.GetComponentInParent<ArcherAnimationReciever>();
        _AnimationReceiver.OnTurnCompletion.AddListener(() => this.m_CanMakeTurnMotion = true);
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public override NodeResult Execute()
    {
        if (!this.m_HasDrawnBow)
        {
            this.m_HasDrawnBow = true;
            ArcherAnimationEvents.DrawBow.Run(this.m_ArcherAnimator);
            
            this.m_RigControl.StopAllCoroutines();
            this.m_RigControl.StartCoroutine(this.RampUpLegAnimatorLayerWeight());
        }

        Vector3 _Forward = this.m_ArcherTransform.forward;
        Vector3 _Direction = (this.m_PlayerTransform.Value.position - this.m_ArcherTransform.position).normalized;
        _Direction.y = 0;
        Quaternion _TargetRotation = Quaternion.LookRotation(_Direction);
        
        float _AngleToPlayer = Quaternion.Angle(this.m_ArcherTransform.rotation, _TargetRotation);
        if (_AngleToPlayer <= this.m_AimDetectionThreshold.Value)
        {
            this.m_HasDrawnBow = false;
            this.m_RigControl.StopAllCoroutines();
            this.m_RigControl.StartCoroutine(this.RampDownLegAnimatorLayerWeight());
            return NodeResult.success;
        }

        if (this.m_CanMakeTurnMotion)
        {
            // Trigger turn motion
            float _SignedAngle = Vector3.SignedAngle(_Forward, _Direction, Vector3.up);
            if (_SignedAngle > 0f)
            {
                ArcherAnimationEvents.TurnRight.Run(this.m_ArcherAnimator);
                this.m_CanMakeTurnMotion = false;
            }
            else if (_SignedAngle < 0f)
            {
                ArcherAnimationEvents.TurnLeft.Run(this.m_ArcherAnimator);
                this.m_CanMakeTurnMotion = false;
            }
        }
        
        return NodeResult.running;
    }

    private IEnumerator RampUpLegAnimatorLayerWeight()
    {
        float _TotalTime = 0;
        while (_TotalTime < this.m_LayerWeightLerpTime)
        {
            float _T = Mathf.Clamp01(_TotalTime / this.m_LayerWeightLerpTime);
            ArcherAnimationEvents.SetLegsLayerOverride.Run(
                animator: this.m_ArcherAnimator,
                floatValue: this.m_EnableWeightCurves.Evaluate(_T));
            _TotalTime += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator RampDownLegAnimatorLayerWeight()
    {
        float _TotalTime = 0;
        while (_TotalTime < this.m_LayerWeightLerpTime)
        {
            float _T = Mathf.Clamp01(1 - _TotalTime / this.m_LayerWeightLerpTime);
            ArcherAnimationEvents.SetLegsLayerOverride.Run(
                animator: this.m_ArcherAnimator,
                floatValue: this.m_DisableWeightCurves.Evaluate(_T));
            _TotalTime += Time.deltaTime;
            yield return null;
        }
    }


    #endregion Methods
  
}
