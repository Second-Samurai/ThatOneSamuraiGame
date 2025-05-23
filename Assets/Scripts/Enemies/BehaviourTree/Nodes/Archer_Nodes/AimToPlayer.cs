﻿using System.Collections;
using System.Linq;
using MBT;
using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Archer/Aim to Player")]
public class AimToPlayer : Leaf
{

    #region - - - - - - Fields - - - - - -

    [FormerlySerializedAs("m_PlayerTransform")]
    [Header("Aiming Modifiers")]
    [SerializeField] private TransformReference m_PlayerTransformReference = new();
    [SerializeField] private TransformReference m_ArcherTransformReference = new();
    [SerializeField] private FloatReference m_AimDetectionThreshold = new();
    [SerializeField] private float m_LayerWeightLerpTime;
    [SerializeField] private float m_TurnAngleSize;

    [Space]
    [SerializeField] private AnimationCurve m_EnableWeightCurves;
    [SerializeField] private AnimationCurve m_DisableWeightCurves;

    // Required Dependencies
    private Animator m_ArcherAnimator;
    private AnimationRigControl m_RigControl;
    
    // Turn motion fields
    private bool m_CanMakeTurnMotion = true;
    private float m_TurnLeftClipLength;
    private float m_TurnRightClipLength;
    
    // Aiming calculation fields
    private float m_AngleToPlayer;
    private Vector3 m_DirectionToTarget;
    private Vector3 m_Forward;
    private bool m_HasDrawnBow;
    private Quaternion m_TargetRotation;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_ArcherAnimator = this.GetComponentInParent<Animator>();
        this.m_RigControl = this.GetComponentInParent<AnimationRigControl>();

        GameValidator.NotNull(this.m_PlayerTransformReference, nameof(m_PlayerTransformReference));
        GameValidator.NotNull(this.m_ArcherTransformReference, nameof(m_ArcherTransformReference));

        ArcherAnimationReciever _AnimationReceiver = this.GetComponentInParent<ArcherAnimationReciever>();
        _AnimationReceiver.OnTurnCompletion.AddListener(() => this.m_CanMakeTurnMotion = true);

        this.m_TurnLeftClipLength = this.m_ArcherAnimator.runtimeAnimatorController.animationClips
            .SingleOrDefault(c => c.name == ArcherAnimationEvents.TurnLeft.ClipName)?.length ?? 1f;
        this.m_TurnRightClipLength = this.m_ArcherAnimator.runtimeAnimatorController.animationClips
            .SingleOrDefault(c => c.name == ArcherAnimationEvents.TurnRight.ClipName)?.length ?? 1f;
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public override NodeResult Execute()
    {
        if (!this.m_HasDrawnBow)
        {
            this.m_HasDrawnBow = true;
            ArcherAnimationEvents.DrawBow.Run(this.m_ArcherAnimator);
            this.m_RigControl.ActivateAllLayers();
            
            this.m_RigControl.StopAllCoroutines();
            this.m_RigControl.StartCoroutine(this.AnimateLegsLayerMasterWeight(this.m_EnableWeightCurves));
        }

        // Calculate target rotation
        this.m_Forward = this.m_ArcherTransformReference.Value.forward;
        this.m_DirectionToTarget = 
            (this.m_PlayerTransformReference.Value.position - this.m_ArcherTransformReference.Value.position).normalized;
        this.m_DirectionToTarget.y = 0;
        Quaternion _TargetRotation = Quaternion.LookRotation(this.m_DirectionToTarget);
        
        // Trigger disable action
        this.m_AngleToPlayer = Quaternion.Angle(this.m_ArcherTransformReference.Value.rotation, _TargetRotation);
        if (this.m_AngleToPlayer <= this.m_AimDetectionThreshold.Value)
        {
            this.m_HasDrawnBow = false;
            this.m_RigControl.StartCoroutine(this.AnimateLegsLayerMasterWeight(this.m_DisableWeightCurves));
            this.StopAllCoroutines();
            
            return NodeResult.success;
        }

        this.TriggerTurnMotion();
        
        return NodeResult.running;
    }

    private IEnumerator AnimateLegsLayerMasterWeight(AnimationCurve selectedCurve)
    {
        float _TotalTime = 0;
        while (_TotalTime < this.m_LayerWeightLerpTime)
        {
            float _T = Mathf.Clamp01(_TotalTime / this.m_LayerWeightLerpTime);
            ArcherAnimationEvents.SetLegsLayerOverride.Run(
                animator: this.m_ArcherAnimator,
                floatValue: selectedCurve.Evaluate(_T));
            _TotalTime += Time.deltaTime;
            yield return null;
        }
    }

    private void TriggerTurnMotion()
    {
        if (!this.m_CanMakeTurnMotion) return;
        
        float _SignedAngle = Vector3.SignedAngle(this.m_Forward, this.m_DirectionToTarget, Vector3.up);
        if (_SignedAngle > 0f)
        {
            ArcherAnimationEvents.TurnRight.Run(this.m_ArcherAnimator);
            this.StartCoroutine(this.TurnRotationOverride(this.m_TurnRightClipLength, _SignedAngle));
        }
        else if (_SignedAngle < 0f)
        {
            ArcherAnimationEvents.TurnLeft.Run(this.m_ArcherAnimator);
            this.StartCoroutine(this.TurnRotationOverride(this.m_TurnLeftClipLength, _SignedAngle));
        }
    }
    
    // This is done since root motion could not be properly applied on anything but the base layer
    // Its highly possible that this is also a bug 'https://discussions.unity.com/t/root-motion-from-additive-layer-not-applied/845597'
    private IEnumerator TurnRotationOverride(float clipLength, float signedAngle)
    {
        float _Angle = signedAngle > 0f ? this.m_TurnAngleSize : -this.m_TurnAngleSize;
        Quaternion _StartRotation = this.m_ArcherTransformReference.Value.rotation;
        Quaternion _EndRotation = _StartRotation * Quaternion.Euler(0f, _Angle, 0f);

        float _ElapsedTime = 0f;
        while (_ElapsedTime < clipLength)
        {
            this.m_ArcherTransformReference.Value.rotation = Quaternion.Slerp(
                _StartRotation, 
                _EndRotation, 
                _ElapsedTime / clipLength);
            
            _ElapsedTime += Time.deltaTime;
            yield return null;
        }

        this.m_ArcherTransformReference.Value.rotation = _EndRotation;
        this.m_CanMakeTurnMotion = false;
    }

    #endregion Methods
  
}
