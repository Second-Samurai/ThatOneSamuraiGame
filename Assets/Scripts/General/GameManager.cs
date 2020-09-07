using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Game Settings")]
    public GameSettings gameSettings;
    public Transform playerSpawnPoint;

    //Hidden accessible variables
    [HideInInspector] public GameObject thirdPersonViewCamera;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Camera mainCamera;
    public RewindManager rewindManager;
    public EnemyTracker enemyTracker;

    //UICanvases
    [HideInInspector] public GameObject guardMeterCanvas;


    PlayerInputScript _player;

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
        SetupUI();
        SetupRewind();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupPlayer();
        SetupEnemies();
    }

    void SetupSceneCamera()
    {
        Vector3 thirdPersonViewPos = gameSettings.thirdPersonViewCam.transform.position;
        thirdPersonViewCamera = Instantiate(gameSettings.thirdPersonViewCam, thirdPersonViewPos, Quaternion.identity);

        Vector3 mainCameraPos = gameSettings.mainCamera.transform.position;
        mainCamera = Instantiate(gameSettings.mainCamera, mainCameraPos, Quaternion.identity).GetComponent<Camera>();

        //Add Post Processing
        Instantiate(gameSettings.dayPostProcessing, transform.position, Quaternion.identity);
    }

    void SetupUI()
    {
        guardMeterCanvas = Instantiate(gameSettings.guardCanvasPrefab, transform.position, Quaternion.identity);
    }

    void SetupPlayer()
    {
        Vector3 targetHolderPos = gameSettings.targetHolderPrefab.transform.position;
        GameObject tarrgetHolder = Instantiate(gameSettings.targetHolderPrefab, targetHolderPos, Quaternion.identity);

        GameObject playerObject = Instantiate(gameSettings.playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        playerController = playerObject.GetComponentInChildren<PlayerController>();
        playerController.Init(tarrgetHolder);
        _player = playerObject.GetComponentInChildren<PlayerInputScript>();
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
        Debug.Log(">> GameManager: Guard Meter Added");
        return guardMeter;
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