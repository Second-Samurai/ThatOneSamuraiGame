using System.Collections;
using System.Collections.Generic;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers;
using UnityEngine;

/// <summary>
/// Responsible for handling loading the objects within the scene.
/// </summary>
/// <remarks>Avoid setting this last. Unexpected results will occur if required objects are not active in hierarchy</remarks>
public class LoadSceneObjectsSetupHandler : MonoBehaviour, ISetupHandler
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private List<GameObject> m_ObjectsToEnableInSequence;
    
    private ISetupHandler m_NextHandler;

    #endregion Fields
  
    #region - - - - - - Methods - - - - - -

    public void SetNext(ISetupHandler setupHandler) 
        => this.m_NextHandler = setupHandler;

    public void Handle(SceneSetupContext setupContext)
    {
        this.StartCoroutine(this.GraduallyLoadObjects());
        GameLogger.Log("Scene objects are loaded");
        
        this.m_NextHandler?.Handle(setupContext);
    }

    private IEnumerator GraduallyLoadObjects()
    {
        for (int i = 0; i < this.m_ObjectsToEnableInSequence.Count; i++)
        {
            if (this.m_ObjectsToEnableInSequence[i].activeInHierarchy)
                GameLogger.LogWarning($"'{this.m_ObjectsToEnableInSequence[i].name}' should be disabled before running.");
            
            this.m_ObjectsToEnableInSequence[i].SetActive(true);
            yield return null;
        }
    }

    #endregion Methods
  
}
