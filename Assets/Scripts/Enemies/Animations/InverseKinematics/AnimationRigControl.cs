using System;
using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Base;
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
  
}
