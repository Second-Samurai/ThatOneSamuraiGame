using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ThatOneSamuraiGame.Scripts;
using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseManager;
using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

// Ticket: #45 - Move managers and services into their own namespaces.
public class GameManager : MonoBehaviour
{
    /*
     * The Game manager should be reserved for:
     * - holding high level references
     * - handling game initialisation 
     * - handling settings
     * - handling state transitions for whole game
     * - handling configuration
     * - Handling coordination of global events
     * - management of global services and lookup
     */
    
    #region - - - - - - Fields - - - - - -

    //Singleton instance
    public static GameManager instance = null;

    [Header("Game Settings")]
    public GameSettings gameSettings;
    // public Transform playerSpawnPoint; // scene manager
    public bool bShowAttackPopups = true;

    // [FormerlySerializedAs("thirdPersonViewCamera")]
    // [FormerlySerializedAs("mainCamera")]
    // [Header("Camera")]
    // [HideInInspector] public Camera MainCamera; // scene manager
    // public GameObject ThirdPersonViewCamera; // scene manager
    // [FormerlySerializedAs("lockOnTracker")]
    // public LockOnTracker LockOnTracker; //scene manager

    // [FormerlySerializedAs("playerController")]
    // [Header("Player")]
    // public PlayerController PlayerController; // scene manager
    // [FormerlySerializedAs("cameraControl")]
    // public CameraControl CameraControl; //scene manager

    // [FormerlySerializedAs("enemyTracker")]
    // [FormerlySerializedAs("rewindManager")]
    // [Header("Controllers and Managers")]
    // public RewindManager RewindManager; //scene manager
    // public EnemyTracker EnemyTracker; // scene manager

    [Space]
    public PostProcessingController postProcessingController; // keep this, its likely to be part of some graphics manager
    public AudioManager audioManager; 

    // [FormerlySerializedAs("checkpointManager")]
    // [Space]
    // public CheckpointManager CheckpointManager; // scene manager
    // public EnemySpawnManager EnemySpawnManager; // scene manager
    // public ButtonController buttonController; // UI manager
    // public BossThemeManager bossThemeManager; // Audio manager

    //UICanvases
    // [HideInInspector] public GameObject guardMeterCanvas; // UI manager
    
    private GameState m_GameState;

    [Header("Persistent Managers")]
    [SerializeField] private SceneManager m_SceneManager;
    [SerializeField] private UserInterfaceManager m_UserInterfaceManager;
    [SerializeField] private PauseManager m_PauseManager;
    
    private IInputManager m_InputManager;
    
    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public GameState GameState
        => this.m_GameState;
    
    // ----------------------------------------------
    // Managers
    // ----------------------------------------------

    public IInputManager InputManager
        => this.m_InputManager;

    public IPauseManager PauseManager
        => this.m_PauseManager;

    public ISceneManager SceneManager
        => this.m_SceneManager;

    public IUserInterfaceManager UserInterfaceManager
        => this.m_UserInterfaceManager;
    
    // ----------------------------------------------
    // Property pass-through 
    // ----------------------------------------------

    // ALL PROPERTIES BELOW THIS:
    //  - This is to only maintain existing references to the old fields to reduce propagated changes.
    //  - Will be resolved once the state of the source values are resolved.
    
    public CheckpointManager CheckpointManager
        => ((ISceneManager)this.m_SceneManager).CheckpointManager;

    public RewindManager RewindManager
        => ((ISceneManager)this.m_SceneManager).RewindManager;

    public EnemyTracker EnemyTracker
        => ((ISceneManager)this.m_SceneManager).EnemyTracker;

    public EnemySpawnManager EnemySpawnManager
        => ((ISceneManager)this.m_SceneManager).EnemySpawnManager;
    
    // ----------------------------------------------
    // Camera
    // ----------------------------------------------

    public CameraControl CameraControl
        => ((ISceneManager)this.m_SceneManager).CameraControl;
    
    public LockOnTracker LockOnTracker
        => ((ISceneManager)this.m_SceneManager).LockOnTracker;
    
    public Camera MainCamera
        => ((ISceneManager)this.m_SceneManager).MainCamera;
    
    public GameObject ThirdPersonViewCamera
        => ((ISceneManager)this.m_SceneManager).ThirdPersonViewCamera;

    // ----------------------------------------------
    // Player
    // ----------------------------------------------

    public PlayerController PlayerController
        => ((ISceneManager)this.m_SceneManager).PlayerController;
    
    #endregion Properties

    #region - - - - - - Lifecycle Methods - - - - - -

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Perform setup pipeline
        // Ticket: #43 - Move this into its own pipeline handler to separate initialisation logic from the game manager.
        
        // Setup game scene
        SetupSceneCamera();
        // SetupScene();
        // SetupUI();
        // SetupRewind();
        SetupAudio();

        // Locate services
        this.m_GameState = this.GetComponent<GameState>();
        this.m_InputManager = this.GetComponent<IInputManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // SetupPlayer();
        // SetupEnemies();
        
        // if (!buttonController)
        // {
        //     Debug.LogError("Main Menu not assigned! Finding in code");
        //     buttonController = GameObject.FindWithTag("MainMenu").GetComponent<ButtonController>();
        // }
        ((ISceneManager)this.m_SceneManager).SetupScene();
        ((IUserInterfaceManager)this.m_UserInterfaceManager).SetupUserInterface();
        
        this.m_InputManager.ConfigureMenuInputControl();
        this.m_InputManager.SwitchToMenuControls();
    }

    #endregion Lifecycle Methods

    #region - - - - - - Methods - - - - - -

    // void SetupScene() // Handled in its own pipeline
    // {
    //     List<Transform> sceneLoaders = FindObjectsOfType<Transform>().
    //         Where(o => o.GetComponent<ISceneLoader>() != null).ToList();
    //
    //     for (int i = 0; i < sceneLoaders.Count; i++)
    //     {
    //         ISceneLoader loader = sceneLoaders[i].GetComponent<ISceneLoader>();
    //         loader.Initialise(MainCamera.transform);
    //     }
    // }

    void SetupSceneCamera() // Handled in pipeline
    {
        // LockOnTracker = FindObjectOfType<LockOnTracker>();
        //
        // if (!ThirdPersonViewCamera)
        // {
        //     Debug.LogError("No third person camera in scene! Adding new object but please assign in inspector instead!");
        //     Vector3 thirdPersonViewPos = gameSettings.thirdPersonViewCam.transform.position;
        //     ThirdPersonViewCamera = Instantiate(gameSettings.thirdPersonViewCam, thirdPersonViewPos, Quaternion.identity);
        //     
        // }
        // Vector3 mainCameraPos = gameSettings.mainCamera.transform.position;
        // MainCamera = Instantiate(gameSettings.mainCamera, mainCameraPos, Quaternion.identity).GetComponent<Camera>();

        //Add Post Processing
        postProcessingController = Instantiate(gameSettings.dayPostProcessing, transform.position, Quaternion.identity).GetComponent<PostProcessingController>();
    }

    // void SetupUI()
    // {
    //     //guardMeterCanvas = Instantiate(gameSettings.guardCanvasPrefab, transform.position, Quaternion.identity);
    // }

    // void SetupPlayer() // Handled in pipeline
    // {
    //     Vector3 targetHolderPos = gameSettings.targetHolderPrefab.transform.position;
    //     GameObject targetHolder = Instantiate(gameSettings.targetHolderPrefab, targetHolderPos, Quaternion.identity);
    //
    //     PlayerController playerControl = FindObjectOfType<PlayerController>();
    //     
    //     if(PlayerController != null)
    //     {
    //         PlayerController = playerControl;
    //         CameraControl = playerControl.GetComponent<CameraControl>();
    //         InitialisePlayer(targetHolder);
    //         return;
    //     }
    //
    //     GameObject playerObject = Instantiate(gameSettings.playerPrefab, playerSpawnPoint.position, Quaternion.identity);
    //     PlayerController = playerObject.GetComponentInChildren<PlayerController>();
    //     InitialisePlayer(targetHolder);
    // }

    // void InitialisePlayer(GameObject targetHolder) // Handled in pipeline
    // {
    //     PlayerController.Init(targetHolder);
    //
    //     if (this.m_InputManager.DoesGameplayInputControlExist()) 
    //         this.m_InputManager.PossesPlayerObject(this.PlayerController.gameObject);
    // }

    // void SetupEnemies() // Handled in pipeline
    // {
    //     EnemyTracker = FindObjectOfType<EnemyTracker>();
    //     gameSettings.enemySettings.SetTarget(FindObjectOfType<PlayerController>().transform);
    //     
    //     //Sets up the test enemies for tracking
    //     SetupTestScene();
    // }
    //
    // void SetupTestScene() // Handled in pipeline
    // {
    //     TestStaticTarget[] testEnemies = FindObjectsOfType<TestStaticTarget>();
    //     
    //     //Check if there is none
    //     if (testEnemies.Length == 0) return;
    //
    //     foreach (TestStaticTarget enemy in testEnemies) 
    //         EnemyTracker.AddEnemy(enemy.GetComponentInParent<Transform>());
    // }

    // void SetupRewind() // Handled in pipeline
    // {
    //     if (RewindManager == null)
    //     {
    //         this.RewindManager = Instantiate(gameSettings.rewindManager, transform.position, Quaternion.identity).GetComponent<RewindManager>();
    //     }
    // }

    void SetupAudio() // Handled in pipeline
    {
        if (FindObjectOfType<AudioManager>() == null) 
            audioManager = Instantiate(gameSettings.audioManger, transform.position, Quaternion.identity).GetComponent<AudioManager>();
    }
    
    // -----------------------------------------------------------
    // Temporary: Actions pertaining to defined game events
    // -----------------------------------------------------------

    // Note: Should be refactored to its own definable hard-coded event.
    public void OnOpeningSceneStart() // Scene manager - but possible delete if not used
        => Debug.LogWarning("Not implemented");

    #endregion Methods

}
