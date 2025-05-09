using UnityEngine;

public class ArcherArmRigEventHandler : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    // Required Dependencies
    [SerializeField, RequiredField] private ArcherAnimationReciever m_AnimationReceiver;
    [SerializeField, RequiredField] private RigLayerToggle m_RigLayerToggle;
    private IFieldOfViewOcclusionState m_FOVOcclusionState;
    
    [SerializeField] private float m_AnimateSpeedMultiplier;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_FOVOcclusionState = this.GetComponent<IFieldOfViewOcclusionState>();
        this.m_RigLayerToggle = this.GetComponent<RigLayerToggle>();
        
        GameValidator.NotNull(this.m_AnimationReceiver, nameof(m_AnimationReceiver));
        GameValidator.NotNull(this.m_RigLayerToggle, nameof(m_RigLayerToggle));
        GameValidator.NotNull(this.m_FOVOcclusionState, nameof(m_FOVOcclusionState));

        this.m_AnimationReceiver.OnDrawBow.AddListener(() =>
        {
            if (this.m_FOVOcclusionState.IsWithinFieldOfView)
                this.m_RigLayerToggle.AnimateEnableRigWeight(
                    this.m_AnimateSpeedMultiplier, 
                    () => this.m_RigLayerToggle.IsActive = true);
        });
        this.m_AnimationReceiver.OnBowRelease.AddListener(() =>
        {
            if (this.m_FOVOcclusionState.IsWithinFieldOfView)
                this.m_RigLayerToggle.AnimateDisableRigWeight(
                    this.m_AnimateSpeedMultiplier,
                    () => this.m_RigLayerToggle.IsActive = false);
        });
    }

    #endregion Unity Methods
  
}
