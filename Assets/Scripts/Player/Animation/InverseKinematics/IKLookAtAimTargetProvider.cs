using UnityEngine;

public class IKLookAtAimTargetProvider : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField, RequiredField] private Transform m_TargetAimTransform;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    public Transform GetAimTarget()
        => this.m_TargetAimTransform;

    #endregion Methods

}
