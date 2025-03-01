using System;
using Cinemachine;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

public class PlayerFinisherControllerInitializerData
{

    #region - - - - - - Properties - - - - - -

    public ICameraController CameraController { get; private set; }
    
    public ILockOnObserver LockOnObserver { get; private set; }

    #endregion Properties

    #region - - - - - - Constructors - - - - - -

    public PlayerFinisherControllerInitializerData(ICameraController cameraController, ILockOnObserver lockOnObserver)
    {
        this.CameraController = cameraController;
        this.LockOnObserver = lockOnObserver;
    }

    #endregion Constructors
  
}

/// <summary>
/// Facade for handling 'Finishing' action related behaviour. Its responsible for handling finishing sequences against
/// bladed enemies.
/// </summary>
public class PlayerFinisherAttackHandler : 
    PausableMonoBehaviour, 
    IFinisherAttackHandler, 
    IInitialize<PlayerFinisherControllerInitializerData>
{

    #region - - - - - - Fields - - - - - -

    // Required Components
    [RequiredField] 
    [SerializeField] 
    private PlayerMovement m_PlayerMovement;
    [RequiredField] 
    [SerializeField] 
    private PlayerFinisherCutsceneDirector m_CutSceneDirector;
    [RequiredField] 
    [SerializeField] 
    private PDamageController m_PlayerDamageController;
    [RequiredField] 
    [SerializeField] 
    private PlayerAttackSystem m_PlayerAttackHandler;
    [RequiredField] 
    [SerializeField] 
    private GameObject m_PlayerObject;
    [RequiredField] 
    [SerializeField] 
    private GameObject PlayerDetector; // Note: Used for enemies for detecting the player.
    private ICameraController m_CameraController;
    private ILockOnObserver m_LockOnObserver;
    
    // Runtime Fields
    private GameObject m_TargetEnemy;
    private bool m_CanRunFinisherAttack;

    #endregion Fields
    
    #region - - - - - - Initializers - - - - - -

    public void Initialize(PlayerFinisherControllerInitializerData initializerData)
    {
        this.m_CameraController = initializerData.CameraController 
            ?? throw new ArgumentNullException(nameof(initializerData.CameraController));
        this.m_LockOnObserver = initializerData.LockOnObserver
            ?? throw new ArgumentNullException(nameof(initializerData.LockOnObserver));
    }

    #endregion Initializers
    
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_LockOnObserver.OnNewLockOnTarget.AddListener(enemyTransofrm => 
            ((IFinisherAttackHandler)this).SetFinishingTargetEnemy(enemyTransofrm.gameObject));
        
        CinemachineBrain _MainCameraCinemachineBrain = GameManager.instance.MainCamera.GetComponent<CinemachineBrain>();
        this.m_CutSceneDirector.BindToTrack("Cinemachine Track", _MainCameraCinemachineBrain);
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    public void RunFinishingAttack(GameObject targetEnemy = null)
    {
        if (targetEnemy == null && this.m_TargetEnemy == null || !this.m_CanRunFinisherAttack)
            return;
        
        // Validate target
        IGuarding _EnemyGuard = (targetEnemy ?? this.m_TargetEnemy).GetComponent<IGuarding>();
        if (_EnemyGuard != null)
        {
            // Disable the enemy guard and its UI.
            _EnemyGuard.CanRunCooldownTimer = false;
            _EnemyGuard.CanRunRecoveryTimer = false;
            _EnemyGuard.UIGuardMeter.HideFinisherKey();

            this.m_PlayerDamageController.DisableDamage();
            
            // Note: This is only a temporary solution
            PlayerAttackState _PlayerAttackState = 
                this.m_PlayerObject.GetComponent<IPlayerState>().PlayerAttackState;
            _PlayerAttackState.CanAttack = false;
        }

        this.m_CameraController.SelectCamera(SceneCameras.FollowPlayer);
        ((IPlayerMovement)this.m_PlayerMovement).SetState(PlayerMovementStates.Finisher);
        
        this.m_CutSceneDirector.Play();
        this.PlayerDetector.SetActive(false);
    }

    // This is being invoked by the SignalReciever component, triggered by the timeline played by the director.
    public void EndFinishingActionSequence()
    {
        this.PlayerDetector.SetActive(true);
        this.m_PlayerDamageController.EnableDamage();
        ((IPlayerMovement)this.m_PlayerMovement).EnableMovement();
        ((IPlayerAttackSystem)m_PlayerAttackHandler).ResetAttack();
        
        this.m_CameraController.SelectCamera(SceneCameras.FollowPlayer);
        ((IPlayerMovement)this.m_PlayerMovement).SetState(PlayerMovementStates.Normal);
    }
    
    public void EnableFinisherAttack()
        => this.m_CanRunFinisherAttack = true;
    
    public void DisableFinisherAttack()
        => this.m_CanRunFinisherAttack = false;
    
    // Transform is passed instead of GameObject as LockOnSystem relies on transform over gameobject.
    void IFinisherAttackHandler.SetFinishingTargetEnemy(GameObject targetEnemy)
    {
        this.m_TargetEnemy = targetEnemy;
        this.m_CutSceneDirector.BindToTrack("Animation Track (1)", this.m_TargetEnemy);
    }

    #endregion Methods
  
}
