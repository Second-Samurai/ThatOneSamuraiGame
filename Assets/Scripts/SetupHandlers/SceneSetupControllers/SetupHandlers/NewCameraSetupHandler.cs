using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers;
using UnityEngine;

public class NewCameraSetupHandler : MonoBehaviour, ISetupHandler
{

    #region - - - - - - Fields - - - - - -

    private ISetupHandler m_NextHandler;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    void ISetupHandler.SetNext(ISetupHandler setupHandler)
        => this.m_NextHandler = setupHandler;

    void ISetupHandler.Handle()
    {
        GameSettings _GameSettings = GameManager.instance.gameSettings;
        
        Vector3 _MainCameraPos = _GameSettings.mainCamera.transform.position;
        SceneManager.Instance.MainCamera = Instantiate(
            _GameSettings.mainCamera, 
            _MainCameraPos, 
            Quaternion.identity
        ).GetComponent<UnityEngine.Camera>();
        
        print("[LOG]: Completed Scene Camera setup.");
        this.m_NextHandler?.Handle();
    }

    #endregion Methods

}