using UnityEngine;

public class ArcherArmRigEventHandler : MonoBehaviour, IInitialize
{

    #region - - - - - - Fields - - - - - -

    // Required Dependencies
    [SerializeField, RequiredField] private ArcherAnimationReciever m_AnimationReceiver;
    [SerializeField, RequiredField] private RigLayerToggle m_RigLayerToggle;
    
    [SerializeField] private float m_AnimateSpeedMultiplier;

    #endregion Fields
  
    #region - - - - - - Initializers - - - - - -

    public void Initialize()
    {
        this.m_RigLayerToggle = this.GetComponent<RigLayerToggle>();
        
        GameValidator.NotNull(this.m_RigLayerToggle, nameof(m_RigLayerToggle));
        GameValidator.NotNull(this.m_AnimationReceiver, nameof(m_AnimationReceiver));
        
        // this.m_AnimationReceiver.OnDrawBow.AddListener(() => 
        //     this.m_RigLayerToggle.AnimateEnableRigWeight(this.m_AnimateSpeedMultiplier));
        // this.m_AnimationReceiver.OnBowRelease.AddListener(() => 
        //     this.m_RigLayerToggle.AnimateDisableRigWeight(this.m_AnimateSpeedMultiplier));
    }

    #endregion Initializers    
  
}
