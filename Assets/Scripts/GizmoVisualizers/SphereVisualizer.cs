using System;
using UnityEngine;

public class SphereVisualizer : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private bool m_IsWired;
    [SerializeField] private bool m_IsDrawnOnSelected;
    [SerializeField] private float m_SphereRadius;
    [SerializeField] private Color m_SphereColor;

    #endregion Fields

    #region - - - - - - Gizmos - - - - - -

    private void OnDrawGizmos()
    {
        if (this.m_IsDrawnOnSelected) return;
        
        this.DrawSphere();
    }

    private void OnDrawGizmosSelected()
    {
        if (!this.m_IsDrawnOnSelected) return;

        this.DrawSphere();
    }

    private void DrawSphere()
    {
        Gizmos.color = this.m_SphereColor;
        
        if (this.m_IsWired)
            Gizmos.DrawWireSphere(this.transform.position, this.m_SphereRadius);
        else
            Gizmos.DrawSphere(this.transform.position, this.m_SphereRadius);
    }

    #endregion Gizmos

}
