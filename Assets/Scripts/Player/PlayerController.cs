using UnityEngine;
using Cinemachine;
using ThatOneSamuraiGame.Legacy;
using ThatOneSamuraiGame.Scripts.Player.Attack;

public interface IPlayerController {
    string GetStringID();
    StatHandler GetPlayerStats();
}

//Replace to this
public interface IEntity
{
    string GetStringID();
    StatHandler GetPlayerStats();
}

public interface ISecretValidator
{
    int GetKillCount();
    int GetKeyCount();
}

public class PlayerController : MonoBehaviour, IEntity, ISecretValidator, IInitialize<PlayerControllerInitializerData>
{
    
    #region - - - - - - Fields - - - - - -

    public string playerID = "defaultID1234";
    [HideInInspector] public StatHandler playerStats;
    [HideInInspector] public PlayerSettings playerSettings;
    [HideInInspector] public PlayerStateMachine stateMachine;
    // [HideInInspector] public CameraControl cameraController; // TODO: Remove any old reference to this

    [HideInInspector] public int totalCollectedKeys = 0;
    [HideInInspector] public int totalKillCount = 0;

    private PlayerState m_PlayerState;

    #endregion Fields

    #region - - - - - - Lifecycle Methods - - - - - -

    //NOTE: Once object is spawned through code init through awake instead.
    void Awake() {
        //NOTE: This is only temporary to fix the camera referencing issues
        // GameManager.instance.PlayerController = this;
    }

    private void Start() 
        => this.m_PlayerState = this.GetComponent<PlayerState>();

    #endregion Lifecycle Methods

    #region - - - - - - Methods - - - - - -

    //Summary: Sets initial state and initialise variables
    //
    public void Initialize(PlayerControllerInitializerData initializerData)
    {
        // GameManager gameManager = GameManager.instance;
        // playerSettings = gameManager.gameSettings.playerSettings;
        // EntityStatData playerData = playerSettings.playerStats;
        //
        // playerStats = new StatHandler();
        // playerStats.Init(playerData);

        this.playerStats = initializerData.PlayerStatHandler;
        this.stateMachine = this.gameObject.AddComponent<PlayerStateMachine>();

        //This assigns the thirdperson camera targets to this player
        // CinemachineFreeLook freeLockCamera = gameManager.ThirdPersonViewCamera.GetComponent<CinemachineFreeLook>();
        //freeLockCamera.Follow = this.transform;
        //freeLockCamera.LookAt = this.transform;

        // PCombatController combatController = this.GetComponent<PCombatController>();
        // combatController.Init(playerStats);
        // combatController.UnblockCombatInputs();

        IPlayerAttackHandler _AttackHandler = this.GetComponent<IPlayerAttackHandler>();
        _AttackHandler.EnableAttack();

        this.SetState<PNormalState>();
    }

    //Summary: Clears and Sets the new specified state for player.
    //
    public void SetState<T>() where T : PlayerState 
        => stateMachine.AddState<T>();

    public string GetStringID() 
        => playerID;

    public StatHandler GetPlayerStats() 
        => playerStats;

    public int GetKillCount() 
        => totalKillCount;

    public int GetKeyCount() 
        => totalCollectedKeys;

    #endregion Methods
    
}

public class PlayerControllerInitializerData
{

    #region - - - - - - Properties - - - - - -

    public StatHandler PlayerStatHandler { get; set; }

    #endregion Properties
  
}
