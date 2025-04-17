using System.Collections;
using ThatOneSamuraiGame.Scripts.Base;
using UnityEngine;

/// <summary>
/// Responsible for interpolating transform between two positions
/// </summary>
public class TransformReferenceInterpolator : PausableMonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [Header("Depencencies")]
    [SerializeField, RequiredField] private ArcherAnimationReciever m_AnimationReceiver;
    [SerializeField, RequiredField] private Transform m_AffectedTransform;
    
    [Header("Source Transforms")]
    [SerializeField] private Transform m_FirstTargetSourceTransform;
    [SerializeField] private Transform m_SecondTargetSourceTransform;

    [Header("Transition Fields")]
    [SerializeField] private AnimationCurve m_TransitionCurve;
    [SerializeField] private float m_TransitionTimeLength;

    #endregion Fields

    #region - - - - - - Unity Methods' - - - - - -

    private void Start()
    {
        // TODO: Move animation binding to seperate area.
        this.m_AnimationReceiver.OnBowEquip.AddListener(this.TransitionToA);
        this.m_AnimationReceiver.OnBowDisarm.AddListener(this.TransitionToB);
    }

    #endregion Unity Methods'
  
    #region - - - - - - Methods - - - - - -

    public void TransitionToA()
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.InterpolateBetweenTransforms(
            this.m_FirstTargetSourceTransform,
            this.m_SecondTargetSourceTransform));
    }

    public void TransitionToB()
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.InterpolateBetweenTransforms(
            this.m_SecondTargetSourceTransform,
            this.m_FirstTargetSourceTransform));
    }

    private IEnumerator InterpolateBetweenTransforms(Transform startingTransform, Transform endingTransform)
    {
        float _TotalTime = 0f;
        while (_TotalTime < this.m_TransitionTimeLength)
        {
            float _Time = Mathf.Clamp01(_TotalTime / this.m_TransitionTimeLength);
            float _CurveTime = this.m_TransitionCurve.Evaluate(_Time);

            Vector3 _Position = Vector3.MoveTowards(startingTransform.position, endingTransform.position, _CurveTime);
            Quaternion _Rotation = Quaternion.Lerp(startingTransform.rotation, endingTransform.rotation, _CurveTime);

            this.m_AffectedTransform.position = _Position;
            this.m_AffectedTransform.rotation = _Rotation;

            _TotalTime += Time.deltaTime;
            yield return null;
        }

    }

    #endregion Methods
  
}
