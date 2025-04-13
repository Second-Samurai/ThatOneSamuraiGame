using System;
using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Base;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationRigControl : PausableMonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public List<RigLayerMasterControl> RigWeightLayers;
    public Transform AimTarget;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        foreach (IRigLayerControl _RigLayerControl in this.RigWeightLayers)
            _RigLayerControl.OnStart(this, this.AimTarget);
    }

    private void Update()
    {
        if (this.IsPaused) return;

        for(int i = 0; i < this.RigWeightLayers.Count; i++)
            this.RigWeightLayers[i].UpdateControl();
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    public void ActivateAllLayers()
    {
        foreach(IRigLayerControl _RigLayerControl in this.RigWeightLayers)
            _RigLayerControl.IsActive = true;
    }

    // Todo: Implement exit behaviour when the player is too far away.
    public void DeActivateAllLayers()
    {
        foreach (IRigLayerControl _RigLayerControl in this.RigWeightLayers)
            _RigLayerControl.IsActive = false;
    }

    #endregion Methods

    #region - - - - - - Gizmos - - - - - -

    private void OnDrawGizmos()
    {
        if (this.RigWeightLayers == null) return;

        foreach (var rigLayer in this.RigWeightLayers)
        {
            if (rigLayer is RigLayerMasterControl masterControl)
                masterControl.DrawDebugGizmos(transform, this.AimTarget);
        }
    }

    #endregion Gizmos
  
}

public interface IRigLayerControl
{

    #region - - - - - - Properties - - - - - -

    bool IsActive { get; set; }

    #endregion Properties
  
    #region - - - - - - Methods - - - - - -

    void OnStart(AnimationRigControl rigControl, Transform aimTarget);
    
    void UpdateControl();

    #endregion Methods

}

[Serializable]
public class RigLayerMasterControl : IRigLayerControl
{

    #region - - - - - - Fields - - - - - -

    public string m_LayerName;
    public bool m_IsActive;
    [Range(0f, 180f)]
    public float m_MaxWeightAffectedAngle;
    public float m_ViewPaddingAngle;
    public AnimationCurve m_FrustumWeightCurve;
    public Rig m_AffectedRig;

    private Transform m_TargetTransform;
    private Transform m_CharacterTransform;

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public bool IsActive
    {
        get => this.m_IsActive;
        set => this.m_IsActive = value;
    }

    #endregion Properties
  
    #region - - - - - - Methods - - - - - -

    public void OnStart(AnimationRigControl rigControl, Transform aimTarget)
    {
        this.m_TargetTransform = aimTarget;
        this.m_CharacterTransform = rigControl.transform;
    }

    public void UpdateControl()
    {
        if (!this.m_IsActive) return;
        
        Vector3 _DirectionToTarget = (this.m_TargetTransform.position - this.m_CharacterTransform.position).normalized;
        Vector3 _Forward = this.m_CharacterTransform.forward;
        Vector3 _Up = this.m_CharacterTransform.up;

        float _SignedAngle = Vector3.SignedAngle(_Forward, _DirectionToTarget, _Up);
        float _AbsSignedAngle = Mathf.Abs(_SignedAngle);
        float _WithinAngleRange = this.m_MaxWeightAffectedAngle - m_ViewPaddingAngle;
        float _FadeAngle = _AbsSignedAngle - _WithinAngleRange;
        float _T = 1f - Mathf.Clamp01(_FadeAngle / _WithinAngleRange);

        this.m_AffectedRig.weight = this.m_FrustumWeightCurve.Evaluate(_T);
    }

    #endregion Methods

    #region - - - - - - Gizmos - - - - - -

    public void DrawDebugGizmos(Transform characterTransform, Transform aimTarget)
    {
        if (aimTarget == null || this.m_AffectedRig == null) return;
        
        Vector3 _Origin = characterTransform.position;
        Vector3 _Forward = characterTransform.forward;
        Vector3 _Up = characterTransform.up;

        float _TotalAngle = m_MaxWeightAffectedAngle;
        float _ViewRange = m_MaxWeightAffectedAngle - m_ViewPaddingAngle;

        // Draw total view arc
#if UNITY_EDITOR
        Handles.color = new Color(0f, 1f, 0f, 0.15f); // Green
        Handles.DrawSolidArc(_Origin, _Up, Quaternion.AngleAxis(-_TotalAngle, _Up) * _Forward, _TotalAngle * 2f, 2f);

        // Draw inner "full weight" arc
        Handles.color = new Color(1f, 1f, 0f, 0.15f); // Yellow
        Handles.DrawSolidArc(_Origin, _Up, Quaternion.AngleAxis(-_ViewRange, _Up) * _Forward, _ViewRange * 2f, 1.5f);
#endif

        // Draw direction to target
        Vector3 toTarget = (aimTarget.position - _Origin).normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_Origin, aimTarget.position);

        float signedAngle = Vector3.SignedAngle(_Forward, toTarget, _Up);
        float absAngle = Mathf.Abs(signedAngle);
        float fadeAngle = absAngle - _ViewRange;
        float t = 1f - Mathf.Clamp01(fadeAngle / (_TotalAngle - _ViewRange));
        float currentWeight = m_FrustumWeightCurve.Evaluate(t) * m_AffectedRig.weight;

#if UNITY_EDITOR
        Handles.color = Color.white;
        Handles.Label(_Origin + Vector3.up * 2f, 
            $"{m_LayerName}\nAngle: {signedAngle:F1}°\nWeight: {currentWeight:F2}");
#endif
    }

    #endregion Gizmos
  
}
