using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThatOneSamuraiGame.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region - - - - - - Fields - - - - - -

    //Singleton instance
    public static GameManager instance = null;

    [Header("Game Settings")]
    public GameSettings gameSettings;
    public Transform playerSpawnPoint;
    public bool bShowAttackPopups = true;

    [Header("Camera")]
    [HideInInspector] public Camera mainCamera;
    public GameObject thirdPersonViewCamera;
    public LockOnTracker lockOnTracker;

    [Header("Player")]
    public PlayerController playerController;
    public CameraControl cameraControl;

    [Header("Controllers and Managers")]
    public RewindManager rewindManager;
    public EnemyTracker enemyTracker;

    [Space]
    public PostProcessingController postProcessingController;
    public AudioManager audioManager;

    [Space]
    public CheckpointManager checkpointManager;
    public EnemySpawnManager enemySpawnManager;
    public ButtonController buttonController;

    public BossThemeManager bossThemeManager;

    //UICanvases
    [HideInInspector] public GameObject guardMeterCanvas;

    //Private variables
    private PlayerInputScript _player;
    private GameState m_GameState;

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public GameState GameState
        => this.m_GameState;

    #endregion Properties

    #region - - - - - - Lifecycle Methods - - - - - -

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SetupSceneCamera();
        SetupScene();
        SetupUI();
        SetupRewind();
        SetupAudio();

        this.m_GameState = this.GetComponent<GameState>();
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
    }

    #endregion Lifecycle Methods

    #region - - - - - - Methods - - - - - -

    void SetupScene()
    {
        List<Transform> sceneLoaders = FindObjectsOfType<Transform>().
            Where(o => o.GetComponent<ISceneLoader>() != null).ToList();

        for (int i = 0; i < sceneLoaders.Count; i++)
        {
            ISceneLoader loader = sceneLoaders[i].GetComponent<ISceneLoader>();
            loader.Initialise(mainCamera.transform);
        }
    }

    void SetupSceneCamera()
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

    void SetupUI()
    {
        //guardMeterCanvas = Instantiate(gameSettings.guardCanvasPrefab, transform.position, Quaternion.identity);
    }

    void SetupPlayer()
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

    void InitialisePlayer(GameObject targetHolder)
    {
        playerController.Init(targetHolder);
        _player = playerController.GetComponentInChildren<PlayerInputScript>();
    }

    void SetupEnemies()
    {
        enemyTracker = FindObjectOfType<EnemyTracker>();
        gameSettings.enemySettings.SetTarget(FindObjectOfType<PlayerController>().transform);
        
        //Sets up the test enemies for tracking
        SetupTestScene();
    }

    void SetupTestScene()
    {
        TestStaticTarget[] testEnemies = FindObjectsOfType<TestStaticTarget>();

        // Debug.Log(testEnemies.Length);

        //Check if there is none
        if (testEnemies.Length == 0) return;

        foreach (TestStaticTarget enemy in testEnemies)
        {
            enemyTracker.AddEnemy(enemy.GetComponentInParent<Transform>());
        }
    }

    void SetupRewind() 
    {
        if (rewindManager == null)
        {
            rewindManager = Instantiate(gameSettings.rewindManager, transform.position, Quaternion.identity).GetComponent<RewindManager>();
        }
    }

    void SetupAudio() 
    {
        if (FindObjectOfType<AudioManager>() == null)
        {
            audioManager = Instantiate(gameSettings.audioManger, transform.position, Quaternion.identity).GetComponent<AudioManager>();
        }
    }

    public void EnableInput()
    {
        _player.EnableInput();
    }

    public void DisableInput()
    {
        _player.DisableInput();
    }

    #endregion Methods

}
