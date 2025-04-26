using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigPrewarmer : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    private RigBuilder m_RigBuilder;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_RigBuilder = this.GetComponent<RigBuilder>();
        this.m_RigBuilder.Build();
    }

    #endregion Unity Methods
    
  
}
