using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

public class PlayerFinishMovement : BasePlayerMovementState
{
    #region - - - - - - Fields - - - - - -

    // private SphereCollider m_CollisionDetector; // May not be used
    // public PlayableAsset[] finishingMoves;
    // public bool bIsFinishing = false; // Might be no longer relevant

    // private PlayableDirector _cutsceneDirector;
    // private PDamageController damageController;
    // private List<Transform> enemies; // There is no reason to have all the enemies
    // private List<AISystem> enemiesCache; // This seems unnecessary
    // private GameObject targetEnemy;

    private readonly ILockOnSystem m_LockOnSystem;
    // private readonly Transform m_PlayerTransform;
    // private readonly MonoBehaviour m_ReferenceMonoBehaviour; // Required for Coroutine
    
    // private bool m_HasPlayedFinisherMove;
    private GameObject m_TargetEnemy;

    #endregion Fields

    public PlayerFinishMovement(
        ILockOnSystem lockOnSystem,
        PlayerMovementState movementState,
        Animator playerAnimator,
        Transform playerTransform,
        MonoBehaviour refMonoBehaviour)
        :base(playerAnimator, playerTransform, movementState)
    {
        // this.m_PlayerTransform = playerTransform ?? throw new ArgumentNullException(nameof(playerTransform));
        // this.m_ReferenceMonoBehaviour = refMonoBehaviour ?? throw new ArgumentNullException(nameof(refMonoBehaviour));
        
        // lockOnSystem.OnNewLockOnTarget.AddListener(this.SetFinishingTargetEnemy);
        // lockOnSystem.OnLockOnDisable.AddListener(this.ResetState);
        
        // _cutsceneDirector = this.m_ReferenceMonoBehaviour.GetComponent<PlayableDirector>();
        // BindToTrack("Cinemachine Track", GameManager.instance.MainCamera.GetComponent<CinemachineBrain>());
        // damageController = GameManager.instance.PlayerController.gameObject.GetComponent<PDamageController>();
    }
  
    public override void CalculateMovement() { }

    public override void ApplyMovement()
    {
        // Note: The finisher movement is intended to only run once.
        // if (this.m_HasPlayedFinisherMove || !this.m_TargetEnemy) return;

        // This appears to be irrelevant to the movement.
        // Guarding _EnemyGuardSystem = this.m_TargetEnemy.GetComponent<AISystem>().eDamageController.enemyGuard;
        // Animator _EnemyAnimator = this.m_TargetEnemy.GetComponentInChildren<Animator>();
        //
        // _EnemyGuardSystem.bRunCooldownTimer = false;
        // _EnemyGuardSystem.bRunRecoveryTimer = false;
        // _EnemyGuardSystem.uiGuardMeter.HideFinisherKey();
        //
        // this.m_CollisionDetector.enabled = false;
        // damageController.DisableDamage();
        
        // _cutsceneDirector.Play();
        
        // Note: This is only a temporary solution
        // PlayerAttackState _PlayerAttackState = this.m_PlayerTransform.parent.GetComponent<IPlayerState>().PlayerAttackState;
        // _PlayerAttackState.CanAttack = false;
    }
    
    public override PlayerMovementStates GetState()
        => PlayerMovementStates.Finisher;

    public override void PerformDodge() { }

    public override void SetInputDirection(Vector2 inputDirection) { }
    
    // TODO: Change this to instead update if using LockOn
    // public void SetTargetEnemy(Animator enemy)
    // {
    //     BindToTrack("Animation Track (1)", enemy);
    //
    //     targetEnemy = enemy.gameObject;
    // }
    
    // public void PlayFinishingMove(GameObject enemy)
    // {
    //     // showFinisherTutorialEvent.Raise();
    //     // bIsFinishing = true;
    //     // Stop recovering guard process and hide finisher key
    //     Guarding guardScript = enemy.GetComponent<AISystem>().eDamageController.enemyGuard;
    //     guardScript.bRunCooldownTimer = false;
    //     guardScript.bRunRecoveryTimer = false;
    //     guardScript.uiGuardMeter.HideFinisherKey();
    //
    //     detector.SetActive(false);
    //     damageController.DisableDamage();
    //     
    //     // Note: This is only a temporary solution
    //     // // TODO: Remove this is no longer relevant as now movement is tied to its state
    //     // IPlayerMovement _PlayerMovement = this.m_PlayerTransform.parent.GetComponent<IPlayerMovement>();
    //     // _PlayerMovement.DisableMovement();
    //     //
    //     SetTargetEnemy(enemy.GetComponentInChildren<Animator>());
    //     // SelectFinishingMove();
    //     _cutsceneDirector.Play();
    //     
    //     // Note: This is only a temporary solution
    //     PlayerAttackState _PlayerAttackState = this.m_PlayerTransform.parent.GetComponent<IPlayerState>().PlayerAttackState;
    //     _PlayerAttackState.CanAttack = false;
    //     
    //     enemies = GameManager.instance.EnemyTracker.currentEnemies;
    // }
    
    // Note: This basically does nothing. It loads from the finishing move set.
    // public void SelectFinishingMove()
    // {
    //     PlayableAsset move = finishingMoves[Random.Range(0, finishingMoves.Length - 1)];
    // }
    //
    // TODO: Remove this to exist instead from the Player's attack controller.
    // - This should be seperated from the attack and movement state
    // public void KillEnemy()
    // {
    //     this.m_TargetEnemy.GetComponent<AISystem>().bFinish = true; // Belongs to enemy
    //     this.m_TargetEnemy.GetComponent<AISystem>().OnEnemyDeath(); // Belongs to enemy
    //     this.m_CollisionDetector.enabled = true;
    //     
    //     // Below can be removed.
    //     //GameManager.instance.RewindManager.IncreaseRewindAmount();
    //     // playerInputScript.bCanAttack = true;
    //     // playerInputScript.EnableMovement();
    //     // playerInputScript.bAlreadyAttacked = false;
    //     // playerInputScript.ResetAttack();
    //     
    //     // Note: This is only a temporary solution
    //     IPlayerMovement _PlayerMovement = this.m_PlayerTransform.parent.GetComponent<IPlayerMovement>();
    //     _PlayerMovement.EnableMovement();
    //     
    //     damageController.EnableDamage();
    //
    //     IPlayerAttackHandler _PlayerAttackHandler = this.m_PlayerTransform.parent.GetComponent<IPlayerAttackHandler>();
    //     _PlayerAttackHandler.ResetAttack();
    //     
    //     // bIsFinishing = false;
    // }
    //
    // // TODO: This is behaviour specific to the CutSceneDirector and not the FinishMovementController
    // private void BindToTrack(string trackName, Object val)
    // {
    //     foreach (var playableAssetOutput in _cutsceneDirector.playableAsset.outputs)
    //     {
    //         if (playableAssetOutput.streamName == trackName)
    //         {
    //             _cutsceneDirector.SetGenericBinding(playableAssetOutput.sourceObject, val);
    //             break;
    //         }
    //     }
    // }

    // Transform is passed instead of GameObject as LockOnSystem relies on transform over gameobject.
    // private void SetFinishingTargetEnemy(Transform targetEnemyTransform)
    // {
    //     // this.m_TargetEnemy = targetEnemyTransform.gameObject;
    //     // BindToTrack("Animation Track (1)", this.m_TargetEnemy);
    // }

    // Note: If necessary, an end state method can be applied to the BasePlayerMovementState class
    //      to handle transitioning between movement states.
    // private void ResetState()
    // {
    //     // this.m_TargetEnemy = null;
    //     // this.m_HasPlayedFinisherMove = false;
    // }
    
}
