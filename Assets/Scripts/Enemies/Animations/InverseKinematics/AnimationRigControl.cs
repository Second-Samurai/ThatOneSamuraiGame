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
    
    #endregion Fields
  
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
    
    #endregion Methods
  
}
