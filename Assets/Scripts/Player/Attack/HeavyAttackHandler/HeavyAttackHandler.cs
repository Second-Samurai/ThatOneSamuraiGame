using Player.Animation;
using ThatOneSamuraiGame;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using UnityEngine;

public class HeavyAttackHandler : PausableMonoBehaviour, IInitialize<HeavyAttackInitializerData>
{

    #region - - - - - - Fields - - - - - -

    // Player Components
    private IPlayerAnimationDispatcher m_AnimationDispatcher;
    private ICameraController m_CameraController;
    private PlayerAnimationComponent m_PlayerAnimationComponent;
    private PlayerAttackState m_PlayerAttackState;
    private IWeaponSystem m_WeaponSystem;
    
    [SerializeField] 
    private GameEvent m_ShowHeavyTutorialEvent;
    [SerializeField] 
    private GameEvent m_StartHeavyTelegraphEvent;
    [SerializeField] 
    private GameEvent m_EndHeavyTelegraphEvent; 
    
    // Heavy Attack Timer Fields
    private readonly float m_HeavyAttackChargeTime = 1.5f;
    private EventTimer m_HeavyAttackTimer;

    #endregion Fields

    #region - - - - - - Initializers - - - - - -

    public void Initialize(HeavyAttackInitializerData initializerData)
        => this.m_CameraController = initializerData.CameraController;

    #endregion Initializers
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_AnimationDispatcher = this.GetComponent<IPlayerAnimationDispatcher>();
        this.m_PlayerAnimationComponent = this.GetComponent<PlayerAnimationComponent>();
        this.m_PlayerAttackState = this.GetComponent<IPlayerState>().PlayerAttackState;
        this.m_WeaponSystem = this.GetComponent<IWeaponSystem>();
        
        BlockingAttackHandler _BlockingAttackHandler = this.GetComponent<BlockingAttackHandler>();
        
        this.m_HeavyAttackTimer = new EventTimer(
            this.m_HeavyAttackChargeTime,
            Time.deltaTime,
            _BlockingAttackHandler.m_BlockingEffects.PlayGleam,
            false,
            false);
    }

    private void Update()
    {
        if (this.IsPaused)
            return;
    
        if (this.m_PlayerAttackState.IsHeavyAttackCharging) 
            this.m_HeavyAttackTimer.TickTimer();
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    public void StartHeavyAttack()
    {
        if (!this.m_WeaponSystem.IsWeaponEquipped()) return;
        
        // Commenting line below from merge conflict with camera rework
        //if (!this.m_PlayerAttackState.CanAttack /*&& this.m_Animator.GetBool("HeavyAttackHeld")*/)
        if (!this.m_PlayerAttackState.CanAttack 
            && this.m_AnimationDispatcher.Check(PlayerAnimationCheckState.HeavyAttackHeld))
            return;
        
        this.m_HeavyAttackTimer.StartTimer();
        
        this.m_StartHeavyTelegraphEvent.Raise();
        
        this.m_PlayerAttackState.IsWeaponSheathed = true;
        this.m_PlayerAttackState.IsHeavyAttackCharging = true;
          
        // Commenting line below from merge conflict with camera rework
        this.m_PlayerAnimationComponent.ResetAttackParameters();
        this.m_PlayerAnimationComponent.ChargeHeavyAttack(true);
        
        this.m_AnimationDispatcher.Dispatch(PlayerAnimationEventStates.StartHeavyAttackHeld);
        
        // Ends the camera roll
        // TODO: Fix the error from the line below
        GameLogger.Log(this.m_CameraController.ToString());
        this.m_CameraController.EndCameraAction();
    }
    
    public void PerformHeavyAttack()
    {
        this.m_ShowHeavyTutorialEvent.Raise();
        this.m_EndHeavyTelegraphEvent.Raise();

        this.m_PlayerAttackState.IsHeavyAttackCharging = false;
        this.m_PlayerAttackState.IsWeaponSheathed = false;
        
        this.m_HeavyAttackTimer.StopTimer();
        this.m_HeavyAttackTimer.ResetTimer();
        
        this.m_PlayerAnimationComponent.TriggerHeavyAttack();
        this.m_AnimationDispatcher.Dispatch(PlayerAnimationEventStates.EndHeavyAttachHeld);
        
        // Rolls the camera
        // TODO: Fix the error from the code below
        IFreelookCameraController _FreeLookCamera = this.m_CameraController
            .GetCamera(SceneCameras.FreeLook)
            .GetComponent<IFreelookCameraController>();
        CameraRollAction _CameraRoll = new CameraRollAction(_FreeLookCamera, this);
        this.m_CameraController.SetCameraAction(_CameraRoll);
    }

    #endregion Methods
  
}

public class HeavyAttackInitializerData
{

    #region - - - - - - Properties - - - - - -

    public ICameraController CameraController { get; set; }

    #endregion Properties
  
}
