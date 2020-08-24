using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Game Settings")]
    public GameSettings gameSettings;

    //Hidden accessible variables
    [HideInInspector] public GameObject thirdPersonViewCamera;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public EnemyTracker enemyManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SetupSceneCamera();
        //SetupPlayer();
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
    }

    void SetupPlayer()
    {
        GameObject playerObject = Instantiate(gameSettings.playerPrefab, transform.position, Quaternion.identity);
        playerController = playerObject.GetComponentInChildren<PlayerController>();
        playerController.Init();
    }

    void SetupEnemies()
    {
        enemyManager = Instantiate(gameSettings.enemyManagerPrefab, transform.position, Quaternion.identity).GetComponent<EnemyTracker>();
    }
}