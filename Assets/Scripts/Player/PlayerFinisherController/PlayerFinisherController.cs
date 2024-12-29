using Cinemachine;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

public interface IFinisherController
{

    #region - - - - - - Methods - - - - - -

    void StartFinishingAction();

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

    public GameObject PlayerDetector; // Note: Used for enemies for detecting the player.

    private GameObject m_TargetEnemy;

    private void Start()
    {
        this.m_LockOnSystem.OnNewLockOnTarget.AddListener(this.SetFinishingTargetEnemy);
        
        CinemachineBrain _MainCameraCinemachineBrain = GameManager.instance.MainCamera.GetComponent<CinemachineBrain>();
        this.m_CutSceneDirector.BindToTrack("Cinemachine Track", _MainCameraCinemachineBrain);
    }

    void IFinisherController.StartFinishingAction()
    {
        this.PlayerDetector.SetActive(false);
        
        ((IPlayerMovement)this.m_PlayerMovement).SetState(PlayerMovementStates.Finisher);
        this.m_CameraController.SelectCamera(SceneCameras.FollowPlayer);
        this.m_CutSceneDirector.Play();
    }
    
    // Transform is passed instead of GameObject as LockOnSystem relies on transform over gameobject.
    private void SetFinishingTargetEnemy(Transform targetEnemyTransform)
    {
        this.m_TargetEnemy = targetEnemyTransform.gameObject;
        this.m_CutSceneDirector.BindToTrack("Animation Track (1)", this.m_TargetEnemy);
    }

    private void EndFinishingActionSequence()
    {
        this.PlayerDetector.SetActive(true);
        
        ((IPlayerMovement)this.m_PlayerMovement).EnableMovement();
        ((IPlayerAttackHandler)m_PlayerAttackHandler).ResetAttack();
        this.m_PlayerDamageController.EnableDamage();
    }
}
