using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers;
using UnityEngine;

public class NewCameraSetupHandler : MonoBehaviour, ISetupHandler
{

    #region - - - - - - Fields - - - - - -
    
    [SerializeField] private GameObject m_CameraControlObject;
    [SerializeField] private LockOnSystem m_LockOnSystem;
    [SerializeField] private GameObject m_PlayerObject;

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

        this.SetupLockOnCamera();
        
        print("[LOG]: Completed Scene Camera setup.");
        this.m_NextHandler?.Handle();
    }
    
    private void SetupLockOnCamera()
    {
        if (!GameValidator.NotNull(this.m_LockOnSystem, nameof(this.m_LockOnSystem))) return;
            
        CameraController _CameraController = this.m_CameraControlObject.GetComponent<CameraController>();
        ILockOnCamera _LockOnCamera = _CameraController.GetCamera(SceneCameras.LockOn).GetComponent<ILockOnCamera>();

        GameLogger.Log(
            ("LockOnSystem", m_LockOnSystem), 
            ("CameraController", _CameraController), 
            ("LockOnCamera", _LockOnCamera));
            
        this.m_LockOnSystem.OnLockOnEnable
            .AddListener(() => _CameraController.SelectCamera(SceneCameras.LockOn));
        this.m_LockOnSystem.OnNewLockOnTarget.AddListener(_LockOnCamera.SetLockOnTarget);
        this.m_LockOnSystem.OnLockOnDisable
            .AddListener(() => _CameraController.SelectCamera(SceneCameras.FollowPlayer));

        _LockOnCamera.SetFollowedTransform(this.m_PlayerObject.transform);
    }

    #endregion Methods

}