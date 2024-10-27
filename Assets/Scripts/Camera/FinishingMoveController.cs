using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using Enemies;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;

public class FinishingMoveController : MonoBehaviour
{
    
    #region - - - - - - Fields - - - - - -

    public GameObject detector;
    public PlayableAsset[] finishingMoves;
    public GameEvent showFinisherTutorialEvent;
    public bool bIsFinishing = false;

    PlayableDirector _cutsceneDirector;
    PDamageController damageController;
    List<Transform> enemies; 
    List<AISystem> enemiesCache;
    GameObject targetEnemy;

    #endregion Fields

    #region - - - - - - Unity Lifecycle Methods - - - - - -

    // void Start()
    // {
    //     _cutsceneDirector = GetComponent<PlayableDirector>();
    //     BindToTrack("Cinemachine Track", GameManager.instance.MainCamera.GetComponent<CinemachineBrain>());
    //     damageController = GameManager.instance.PlayerController.gameObject.GetComponent<PDamageController>();
    // }

    #endregion Unity Lifecycle Methods

    #region - - - - - - Methods - - - - - -

    public void Initialise()
    {
        _cutsceneDirector = GetComponent<PlayableDirector>();
        BindToTrack("Cinemachine Track", GameManager.instance.MainCamera.GetComponent<CinemachineBrain>());
        damageController = GameManager.instance.PlayerController.gameObject.GetComponent<PDamageController>();
    }

    void BindToTrack(string trackName, Object val)
    {
        foreach (var playableAssetOutput in _cutsceneDirector.playableAsset.outputs)
        {
            if (playableAssetOutput.streamName == trackName)
            {
                _cutsceneDirector.SetGenericBinding(playableAssetOutput.sourceObject, val);
                break;
            }
        }
    }

    public void SetTargetEnemy(Animator enemy)
    {
        BindToTrack("Animation Track (1)", enemy);
        targetEnemy = enemy.gameObject;
    }

    public void SelectFinishingMove()
    {
        PlayableAsset move = finishingMoves[Random.Range(0, finishingMoves.Length - 1)];
    }

    public void PlayFinishingMove(GameObject enemy)
    {
        showFinisherTutorialEvent.Raise();
        bIsFinishing = true;
        // Stop recovering guard process and hide finisher key
        Guarding guardScript = enemy.GetComponent<AISystem>().eDamageController.enemyGuard;
        guardScript.bRunCooldownTimer = false;
        guardScript.bRunRecoveryTimer = false;
        guardScript.uiGuardMeter.HideFinisherKey();

        detector.SetActive(false);
        damageController.DisableDamage();
        
        // Note: This is only a temporary solution
        IPlayerMovement _PlayerMovement = this.transform.parent.GetComponent<IPlayerMovement>();
        _PlayerMovement.DisableMovement();
        
        SetTargetEnemy(enemy.GetComponentInChildren<Animator>());
        SelectFinishingMove();
        _cutsceneDirector.Play();
        
        // Note: This is only a temporary solution
        PlayerAttackState _PlayerAttackState = this.transform.parent.GetComponent<IPlayerState>().PlayerAttackState;
        _PlayerAttackState.CanAttack = false;
        
        enemies = GameManager.instance.EnemyTracker.currentEnemies;
    }

    public void KillEnemy()
    {
        targetEnemy.GetComponent<AISystem>().bFinish = true;
        targetEnemy.GetComponent<AISystem>().OnEnemyDeath();
        detector.SetActive(true);
        GameManager.instance.RewindManager.IncreaseRewindAmount();
        
        // Note: This is only a temporary solution
        IPlayerMovement _PlayerMovement = this.transform.parent.GetComponent<IPlayerMovement>();
        _PlayerMovement.EnableMovement();
        
        damageController.EnableDamage();

        IPlayerAttackHandler _PlayerAttackHandler = this.transform.parent.GetComponent<IPlayerAttackHandler>();
        _PlayerAttackHandler.ResetAttack();
        
        bIsFinishing = false;
    }

    #endregion Methods
  
}
