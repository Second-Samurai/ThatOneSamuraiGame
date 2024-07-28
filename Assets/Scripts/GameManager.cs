using System.Collections.Generic;
using System.Linq;
using ThatOneSamuraiGame.Scripts;
using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseManager;
using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;

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
    public Transform playerSpawnPoint; // scene manager
    public bool bShowAttackPopups = true;

    [Header("Camera")]
    [HideInInspector] public Camera mainCamera; // scene manager
    public GameObject thirdPersonViewCamera; // scene manager
    public LockOnTracker lockOnTracker; //scene manager

    [Header("Player")]
    public PlayerController playerController; // scene manager
    public CameraControl cameraControl; //scene manager

    [Header("Controllers and Managers")]
    public RewindManager rewindManager; //scene manager
    public EnemyTracker enemyTracker; // scene manager

    [Space]
    public PostProcessingController postProcessingController; // keep this, its likely to be part of some graphics manager
    public AudioManager audioManager; 

    [Space]
    public CheckpointManager checkpointManager; // scene manager
    public EnemySpawnManager enemySpawnManager; // scene manager
    public ButtonController buttonController; // UI manager
    public BossThemeManager bossThemeManager; // scene manager. Likely rename as its responsible for mostly one specific area

    //UICanvases
    [HideInInspector] public GameObject guardMeterCanvas; // UI manager
    
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
        SetupScene();
        // SetupUI();
        SetupRewind();
        SetupAudio();

        // Locate services
        this.m_GameState = this.GetComponent<GameState>();
        
        this.m_InputManager = this.GetComponent<IInputManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupPlayer();
        SetupEnemies();
        if (!buttonController)
        {
            Debug.LogError("Main Menu not assigned! Finding in code");
            buttonController = GameObject.FindWithTag("MainMenu").GetComponent<ButtonController>();
        }
        
        this.m_InputManager.ConfigureMenuInputControl();
        this.m_InputManager.SwitchToMenuControls();
    }

    #endregion Lifecycle Methods

    #region - - - - - - Methods - - - - - -

    void SetupScene() // Handled in its own pipeline
    {
        List<Transform> sceneLoaders = FindObjectsOfType<Transform>().
            Where(o => o.GetComponent<ISceneLoader>() != null).ToList();

        for (int i = 0; i < sceneLoaders.Count; i++)
        {
            ISceneLoader loader = sceneLoaders[i].GetComponent<ISceneLoader>();
            loader.Initialise(mainCamera.transform);
        }
    }

    void SetupSceneCamera() // Handled in pipeline
    {
        lockOnTracker = FindObjectOfType<LockOnTracker>();
        
        if (!thirdPersonViewCamera)
        {
            Debug.LogError("No third person camera in scene! Adding new object but please assign in inspector instead!");
            Vector3 thirdPersonViewPos = gameSettings.thirdPersonViewCam.transform.position;
            thirdPersonViewCamera = Instantiate(gameSettings.thirdPersonViewCam, thirdPersonViewPos, Quaternion.identity);
            
        }
        Vector3 mainCameraPos = gameSettings.mainCamera.transform.position;
        mainCamera = Instantiate(gameSettings.mainCamera, mainCameraPos, Quaternion.identity).GetComponent<Camera>();

        //Add Post Processing
        postProcessingController = Instantiate(gameSettings.dayPostProcessing, transform.position, Quaternion.identity).GetComponent<PostProcessingController>();
    }

    // void SetupUI()
    // {
    //     //guardMeterCanvas = Instantiate(gameSettings.guardCanvasPrefab, transform.position, Quaternion.identity);
    // }

    void SetupPlayer() // Handled in pipeline
    {
        Vector3 targetHolderPos = gameSettings.targetHolderPrefab.transform.position;
        GameObject targetHolder = Instantiate(gameSettings.targetHolderPrefab, targetHolderPos, Quaternion.identity);

        PlayerController playerControl = FindObjectOfType<PlayerController>();
        
        if(playerController != null)
        {
            playerController = playerControl;
            cameraControl = playerControl.GetComponent<CameraControl>();
            InitialisePlayer(targetHolder);
            return;
        }

        GameObject playerObject = Instantiate(gameSettings.playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        playerController = playerObject.GetComponentInChildren<PlayerController>();
        InitialisePlayer(targetHolder);
    }

    void InitialisePlayer(GameObject targetHolder) // Handled in pipeline
    {
        playerController.Init(targetHolder);

        if (this.m_InputManager.DoesGameplayInputControlExist()) 
            this.m_InputManager.PossesPlayerObject(this.playerController.gameObject);
    }

    void SetupEnemies() // Handled in pipeline
    {
        enemyTracker = FindObjectOfType<EnemyTracker>();
        gameSettings.enemySettings.SetTarget(FindObjectOfType<PlayerController>().transform);
        
        //Sets up the test enemies for tracking
        SetupTestScene();
    }

    void SetupTestScene() // Handled in pipeline
    {
        TestStaticTarget[] testEnemies = FindObjectsOfType<TestStaticTarget>();
        
        //Check if there is none
        if (testEnemies.Length == 0) return;

        foreach (TestStaticTarget enemy in testEnemies) 
            enemyTracker.AddEnemy(enemy.GetComponentInParent<Transform>());
    }

    void SetupRewind() // Handled in pipeline
    {
        if (rewindManager == null)
        {
            rewindManager = Instantiate(gameSettings.rewindManager, transform.position, Quaternion.identity).GetComponent<RewindManager>();
        }
    }

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
