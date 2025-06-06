﻿using ThatOneSamuraiGame.Scripts.Base;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public interface IFieldOfViewOcclusionState
{

    #region - - - - - - Properties - - - - - -

    public bool IsWithinFieldOfView { get; }

    #endregion Properties
  
}

public class ArcherFieldOfViewRigWeightOccluder : PausableMonoBehaviour, IFieldOfViewOcclusionState
{

    #region - - - - - - Fields - - - - - -

    [Header("Rig Controls")]
    [SerializeField, Range(0f, 180f)] private float m_MaxWeightAffectedAngle;
    [SerializeField] private bool m_EnableFadeOut = true;
    [SerializeField] private bool m_EnableFadeIn = true;
    private bool m_IsWithinView;
    
    [Header("Transform Targets")]
    [SerializeField] private Transform m_TargetTransform;
    [SerializeField] private Transform m_CharacterTransform;
    
    // Required Dependencies
    private Rig m_AffectedRig; 
    private RigLayerToggle m_RigLayerToggler;

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public bool IsWithinFieldOfView => this.m_IsWithinView;

    #endregion Properties
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_AffectedRig = this.GetComponent<Rig>();
        this.m_RigLayerToggler = this.GetComponent<RigLayerToggle>();
        
        GameValidator.NotNull(this.m_AffectedRig, nameof(m_AffectedRig));
        GameValidator.NotNull(this.m_RigLayerToggler, nameof(m_RigLayerToggler));
    }

    private void Update()
    {
        if (this.IsPaused || !this.m_RigLayerToggler.IsActive || this.m_RigLayerToggler.IsAnimating) return;
        
        Vector3 _DirectionToTarget = (this.m_TargetTransform.position - this.m_CharacterTransform.position).normalized;
        _DirectionToTarget.y = 0f;

        float _SignedAngle = Vector3.SignedAngle(
            this.m_CharacterTransform.forward, 
            _DirectionToTarget, 
            this.m_CharacterTransform.up);
        float _AbsAngle = Mathf.Abs(_SignedAngle);

        // Handle coroutines for enabled state of animation.
        this.m_IsWithinView = _AbsAngle <= this.m_MaxWeightAffectedAngle;
        if (_AbsAngle > this.m_MaxWeightAffectedAngle 
            && this.m_AffectedRig.weight > 0 
            && this.m_EnableFadeOut)
            this.m_RigLayerToggler.AnimateDisableRigWeight(1);
        else if (_AbsAngle <= this.m_MaxWeightAffectedAngle 
                 && this.m_AffectedRig.weight < 1 
                 && this.m_EnableFadeIn)
            this.m_RigLayerToggler.AnimateEnableRigWeight(1);
    }

    #endregion Unity Methods

    #region - - - - - - Gizmos - - - - - -

    private void OnDrawGizmosSelected()
    {
        if (this.m_TargetTransform == null || this.m_AffectedRig == null)
        {
            this.m_AffectedRig = this.GetComponent<Rig>();
            this.m_RigLayerToggler = this.GetComponent<RigLayerToggle>();
            return;
        }
        
        Vector3 _Origin = this.m_CharacterTransform.position;
        Vector3 _Forward = this.m_CharacterTransform.forward;
        Vector3 _Up = this.m_CharacterTransform.up;

        float _TotalAngle = this.m_MaxWeightAffectedAngle;
        float _ViewRange = this.m_MaxWeightAffectedAngle;

#if UNITY_EDITOR
        // Draw inner "full weight" arc
        Handles.color = new Color(1f, 1f, 0f, 0.25f); // Yellow
        Handles.DrawSolidArc(_Origin, _Up, Quaternion.AngleAxis(-_ViewRange, _Up) * _Forward, _ViewRange * 2f, 1.5f);
#endif

        // Draw direction to target
        Vector3 _ToTarget = (this.m_TargetTransform.position - _Origin).normalized;
        _ToTarget.y = 0f; // strictly in the x-y plane
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_Origin, this.m_TargetTransform.position);

        float _SignedAngle = Vector3.SignedAngle(_Forward, _ToTarget, _Up);
        float _AbsAngle = Mathf.Abs(_SignedAngle);
        float _T = 0f;
        
        if (_AbsAngle > _ViewRange && _AbsAngle <= this.m_MaxWeightAffectedAngle)
            _T = 1f - Mathf.Clamp01((_AbsAngle - _ViewRange) / _ViewRange);
        else if (_AbsAngle <= _ViewRange)
            _T = 1;

#if UNITY_EDITOR
        // Display rig information debug panel
        Handles.color = Color.magenta;
        Handles.Label(_Origin + Vector3.up * 2f, 
            $"{this.gameObject.name}\n" +
            $"Angle: {_SignedAngle:F1}°\n" +
            $"Weight: {this.m_AffectedRig.weight:F2}\n" +
            $"SignedAngle: {_SignedAngle:F2}\n" +
            $"CurveTime: {_T}\n" +
            $"FadeArc: {(_AbsAngle - _ViewRange):F2} / FullFadeArc: {(_TotalAngle - _ViewRange):F2}");
#endif
    }

    #endregion Gizmos
  
}
