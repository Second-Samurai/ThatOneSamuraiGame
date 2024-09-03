using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using Enemies;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;

public class FinishingMoveController : MonoBehaviour
{
    PlayableDirector _cutsceneDirector;

    public GameObject detector;

    public PlayableAsset[] finishingMoves;

    // public PlayerInputScript playerInputScript;

    public GameEvent showFinisherTutorialEvent;

    GameObject targetEnemy;
    PDamageController damageController;
    List<Transform> enemies; 
    List<AISystem> enemiesCache;

    public bool bIsFinishing = false;

    // Start is called before the first frame update
    void Start()
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
        //for (int i = 0; i < enemies.Count-1; i++)
        //{
        //    enemiesCache[i] = enemies[i].GetComponent<AISystem>();
        //    enemies[i].GetComponent<AISystem>().OnEnemyRewind();
        //}
    }

    public void KillEnemy()
    {
        targetEnemy.GetComponent<AISystem>().bFinish = true;
        targetEnemy.GetComponent<AISystem>().OnEnemyDeath();
        detector.SetActive(true);
        GameManager.instance.RewindManager.IncreaseRewindAmount();
        // playerInputScript.bCanAttack = true;
        // playerInputScript.EnableMovement();
        // playerInputScript.bAlreadyAttacked = false;
        // playerInputScript.ResetAttack();
        
        // Note: This is only a temporary solution
        IPlayerMovement _PlayerMovement = this.transform.parent.GetComponent<IPlayerMovement>();
        _PlayerMovement.EnableMovement();
        
        damageController.EnableDamage();

        IPlayerAttackHandler _PlayerAttackHandler = this.transform.parent.GetComponent<IPlayerAttackHandler>();
        _PlayerAttackHandler.ResetAttack();
        
        bIsFinishing = false;
        
        //for (int i = 0; i < enemies.Count - 1; i++)
        //{
        //    enemies[i].GetComponent<AISystem>().EnemyState = enemiesCache[i].EnemyState;
        //}
    }

   
}
