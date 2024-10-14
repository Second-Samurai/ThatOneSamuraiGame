using System.Collections.Generic;
using System.Linq;
using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Scene.DataContainers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThatOneSamuraiGame.Scripts.Scene.SceneManager
{

    /// <summary>
    /// Responsible for managing both state and behavior of the scene.
    /// </summary>
    public class SceneManager : MonoBehaviour, ISceneManager
    {

        #region - - - - - - Fields - - - - - -
        
        [Header("Data State")]
        [SerializeField] private GameSettings m_GameSettings;
        [SerializeField] private SceneState m_SceneState;

        [Header("Controllers and Managers")]
        [SerializeField] private CheckpointManager m_CheckPointManager; 
        [Space]
        [SerializeField] private EnemyTracker m_EnemyTracker;
        [SerializeField] private EnemySpawnManager m_EnemySpawnManager;

        [Header("Camera")]
        [SerializeField] private CameraControl m_CameraControl;
        [SerializeField] private LockOnTracker m_LockOnTracker;
        [SerializeField] private GameObject m_ThirdPersonViewCamera;
        private UnityEngine.Camera m_MainCamera;

        [Header("Player")]
        [SerializeField] private PlayerController m_PlayerController;
        [SerializeField] private Transform m_PlayerSpawnPoint;
        
        #endregion Fields
        
        #region - - - - - - Properties - - - - - -

        // -------------------------------
        // Data State
        // -------------------------------
        
        SceneState ISceneManager.SceneState
            => this.m_SceneState;
        
        // -------------------------------
        // Controllers and Managers
        // -------------------------------
        
        CheckpointManager ISceneManager.CheckpointManager
            => this.m_CheckPointManager;
         
        EnemyTracker ISceneManager.EnemyTracker
            => this.m_EnemyTracker;

        EnemySpawnManager ISceneManager.EnemySpawnManager
            => this.m_EnemySpawnManager;
        
        // -------------------------------
        // Camera
        // -------------------------------
        
        CameraControl ISceneManager.CameraControl
            => this.m_CameraControl;

        LockOnTracker ISceneManager.LockOnTracker
            => this.m_LockOnTracker;

        UnityEngine.Camera ISceneManager.MainCamera
            => this.m_MainCamera;

        GameObject ISceneManager.ThirdPersonViewCamera
            => this.m_ThirdPersonViewCamera;
        
        // -------------------------------
        // Player
        // -------------------------------
        
        PlayerController ISceneManager.PlayerController
            => this.m_PlayerController;

        #endregion Properties

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            if (!this.DoesSceneStateExist())
                this.m_SceneState = this.GetComponent<SceneState>();
        }

        #endregion Lifecycle Methods

        #region - - - - - - Methods - - - - - -
        
        void ISceneManager.SetupScene()
        {
            this.SetupSceneCamera();
            this.SetupSceneLoaders(); 
            this.SetupPlayer();
            this.SetupEnemies();
        }
        
        // --------------------------------------------
        // Level specific behavior
        // -------------------------------------------
        
        private void SetupSceneLoaders() // Handled in its own pipeline
        {
            List<Transform> _SceneLoaders = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None)
                                                .Where(t => t.GetComponent<ISceneLoader>() != null)
                                                .ToList();

            for (int i = 0; i < _SceneLoaders.Count; i++)
            {
                ISceneLoader _Loader = _SceneLoaders[i].GetComponent<ISceneLoader>();
                _Loader.Initialise(this.m_MainCamera.transform);
            }
        }

        // --------------------------------------------
        // Camera specific behavior
        // -------------------------------------------

        private void SetupSceneCamera() // Handled in pipeline
        {
            if (!this.m_ThirdPersonViewCamera)
            {
                Debug.LogError("No third person camera in scene! Adding new object but please assign in inspector instead!");
                
                Vector3 _ThirdPersonViewPos = this.m_GameSettings.thirdPersonViewCam.transform.position;
                this.m_ThirdPersonViewCamera = Instantiate(
                                                this.m_GameSettings.thirdPersonViewCam, 
                                                _ThirdPersonViewPos, 
                                                Quaternion.identity);
            }
            
            Vector3 _MainCameraPos = this.m_GameSettings.mainCamera.transform.position;
            this.m_MainCamera = Instantiate(
                                    this.m_GameSettings.mainCamera, 
                                    _MainCameraPos, 
                                    Quaternion.identity
                                    ).GetComponent<UnityEngine.Camera>();
        }
        
        // --------------------------------------------
        // Player specific behavior
        // -------------------------------------------
        
        private void SetupPlayer() // Handled in pipeline
        {
            Vector3 _TargetHolderPos = this.m_GameSettings.targetHolderPrefab.transform.position;
            GameObject _TargetHolder = Instantiate(this.m_GameSettings.targetHolderPrefab, _TargetHolderPos, Quaternion.identity);

            PlayerController _PlayerControl = Object.FindFirstObjectByType<PlayerController>();
        
            if(this.m_PlayerController != null)
            {
                this.m_PlayerController = _PlayerControl;
                this.m_CameraControl = _PlayerControl.GetComponent<CameraControl>();
                this.InitialisePlayer(_TargetHolder);
                return;
            }

            GameObject _PlayerObject = Instantiate(this.m_GameSettings.playerPrefab, this.m_PlayerSpawnPoint.position, Quaternion.identity);
            this.m_PlayerController = _PlayerObject.GetComponentInChildren<PlayerController>();
            this.InitialisePlayer(_TargetHolder);
        }
        
        private void InitialisePlayer(GameObject targetHolder) // Handled in pipeline
        {
            this.m_PlayerController.Init(targetHolder);

            IInputManager _InputManager = GameManager.instance.InputManager;

            if (_InputManager.DoesGameplayInputControlExist()) 
                _InputManager.PossesPlayerObject(this.m_PlayerController.gameObject);
        }
        
        // --------------------------------------------
        // Enemy specific behavior
        // -------------------------------------------
        
        private void SetupEnemies() // Handled in pipeline
        {
            this.m_EnemyTracker = FindFirstObjectByType<EnemyTracker>();
            this.m_GameSettings.enemySettings.SetTarget(FindFirstObjectByType<PlayerController>().transform);
        
            //Sets up the test enemies for tracking
            this.SetupTestScene();
        }

        // This appears to be less relevant and more debug related
        private void SetupTestScene() // Handled in pipeline
        {
            TestStaticTarget[] _TestEnemies = FindObjectsByType<TestStaticTarget>(FindObjectsSortMode.None);
        
            //Check if there is none
            if (_TestEnemies.Length == 0) return;

            foreach (TestStaticTarget _Enemy in _TestEnemies) 
                this.m_EnemyTracker.AddEnemy(_Enemy.GetComponentInParent<Transform>());
        }
         
        #endregion Methods

        #region - - - - - - Validation Methods - - - - - -

        private bool DoesSceneStateExist()
            => this.m_SceneState != null;

        #endregion Validation Methods

    }

}