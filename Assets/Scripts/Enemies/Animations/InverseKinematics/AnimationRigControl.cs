using System.Collections;
using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Base;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public interface IAnimationRigBuilder
{

    #region - - - - - - Methods - - - - - -

    void BuildRig();

    #endregion Methods

}

public class AnimationRigControl : PausableMonoBehaviour, IAnimationRigBuilder
{

    #region - - - - - - Fields - - - - - -

    [SerializeField, RequiredField] private List<RigLayerToggle> RigWeightLayers;
    [SerializeField, RequiredField] private RigBuilder m_RigBuilder;

    [SerializeField, RequiredField] private Transform m_ReferenceCharacterTransform;
    [SerializeField, RequiredField] private Transform m_TargetTransform;
    [SerializeField, RequiredField] private float m_MaxActiveRigRadius;

    private bool m_IsBuilt;
    private bool m_IsDelayed;
    private float m_RigActivationDelayTime = 1f;
    
    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        GameValidator.NotNull(this.m_RigBuilder, nameof(m_RigBuilder));
        GameValidator.NotNull(this.m_ReferenceCharacterTransform, nameof(m_ReferenceCharacterTransform));
        GameValidator.NotNull(this.m_TargetTransform, nameof(m_TargetTransform));

        this.StartCoroutine(this.DelayRigBuildingChecks());
    }
    
    private void Update()
    {
        if (this.IsPaused) return;

        if (!this.m_IsBuilt && !this.m_IsDelayed)
        {
            float _Distance = (this.m_ReferenceCharacterTransform.position - this.m_TargetTransform.position).sqrMagnitude;
            if (_Distance < this.m_MaxActiveRigRadius * this.m_MaxActiveRigRadius)
                this.StartCoroutine(this.GraduallyBuildRig());
        }
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public void ActivateAllLayers()
    {
        foreach(IRigLayerControl _RigLayerControl in this.RigWeightLayers)
            _RigLayerControl.IsActive = true;
    }

    public void DeActivateAllLayers()
    {
        foreach (IRigLayerControl _RigLayerControl in this.RigWeightLayers)
            _RigLayerControl.DisableLayer();
    }

    public void BuildRig()
    {
        this.m_RigBuilder.Build();
        this.m_RigBuilder.enabled = true;
    }

    private IEnumerator GraduallyBuildRig()
    {
        this.m_IsBuilt = true;
        
        foreach (var _Layer in m_RigBuilder.layers)
        {
            if (_Layer.rig != null && _Layer.rig.enabled)
            {
                _Layer.rig.gameObject.SetActive(false);
                yield return null;
            }
        }
        
        this.m_RigBuilder.Build();
        this.m_RigBuilder.enabled = true;
        yield return null;
        
        foreach (var _Layer in m_RigBuilder.layers)
        {
            if (_Layer.rig != null)
            {
                _Layer.rig.gameObject.SetActive(true);
                yield return null;
            }
        }
    }

    private IEnumerator DelayRigBuildingChecks()
    {
        yield return new WaitForSeconds(this.m_RigActivationDelayTime);
    }
    
    #endregion Methods

    #region - - - - - - Gizmos - - - - - -

    private void OnDrawGizmosSelected()
    {
        if (this.m_ReferenceCharacterTransform == null) return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(this.m_ReferenceCharacterTransform.position, this.m_MaxActiveRigRadius);
        
        if (this.m_TargetTransform == null) return;
        
        Handles.color = Color.magenta;
        Handles.Label(this.m_TargetTransform.position + Vector3.up * 2f,
            $"{this.gameObject.name}\n" +
            $"Calculated Distance: {(this.m_ReferenceCharacterTransform.position - this.m_TargetTransform.position).sqrMagnitude:F1}units\n" +
            $"Compared Distance: {(this.m_MaxActiveRigRadius*this.m_MaxActiveRigRadius)}units");
    }

    #endregion Gizmos
  
}
