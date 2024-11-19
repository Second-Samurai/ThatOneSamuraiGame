using ThatOneSamuraiGame.Scripts;
using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using ThatOneSamuraiGame.Scripts.SetupHandlers.GameSetupHandlers;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseManager;
using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;

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
    public bool bShowAttackPopups = true;

    [Space]
    public PostProcessingController postProcessingController; // keep this, its likely to be part of some graphics manager

    [Header("Persistent Managers")]
    [SerializeField] private SceneManager m_SceneManager;
    [SerializeField] private UserInterfaceManager m_UserInterfaceManager;
    [SerializeField] private PauseManager m_PauseManager;
    [SerializeField] private InputManager m_InputManager;
    [SerializeField] private AudioManager audioManager;

    [Header("SetupHandler")]
    [SerializeField] private GameSetupHandler m_GameSetupHandler;

    private GameState m_GameState;
    
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
        => SceneManager.Instance.PlayerController;
    
    #endregion Properties

    #region - - - - - - Lifecycle Methods - - - - - -

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Run GameSetupHandler
        this.StartCoroutine(this.m_GameSetupHandler.RunSetup());
        
        // Locate services
        this.m_GameState = this.GetComponent<GameState>();
    }

    #endregion Lifecycle Methods

}
