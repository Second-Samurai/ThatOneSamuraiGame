using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Legacy;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public class CameraSetupHandler : MonoBehaviour, ISetupHandler
    {

        #region - - - - - - Fields - - - - - -
        
        [SerializeField] public GameObject m_ThirdPersonViewCamera;

        private ISetupHandler m_NextHandler;

        #endregion Fields
  
        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle()
        {
            GameSettings _GameSettings = GameManager.instance.gameSettings;
            
            this.m_ThirdPersonViewCamera ??= FindFirstObjectByType<ThirdPersonCamController>().gameObject;
            if (!this.m_ThirdPersonViewCamera)
            {
                Debug.LogWarning("No third person camera in scene! Adding new object but please assign in inspector instead!");
                
                // TODO: No point in creating a position variable within this scope, just pass directly.
                Vector3 _ThirdPersonViewPos = _GameSettings.thirdPersonViewCam.transform.position;
                this.m_ThirdPersonViewCamera = Instantiate(
                    _GameSettings.thirdPersonViewCam, 
                    _ThirdPersonViewPos, 
                    Quaternion.identity);
                Debug.LogWarning("Third person camera instantiated for " + nameof(this.m_ThirdPersonViewCamera));
            }
            
            Vector3 _MainCameraPos = _GameSettings.mainCamera.transform.position;
            SceneManager.Instance.MainCamera = Instantiate(
                _GameSettings.mainCamera, 
                _MainCameraPos, 
                Quaternion.identity
            ).GetComponent<UnityEngine.Camera>();
            
            // Get references from scene
            SceneManager.Instance.m_ThirdPersonViewCamera = this.m_ThirdPersonViewCamera;
            SceneManager.Instance.LockOnTracker = Object.FindFirstObjectByType<LockOnTracker>(); // TODO: Remove this, thisd is now legacy
            
            print("[LOG]: Completed Scene Camera setup.");
            this.m_NextHandler?.Handle();
        }

        #endregion Methods
  
    }

}