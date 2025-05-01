using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGroupEnablerSystem : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private List<GameObject> m_ObjectGroups;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    public void EnableObjects()
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.GraduallyEnableObjects());
    }

    public void DisableObjects()
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.GraduallyDisableObjects());
    }

    private IEnumerator GraduallyEnableObjects()
    {
        for (int i = 0; i < this.m_ObjectGroups.Count; i++)
        {
            this.m_ObjectGroups[i].SetActive(true);
            yield return null;
        }
    }

    private IEnumerator GraduallyDisableObjects()
    {
        for (int i = 0; i < this.m_ObjectGroups.Count; i++)
        {
            this.m_ObjectGroups[i].SetActive(false);
            yield return null;
        }
    }

    #endregion Methods

}
