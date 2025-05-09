using UnityEngine;

public class PreferredLockingTransformProvider : MonoBehaviour, IPreferredLockingTransformProvider
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private Transform m_PreferredLockingTransform;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    public Transform GetPreferredTransform()
        => this.m_PreferredLockingTransform ?? this.transform;

    #endregion Methods
  
}
