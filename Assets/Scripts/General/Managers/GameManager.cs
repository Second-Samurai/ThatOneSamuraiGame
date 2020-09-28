using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton instance
    public static GameManager instance = null;

    [Header("Game Settings")]
    public GameSettings gameSettings;
    public Transform playerSpawnPoint;

    //Hidden accessible variables
    [HideInInspector] public Camera mainCamera;
    public GameObject thirdPersonViewCamera;
    public PlayerController playerController;
    public RewindManager rewindManager;
    public EnemyTracker enemyTracker;
    public PostProcessingController postProcessingController;
    public AudioManager audioManager;

    //UICanvases
    [HideInInspector] public GameObject guardMeterCanvas;

    //Private variables
    private PlayerInputScript _player;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupPlayer();
        SetupEnemies();
    }

    void SetupScene()
    {
        List<Transform> sceneLoaders = GameObject.FindObjectsOfType<Transform>().
            Where(o => o.GetComponent<ISceneLoader>() != null).ToList();

        for (int i = 0; i < sceneLoaders.Count; i++)
        {
            ISceneLoader loader = sceneLoaders[i].GetComponent<ISceneLoader>();
            loader.Initialise(mainCamera.transform);
        }
    }

    void SetupSceneCamera()
    {
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
        guardMeterCanvas = Instantiate(gameSettings.guardCanvasPrefab, transform.position, Quaternion.identity);
    }

    void SetupPlayer()
    {
        Vector3 targetHolderPos = gameSettings.targetHolderPrefab.transform.position;
        GameObject targetHolder = Instantiate(gameSettings.targetHolderPrefab, targetHolderPos, Quaternion.identity);

        PlayerController playerControl = GameObject.FindObjectOfType<PlayerController>();
        if(playerController != null)
        {
            playerController = playerControl;
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
        enemyTracker = Instantiate(gameSettings.enemyManagerPrefab, transform.position, Quaternion.identity).GetComponent<EnemyTracker>();
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
        rewindManager = Instantiate(gameSettings.rewindManager, transform.position, Quaternion.identity).GetComponent<RewindManager>();
    }

    //POSSIBLY PARTITION INTO A UI MANAGER
    public UIGuardMeter CreateEntityGuardMeter(Transform entityTransform, StatHandler entityStatHandler)
    {
        UIGuardMeter guardMeter = Instantiate(gameSettings.guardMeterPrefab, guardMeterCanvas.transform).GetComponent<UIGuardMeter>();
        guardMeter.Init(entityTransform, entityStatHandler, mainCamera, guardMeterCanvas.GetComponent<RectTransform>());
        //Debug.Log(">> GameManager: Guard Meter Added");
        return guardMeter;
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

    
}