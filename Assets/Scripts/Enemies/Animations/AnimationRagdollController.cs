using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationRagdollController : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    private List<Rigidbody> m_RagdollRigidbodies;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_RagdollRigidbodies = this.GetComponentsInChildren<Rigidbody>().ToList();
        this.SwitchToKinematic();
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    private void SwitchToKinematic()
    {
        for (int i = 0; i < this.m_RagdollRigidbodies.Count; i++)
            this.m_RagdollRigidbodies[i].isKinematic = true;
    }

    private void SwitchToRagdoll()
    {
        for (int i = 0; i < this.m_RagdollRigidbodies.Count; i++)
            this.m_RagdollRigidbodies[i].isKinematic = false;
    }

    #endregion Methods
  
}
