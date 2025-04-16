using ThatOneSamuraiGame.Scripts.Base;
using UnityEngine;

/// <summary>
/// Responsible for interpolating transform between two positions
/// </summary>
public class TransformReferenceInterpolator : PausableMonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private Transform m_AffectedTransform;
    [SerializeField] private Transform m_FirstTargetSourceTransform;
    [SerializeField] private Transform m_SecondTargetSourceTransform;

    [SerializeField] private float m_TransitionSpeed;
    [SerializeField] private AnimationCurve m_TransitionCurve;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    #endregion Methods
  
}
