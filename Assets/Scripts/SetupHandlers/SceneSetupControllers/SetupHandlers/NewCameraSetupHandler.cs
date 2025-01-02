using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers;
using UnityEngine;

public class NewCameraSetupHandler : MonoBehaviour, ISetupHandler
{

    #region - - - - - - Fields - - - - - -
    
    [SerializeField] private LockOnObserver m_LockOnObserver;
    [SerializeField] private GameObject m_PlayerObject;

    private ISetupHandler m_NextHandler;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    void ISetupHandler.SetNext(ISetupHandler setupHandler)
        => this.m_NextHandler = setupHandler;

    void ISetupHandler.Handle(SceneSetupContext setupContext)
    {
        // Load data      
        GameSettings _GameSettings = GameManager.instance.gameSettings;
        ICameraController _CameraController = setupContext.CameraControlObject.GetComponent<ICameraController>();
        
        // Spawn main camera
        Vector3 _MainCameraPos = _GameSettings.mainCamera.transform.position;
        SceneManager.Instance.MainCamera = Instantiate(
            _GameSettings.mainCamera, 
            _MainCameraPos, 
            Quaternion.identity
        ).GetComponent<UnityEngine.Camera>();

        // Setup Camera Systems
        this.SetupLockOnCamera(_CameraController);
        SceneManager.Instance.CameraController = _CameraController;
        
        print("[LOG]: Completed Scene Camera setup.");
        this.m_NextHandler?.Handle(setupContext);
    }
    
    private void SetupLockOnCamera(ICameraController cameraController)
    {
        if (!GameValidator.NotNull(this.m_LockOnObserver, nameof(this.m_LockOnObserver))) return;
            
        ILockOnCamera _LockOnCamera = cameraController.GetCamera(SceneCameras.LockOn).GetComponent<ILockOnCamera>();
        
        this.m_LockOnObserver.OnLockOnEnable
            .AddListener(() => cameraController.SelectCamera(SceneCameras.LockOn));
        this.m_LockOnObserver.OnNewLockOnTarget.AddListener(_LockOnCamera.SetLockOnTarget);
        this.m_LockOnObserver.OnLockOnDisable
            .AddListener(() => cameraController.SelectCamera(SceneCameras.FollowPlayer));

        _LockOnCamera.SetFollowedTransform(this.m_PlayerObject.transform);
    }

    #endregion Methods

}