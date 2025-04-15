using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigLayerToggle : MonoBehaviour, IRigLayerControl
{

    #region - - - - - - Fields - - - - - -

    [Header("Rig Controls")]
    [SerializeField] private bool m_IsActive;
    [SerializeField] private AnimationCurve m_TransitionWeightCurve;
    [SerializeField] private Rig m_AffectedRig;

    private bool m_IsAnimating;

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public bool IsActive
    {
        get => this.m_IsActive;
        set => this.m_IsActive = value;
    }

    public AnimationCurve TransitionWeightCurve
        => this.m_TransitionWeightCurve;

    #endregion Properties

    #region - - - - - - Unity Methods - - - - - -

    private void Start() 
        => GameValidator.NotNull(this.m_AffectedRig, nameof(m_AffectedRig));

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public void AnimateEnableRigWeight(float speedMultiplier, Action onCompletion = null)
    {
        if (!this.m_IsActive) return;
        
        if (this.m_IsAnimating)
            this.ResetAnimationCoroutines(onCompletion);
        
        this.StartCoroutine(this.AnimateRigWeights(0f, 1f, speedMultiplier, onCompletion));
    }

    public void AnimateDisableRigWeight(float speedMultiplier, Action onCompletion = null)
    {
        if (!this.m_IsActive) return;
        
        if (this.m_IsAnimating)
            this.ResetAnimationCoroutines(onCompletion);
        
        this.StartCoroutine(this.AnimateRigWeights(1f, 0f, speedMultiplier, onCompletion));
    }

    public void SetDefaultRigValues()
    {
        // On start the weight should be set to default as Unity animator will default its influence to 1.0f
        this.m_AffectedRig.weight = 0f;
    }
    
    private IEnumerator AnimateRigWeights(float startWeight, float endWeight, float speedMultiplier, Action onCompletion = null)
    {
        this.m_IsAnimating = true;
        
        float _TotalTime = 0;
        while (_TotalTime < 0.5f)
        {
            float _T = Mathf.Clamp01(_TotalTime / 0.5f);
            this.m_AffectedRig.weight = Mathf.Lerp(
                startWeight,
                endWeight,
                this.m_TransitionWeightCurve.Evaluate(_T));
            _TotalTime += Time.deltaTime * speedMultiplier;
            yield return null;
        }
        
        // Ensure weight does not contain any value after its decimal point
        this.m_AffectedRig.weight = endWeight;
        onCompletion?.Invoke();

        this.m_IsAnimating = false;
    }

    private void ResetAnimationCoroutines(Action onCompletion = null)
    {
        this.StopAllCoroutines();
        this.m_IsAnimating = false;
        onCompletion?.Invoke();
    }

    #endregion Methods

}
