using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Enumeration;
using ThatOneSamuraiGame.Scripts.Scene.DataContainers;
using ThatOneSamuraiGame.Scripts.Scene.Loaders;
using UnityEngine;
using UnityEngine.Events;

namespace ThatOneSamuraiGame.Scripts.Scene.SceneManager
{

    /// <summary>
    /// Responsible for managing both state and behavior of the scene.
    /// </summary>
    public class SceneManager : MonoBehaviour, ISceneManager
    {

        #region - - - - - - Fields - - - - - -

        public static SceneManager Instance;

        [Header("Data State")]
        [SerializeField] public GameSettings m_GameSettings;
        [SerializeField] public SceneState m_SceneState;

        [Header("Controllers and Managers")]
        [SerializeField] private CheckpointManager m_CheckPointManager; 
        [SerializeField] public EnemySpawnManager m_EnemySpawnManager;
        [SerializeField] private SceneLoader m_SceneLoader;
        private EnemyTracker m_EnemyTracker;

        [Header("Camera")]
        [SerializeField] public GameObject m_ThirdPersonViewCamera;
        private UnityEngine.Camera m_MainCamera;

        [Header("Player")]
        [SerializeField] public PlayerController m_PlayerController;
        [SerializeField] public Transform m_PlayerSpawnPoint;

        [Header("Scene Management")]
        [SerializeField] private List<GameScene> m_LoadedScenes;
        private GameScene m_CurrentGameScene;
        
        #endregion Fields
        
        #region - - - - - - Properties - - - - - -

        // -------------------------------
        // Data State
        // -------------------------------
        
        public SceneState SceneState
        {
            get { return this.m_SceneState; }
            set { this.m_SceneState = value; }
        }

        // -------------------------------
        // Controllers and Managers
        // -------------------------------
        
        public CheckpointManager CheckpointManager
        {
            get => this.m_CheckPointManager;
            set => this.m_CheckPointManager = value;
        }

        public EnemyTracker EnemyTracker
        {
            get => this.m_EnemyTracker; 
            set => this.m_EnemyTracker = value;
        }

        EnemySpawnManager ISceneManager.EnemySpawnManager
            => this.m_EnemySpawnManager;

        public SceneLoader SceneLoader
            => this.m_SceneLoader;
        
        // -------------------------------
        // Camera
        // -------------------------------
        
        public ICameraController CameraController { get; set; }
        
        public HitstopController HitstopController { get; set; }
        
        public ILockOnSystem LockOnSystem { get; set; }

        public ILockOnObserver LockOnObserver { get; set; }
        
        public UnityEngine.Camera MainCamera
        {
            get { return this.m_MainCamera; }
            set { this.m_MainCamera = value; }
        }

        GameObject ISceneManager.ThirdPersonViewCamera
            => this.m_ThirdPersonViewCamera;
        
        // -------------------------------
        // Player
        // -------------------------------
        
        public PlayerController PlayerController
        {
            get => this.m_PlayerController;
            set => this.m_PlayerController = value;
        }

        #endregion Properties

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            if (!this.DoesSceneStateExist())
                this.m_SceneState = this.GetComponent<SceneState>();
        }

        #endregion Lifecycle Methods

        #region - - - - - - Methods - - - - - -

        public void AddLoadedScene(GameScene scene)
        {
            
        }
        
        void ISceneManager.SetupCurrentScene(GameScene gameScene) 
            => this.m_CurrentGameScene = gameScene;

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

        public bool IsMembersValid()
        {
            return GameValidator.NotNull(this.m_CheckPointManager, nameof(this.m_CheckPointManager))
                   & GameValidator.NotNull(this.m_EnemyTracker, nameof(this.m_EnemyTracker))
                   & GameValidator.NotNull(this.m_EnemySpawnManager, nameof(this.m_EnemySpawnManager))
                   & GameValidator.NotNull(this.m_SceneLoader, nameof(this.m_SceneLoader))
                   & GameValidator.NotNull(this.m_MainCamera, nameof(this.m_MainCamera));
        }

        private bool DoesSceneStateExist()
            => this.m_SceneState != null;

        #endregion Validation Methods

    }

}