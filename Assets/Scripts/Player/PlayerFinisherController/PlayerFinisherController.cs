using Cinemachine;
using Enemies;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

public interface IFinisherController
{

    #region - - - - - - Methods - - - - - -

    void StartFinishingAction();

    void SetFinishingTargetEnemy(Transform targetEnemyTransform);

    #endregion Methods

}

/// <summary>
/// Facade for handling 'Finishing' action related behaviour. Its responsible for handling finishing sequences against
/// bladed enemies.
/// </summary>
public class PlayerFinisherController : PausableMonoBehaviour, IFinisherController
{
    [RequiredField] public PlayerMovement m_PlayerMovement;
    [RequiredField] public PlayerFinisherCutsceneDirector m_CutSceneDirector;
    [RequiredField] public LockOnSystem m_LockOnSystem;
    [RequiredField] public PDamageController m_PlayerDamageController;
    [RequiredField] public PlayerAttackHandler m_PlayerAttackHandler;
    [RequiredField] public CameraController m_CameraController;
    public GameObject m_PlayerObject;

    public GameObject PlayerDetector; // Note: Used for enemies for detecting the player.

    private GameObject m_TargetEnemy;

    private void Start()
    {
        this.m_LockOnSystem.OnNewLockOnTarget.AddListener(((IFinisherController)this).SetFinishingTargetEnemy);
        
        CinemachineBrain _MainCameraCinemachineBrain = GameManager.instance.MainCamera.GetComponent<CinemachineBrain>();
        this.m_CutSceneDirector.BindToTrack("Cinemachine Track", _MainCameraCinemachineBrain);
    }

    void IFinisherController.StartFinishingAction()
    {
        this.PlayerDetector.SetActive(false);

        if (this.m_TargetEnemy)
        {
            Guarding _EnemyGuardSystem = this.m_TargetEnemy.GetComponent<AISystem>().eDamageController.enemyGuard;
            _EnemyGuardSystem.bRunCooldownTimer = false;
            _EnemyGuardSystem.bRunRecoveryTimer = false;
            _EnemyGuardSystem.uiGuardMeter.HideFinisherKey();

            this.m_PlayerDamageController.DisableDamage();
            
            // Note: This is only a temporary solution
            PlayerAttackState _PlayerAttackState = this.m_PlayerObject.transform.parent
                .GetComponent<IPlayerState>().PlayerAttackState;
            _PlayerAttackState.CanAttack = false;
        }
        
        ((IPlayerMovement)this.m_PlayerMovement).SetState(PlayerMovementStates.Finisher);
        this.m_CameraController.SelectCamera(SceneCameras.FollowPlayer);
        this.m_CutSceneDirector.Play();
    }
    
    // Transform is passed instead of GameObject as LockOnSystem relies on transform over gameobject.
    void IFinisherController.SetFinishingTargetEnemy(Transform targetEnemyTransform)
    {
        this.m_TargetEnemy = targetEnemyTransform.gameObject;
        this.m_CutSceneDirector.BindToTrack("Animation Track (1)", this.m_TargetEnemy);
    }

    public void EndFinishingActionSequence()
    {
        this.PlayerDetector.SetActive(true);
        
        ((IPlayerMovement)this.m_PlayerMovement).EnableMovement();
        ((IPlayerAttackHandler)m_PlayerAttackHandler).ResetAttack();
        this.m_PlayerDamageController.EnableDamage();
    }
}
