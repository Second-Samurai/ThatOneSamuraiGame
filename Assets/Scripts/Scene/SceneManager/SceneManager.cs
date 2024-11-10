using ThatOneSamuraiGame.Scripts.Enumeration;
using ThatOneSamuraiGame.Scripts.Scene.DataContainers;
using ThatOneSamuraiGame.Scripts.Scene.Loaders;
using UnityEngine;

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
        [Space]
        [SerializeField] public EnemyTracker m_EnemyTracker;
        [SerializeField] public EnemySpawnManager m_EnemySpawnManager;
        [SerializeField] private SceneLoader m_SceneLoader;

        [Header("Camera")]
        [SerializeField] public CameraControl m_CameraControl;
        [SerializeField] public LockOnTracker m_LockOnTracker;
        [SerializeField] public GameObject m_ThirdPersonViewCamera;
        private UnityEngine.Camera m_MainCamera;

        [Header("Player")]
        [SerializeField] public PlayerController m_PlayerController;
        [SerializeField] public Transform m_PlayerSpawnPoint;

        private GameScenes m_CurrentGameScene;
        
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
        
        CheckpointManager ISceneManager.CheckpointManager
            => this.m_CheckPointManager;
         
        EnemyTracker ISceneManager.EnemyTracker
            => this.m_EnemyTracker;
        
        EnemySpawnManager ISceneManager.EnemySpawnManager
            => this.m_EnemySpawnManager;

        SceneLoader ISceneManager.SceneLoader
            => this.m_SceneLoader;
        
        // -------------------------------
        // Camera
        // -------------------------------
        
        public CameraControl CameraControl
        {
            get { return this.m_CameraControl; }
            set { this.m_CameraControl = value; }
        }

        public LockOnTracker LockOnTracker
        {
            get { return this.m_LockOnTracker; }
            set { this.m_LockOnTracker = value; }
        }

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
        
        void ISceneManager.SetupCurrentScene(GameScenes gameScene) 
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

        private bool DoesSceneStateExist()
            => this.m_SceneState != null;

        #endregion Validation Methods

    }

}