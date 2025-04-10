using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public interface IIKWeightControl
{

    #region - - - - - - Methods' - - - - - -

    void AnimateMasterWeight(float delta);

    #endregion Methods'

}

public class IKCharacterControl : MonoBehaviour, IIKWeightControl
{

    #region - - - - - - Fields - - - - - -

    public List<Rig> m_RigLayers;
    [Range(0f, 1f)]
    public float m_MasterWeightControl;
    public AnimationCurve m_InterpolationCurve;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    public void AnimateMasterWeight(float delta)
    {
        foreach (Rig _Rig in this.m_RigLayers)
        {
            _Rig.weight = this.m_InterpolationCurve.Evaluate(delta);
        }
    }

    #endregion Methods

}
