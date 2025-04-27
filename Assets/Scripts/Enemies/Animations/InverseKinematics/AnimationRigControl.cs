using System.Collections;
using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Base;
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
    
    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        GameValidator.NotNull(this.m_RigBuilder, nameof(m_RigBuilder));
        GameValidator.NotNull(this.m_ReferenceCharacterTransform, nameof(m_ReferenceCharacterTransform));
        GameValidator.NotNull(this.m_TargetTransform, nameof(m_TargetTransform));
    }
    
    private void Update()
    {
        if (this.IsPaused) return;

        if (!this.m_IsBuilt)
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

        this.m_IsBuilt = true;
        Debug.Log("Rig has been built");
    }
    
    #endregion Methods

    #region - - - - - - Gizmos - - - - - -

    private void OnDrawGizmosSelected()
    {
        if (this.m_ReferenceCharacterTransform == null) return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(this.m_ReferenceCharacterTransform.position, this.m_MaxActiveRigRadius);
    }

    #endregion Gizmos
  
}
