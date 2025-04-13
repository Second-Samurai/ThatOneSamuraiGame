using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Base;

public class AnimationRigControl : PausableMonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public List<RigLayerControl> RigWeightLayers;
    // public Transform AimTarget;
    // [Tooltip("Motion is controlled via 'Root Motion'. Provide the character's transform.")]
    // public Transform CharacterRootTransform;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Awake()
    {
        foreach (IRigLayerControl _RigLayerControl in this.RigWeightLayers)
            _RigLayerControl.SetDefaultRigValues();
        this.DeActivateAllLayers();
    }

    //
    // private void Update()
    // {
    //     if (this.IsPaused) return;
    //
    //     for(int i = 0; i < this.RigWeightLayers.Count; i++)
    //         this.RigWeightLayers[i].UpdateControl();
    // }

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

    // private void OnDrawGizmos()
    // {
    //     if (this.RigWeightLayers == null) return;
    //
    //     foreach (var _RigLayer in this.RigWeightLayers)
    //     {
    //         if (_RigLayer is RigLayerMasterControl _MasterControl)
    //             _MasterControl.DrawDebugGizmos(this.CharacterRootTransform, this.AimTarget);
    //     }
    // }

    #endregion Gizmos
  
}


// // TODO: Move to a seperate file
// [Serializable]
// public class RigLayerMasterControl : MonoBehaviour, IRigLayerControl
// {
//
//     #region - - - - - - Fields - - - - - -
//
//     public string m_LayerName;
//     public bool m_IsActive;
//     [Range(0f, 180f)]
//     public float m_MaxWeightAffectedAngle;
//     public float m_ViewPaddingAngle;
//     public AnimationCurve m_FrustumWeightCurve;
//     public Rig m_AffectedRig;
//
//     private Transform m_TargetTransform;
//     private Transform m_CharacterTransform;
//     private bool m_IsAnimatingWeights;
//
//     [SerializeField] private bool m_CanDrawGizmos;
//
//     #endregion Fields
//
//     #region - - - - - - Properties - - - - - -
//
//     public bool IsActive
//     {
//         get => this.m_IsActive;
//         set => this.m_IsActive = value;
//     }
//
//     #endregion Properties
//   
//     #region - - - - - - Methods - - - - - -
//
//     public void OnStart(Transform characterTransform, Transform aimTarget)
//     {
//         this.m_TargetTransform = aimTarget;
//         this.m_CharacterTransform = characterTransform;
//     }
//
//     public void UpdateControl()
//     {
//         if (!this.m_IsActive) return;
//         
//         Vector3 _DirectionToTarget = (this.m_TargetTransform.position - this.m_CharacterTransform.position).normalized;
//         _DirectionToTarget.y = 0f;
//         Vector3 _Forward = this.m_CharacterTransform.forward;
//         Vector3 _Up = this.m_CharacterTransform.up;
//
//         float _SignedAngle = Vector3.SignedAngle(_Forward, _DirectionToTarget, _Up);
//         float _AbsAngle = Mathf.Abs(_SignedAngle);
//         float _FullWeightAngle = this.m_MaxWeightAffectedAngle - this.m_ViewPaddingAngle;
//         float _T = 0;
//
//         if (_AbsAngle > _FullWeightAngle && _AbsAngle <= this.m_MaxWeightAffectedAngle)
//         {
//             // _T = 1f - Mathf.Clamp01((_AbsAngle - _FullWeightAngle) / _FullWeightAngle);
//         }
//         else if (_AbsAngle <= _FullWeightAngle)
//         {
//             
//             // _T = 1;
//         }
//         
//         // this.m_AffectedRig.weight = this.m_FrustumWeightCurve.Evaluate(_T);
//     }
//
//     public void SetDefaultRigValues()
//     {
//         // On start the weight should be set to default as Unity animator will default its influence to 1.0f
//         this.m_AffectedRig.weight = 0f;
//     }
//
//     private IEnumerator AnimateRigWeights(float direction)
//     {
//         this.m_IsAnimatingWeights = true;
//         
//         float _TotalTime = 0;
//         while (_TotalTime < 0.5f)
//         {
//             float _T = Mathf.Clamp01(_TotalTime / 0.5f) * direction;
//             this.m_AffectedRig.weight = this.m_FrustumWeightCurve.Evaluate(_T);
//             _TotalTime += Time.deltaTime;
//             yield return null;
//         }
//     }
//
//     #endregion Methods
//
//     #region - - - - - - Gizmos - - - - - -
//
//     public void DrawDebugGizmos(Transform characterTransform, Transform aimTarget)
//     {
//         if (!this.m_CanDrawGizmos || aimTarget == null || this.m_AffectedRig == null) return;
//         
//         Vector3 _Origin = characterTransform.position;
//         Vector3 _Forward = characterTransform.forward;
//         Vector3 _Up = characterTransform.up;
//
//         float _TotalAngle = this.m_MaxWeightAffectedAngle;
//         float _ViewRange = this.m_MaxWeightAffectedAngle - this.m_ViewPaddingAngle;
//
// #if UNITY_EDITOR
//         // Draw total view arc
//         Handles.color = new Color(0f, 1f, 0f, 0.15f); // Green
//         Handles.DrawSolidArc(_Origin, _Up, Quaternion.AngleAxis(-_TotalAngle, _Up) * _Forward, _TotalAngle * 2f, 2f);
//
//         // Draw inner "full weight" arc
//         Handles.color = new Color(1f, 1f, 0f, 0.25f); // Yellow
//         Handles.DrawSolidArc(_Origin, _Up, Quaternion.AngleAxis(-_ViewRange, _Up) * _Forward, _ViewRange * 2f, 1.5f);
// #endif
//
//         // Draw direction to target
//         Vector3 _ToTarget = (aimTarget.position - _Origin).normalized;
//         _ToTarget.y = 0f; // strictly in the x-y plane
//         Gizmos.color = Color.red;
//         Gizmos.DrawLine(_Origin, aimTarget.position);
//
//         float _SignedAngle = Vector3.SignedAngle(_Forward, _ToTarget, _Up);
//         float _AbsAngle = Mathf.Abs(_SignedAngle);
//         float _T = 0f;
//         
//         if (_AbsAngle > _ViewRange && _AbsAngle <= this.m_MaxWeightAffectedAngle)
//             _T = 1f - Mathf.Clamp01((_AbsAngle - _ViewRange) / _ViewRange);
//         else if (_AbsAngle <= _ViewRange)
//             _T = 1;
//         float _CurrentWeight = this.m_FrustumWeightCurve.Evaluate(_T);
//
// #if UNITY_EDITOR
//         // Display rig information debug panel
//         Handles.color = Color.magenta;
//         Handles.Label(_Origin + Vector3.up * 2f, 
//             $"{this.m_LayerName}\n" +
//             $"Angle: {_SignedAngle:F1}°\n" +
//             $"Weight: {_CurrentWeight:F2}\n" +
//             $"SignedAngle: {_SignedAngle:F2}\n" +
//             $"CurveTime: {_T}\n" +
//             $"FadeArc: {(_AbsAngle - _ViewRange):F2} / FullFadeArc: {(_TotalAngle - _ViewRange):F2}");
// #endif
//     }
//
//     #endregion Gizmos
//   
// }
