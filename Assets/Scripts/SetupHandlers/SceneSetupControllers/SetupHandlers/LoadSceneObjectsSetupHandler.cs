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

    [SerializeField, RequiredField] private ObjectGroupEnablerSystem m_ObjectEnabler;
    
    private ISetupHandler m_NextHandler;

    #endregion Fields
  
    #region - - - - - - Methods - - - - - -

    public void SetNext(ISetupHandler setupHandler) 
        => this.m_NextHandler = setupHandler;

    public void Handle(SceneSetupContext setupContext)
    {
        this.m_ObjectEnabler.EnableObjects();
        GameLogger.Log("Scene objects are loaded");
        
        this.m_NextHandler?.Handle(setupContext);
    }

    #endregion Methods
  
}
