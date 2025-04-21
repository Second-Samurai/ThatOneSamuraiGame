using System.Collections.Generic;
using UnityEngine;

public class BehaviourScriptDisabler : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private List<MonoBehaviour> m_ScriptsToDisable;
    [SerializeField] private List<GameObject> m_ObjectsToDisable;

    #endregion Fields
  
    #region - - - - - - Methods - - - - - -

    public void DisableObjectsAndScripts()
    {
        for (int i = 0; i < this.m_ScriptsToDisable.Count; i++)
            this.m_ScriptsToDisable[i].enabled = false;
        
        for (int i = 0; i < this.m_ObjectsToDisable.Count; i++)
            this.m_ObjectsToDisable[i].SetActive(false);
    }

    #endregion Methods
  
}