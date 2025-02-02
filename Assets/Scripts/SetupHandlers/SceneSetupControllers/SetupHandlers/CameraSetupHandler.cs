using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers;
using UnityEngine;

public class CameraSetupHandler : MonoBehaviour, ISetupHandler
{

    #region - - - - - - Fields - - - - - -
    
    [RequiredField]
    [SerializeField]
    private GameObject m_PlayerObject;

    private ISetupHandler m_NextHandler;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    void ISetupHandler.SetNext(ISetupHandler setupHandler)
        => this.m_NextHandler = setupHandler;

    void ISetupHandler.Handle(SceneSetupContext setupContext)
    {
        // Load data      
        GameSettings _GameSettings = GameManager.instance.gameSettings;
        
        // Spawn main camera
        Vector3 _MainCameraPos = _GameSettings.mainCamera.transform.position;
        SceneManager.Instance.MainCamera = Instantiate(
            _GameSettings.mainCamera, 
            _MainCameraPos, 
            Quaternion.identity
        ).GetComponent<UnityEngine.Camera>();

        // Setup Camera Systems
        this.SetupLockOnCamera(setupContext.CameraController, setupContext.LockOnObserver);
        
        print("[LOG]: Completed Scene Camera setup.");
        this.m_NextHandler?.Handle(setupContext);
    }
    
    private void SetupLockOnCamera(ICameraController cameraController, ILockOnObserver lockOnObserver)
    {
        if (!GameValidator.NotNull(lockOnObserver, nameof(lockOnObserver))) return;
            
        ILockOnCamera _LockOnCamera = cameraController.GetCamera(SceneCameras.LockOn).GetComponent<ILockOnCamera>();
        lockOnObserver.OnLockOnEnable .AddListener(() => cameraController.SelectCamera(SceneCameras.LockOn));
        lockOnObserver.OnNewLockOnTarget.AddListener(_LockOnCamera.SetLockOnTarget);
        lockOnObserver.OnLockOnDisable.AddListener(() => cameraController.SelectCamera(SceneCameras.FollowPlayer));

        _LockOnCamera.SetFollowedTransform(this.m_PlayerObject.transform);
    }

    #endregion Methods

}