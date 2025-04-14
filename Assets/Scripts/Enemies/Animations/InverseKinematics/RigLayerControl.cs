using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigLayerControl : MonoBehaviour, IRigLayerControl
{

    #region - - - - - - Fields - - - - - -

    [Header("Rig Controls")]
    [SerializeField] private bool m_IsActive;
    [Range(0f, 180f)]
    [SerializeField] private float m_MaxWeightAffectedAngle;
    [SerializeField] private AnimationCurve m_FrustumWeightCurve;
    [SerializeField] private Rig m_AffectedRig;

    [Header("Transform Targets")]
    [SerializeField] private Transform m_TargetTransform;
    [SerializeField] private Transform m_CharacterTransform;
    private bool m_IsAnimatingWeights;

    [Header("Debug")]
    [SerializeField] private bool m_CanDrawGizmos;

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public bool IsActive
    {
        get => this.m_IsActive;
        set => this.m_IsActive = value;
    }

    #endregion Properties

    #region - - - - - - Unity Methods - - - - - -

    private void Update()
    {
        if (!this.m_IsActive || this.m_IsAnimatingWeights) return;
        
        Vector3 _DirectionToTarget = (this.m_TargetTransform.position - this.m_CharacterTransform.position).normalized;
        _DirectionToTarget.y = 0f;
        Vector3 _Forward = this.m_CharacterTransform.forward;
        Vector3 _Up = this.m_CharacterTransform.up;

        float _SignedAngle = Vector3.SignedAngle(_Forward, _DirectionToTarget, _Up);
        float _AbsAngle = Mathf.Abs(_SignedAngle);
        // float _FullWeightAngle = this.m_MaxWeightAffectedAngle - this.m_ViewPaddingAngle;

        if (_AbsAngle > this.m_MaxWeightAffectedAngle && this.m_AffectedRig.weight > 0)
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.AnimateRigWeights(-1));
            // _T = 1f - Mathf.Clamp01((_AbsAngle - _FullWeightAngle) / _FullWeightAngle);
        }
        else if (_AbsAngle <= this.m_MaxWeightAffectedAngle && this.m_AffectedRig.weight < 1)
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.AnimateRigWeights(1));
            // _T = 1;
        }
        
        // this.m_AffectedRig.weight = this.m_FrustumWeightCurve.Evaluate(_T);
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public void SetDefaultRigValues()
    {
        // On start the weight should be set to default as Unity animator will default its influence to 1.0f
        this.m_AffectedRig.weight = 0f;
    }

    private IEnumerator AnimateRigWeights(float direction)
    {
        this.m_IsAnimatingWeights = true;
        
        float _TotalTime = 0;
        while (_TotalTime < 0.5f)
        {
            float _T = Mathf.Clamp01(_TotalTime / 0.5f) * direction;
            this.m_AffectedRig.weight = this.m_FrustumWeightCurve.Evaluate(_T);
            _TotalTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure weight does not contain any value after its decimal point
        this.m_AffectedRig.weight = Mathf.Approximately(direction, 1f) ? 1 : 0;
        this.m_IsAnimatingWeights = false;
    }

    #endregion Methods

    #region - - - - - - Gizmos - - - - - -

    private void OnDrawGizmosSelected()
    {
        if (!this.m_CanDrawGizmos || this.m_TargetTransform == null || this.m_AffectedRig == null) return;
        
        Vector3 _Origin = this.m_CharacterTransform.position;
        Vector3 _Forward = this.m_CharacterTransform.forward;
        Vector3 _Up = this.m_CharacterTransform.up;

        float _TotalAngle = this.m_MaxWeightAffectedAngle;
        float _ViewRange = this.m_MaxWeightAffectedAngle;

#if UNITY_EDITOR
        // Draw total view arc
        // Handles.color = new Color(0f, 1f, 0f, 0.15f); // Green
        // Handles.DrawSolidArc(_Origin, _Up, Quaternion.AngleAxis(-_TotalAngle, _Up) * _Forward, _TotalAngle * 2f, 2f);

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
        float _CurrentWeight = this.m_FrustumWeightCurve.Evaluate(_T);

#if UNITY_EDITOR
        // Display rig information debug panel
        Handles.color = Color.magenta;
        Handles.Label(_Origin + Vector3.up * 2f, 
            $"{this.gameObject.name}\n" +
            $"Angle: {_SignedAngle:F1}°\n" +
            $"Weight: {_CurrentWeight:F2}\n" +
            $"SignedAngle: {_SignedAngle:F2}\n" +
            $"CurveTime: {_T}\n" +
            $"FadeArc: {(_AbsAngle - _ViewRange):F2} / FullFadeArc: {(_TotalAngle - _ViewRange):F2}");
#endif
    }

    #endregion Gizmos

}
