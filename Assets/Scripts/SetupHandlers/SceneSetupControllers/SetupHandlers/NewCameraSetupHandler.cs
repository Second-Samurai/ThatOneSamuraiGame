using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers;
using UnityEngine;

public class NewCameraSetupHandler : MonoBehaviour, ISetupHandler
{

    #region - - - - - - Fields - - - - - -
    
    [SerializeField] private GameObject m_CameraControlObject;
    [SerializeField] private LockOnObserver m_LockOnObserver;
    [SerializeField] private GameObject m_PlayerObject;

    private ISetupHandler m_NextHandler;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    void ISetupHandler.SetNext(ISetupHandler setupHandler)
        => this.m_NextHandler = setupHandler;

    void ISetupHandler.Handle(SceneSetupContext setupContext)
    {
        GameSettings _GameSettings = GameManager.instance.gameSettings;
        
        Vector3 _MainCameraPos = _GameSettings.mainCamera.transform.position;
        SceneManager.Instance.MainCamera = Instantiate(
            _GameSettings.mainCamera, 
            _MainCameraPos, 
            Quaternion.identity
        ).GetComponent<UnityEngine.Camera>();

        this.SetupLockOnCamera();
        
        print("[LOG]: Completed Scene Camera setup.");
        this.m_NextHandler?.Handle(setupContext);
    }
    
    private void SetupLockOnCamera()
    {
        if (!GameValidator.NotNull(this.m_LockOnObserver, nameof(this.m_LockOnObserver))) return;
            
        CameraController _CameraController = this.m_CameraControlObject.GetComponent<CameraController>();
        ILockOnCamera _LockOnCamera = _CameraController.GetCamera(SceneCameras.LockOn).GetComponent<ILockOnCamera>();

        Debug.Log(this.m_LockOnObserver);
        
        this.m_LockOnObserver.OnLockOnEnable
            .AddListener(() => _CameraController.SelectCamera(SceneCameras.LockOn));
        this.m_LockOnObserver.OnNewLockOnTarget.AddListener(_LockOnCamera.SetLockOnTarget);
        this.m_LockOnObserver.OnLockOnDisable
            .AddListener(() => _CameraController.SelectCamera(SceneCameras.FollowPlayer));

        _LockOnCamera.SetFollowedTransform(this.m_PlayerObject.transform);
    }

    #endregion Methods

}