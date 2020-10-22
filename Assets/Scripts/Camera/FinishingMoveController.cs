using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using Enemies;

public class FinishingMoveController : MonoBehaviour
{
    PlayableDirector _cutsceneDirector;

    public GameObject detector;

    public PlayableAsset[] finishingMoves;

    public PlayerInputScript playerInputScript;

    public GameEvent showFinisherTutorialEvent;

    GameObject targetEnemy;
    PDamageController damageController;
    List<Transform> enemies; 
    List<AISystem> enemiesCache;

    // Start is called before the first frame update
    void Start()
    {
        _cutsceneDirector = GetComponent<PlayableDirector>();
        BindToTrack("Cinemachine Track", GameManager.instance.mainCamera.GetComponent<CinemachineBrain>());
        damageController = GameManager.instance.playerController.gameObject.GetComponent<PDamageController>();
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
        detector.SetActive(false);
        damageController.DisableDamage();
        playerInputScript.DisableMovement();
        SetTargetEnemy(enemy.GetComponentInChildren<Animator>());
        SelectFinishingMove();
        _cutsceneDirector.Play();
        playerInputScript.bCanAttack = false;
        
        enemies = GameManager.instance.enemyTracker.currentEnemies;
        //Debug.LogError(enemies.Count);
        //for (int i = 0; i < enemies.Count-1; i++)
        //{
        //    enemiesCache[i] = enemies[i].GetComponent<AISystem>();
        //    enemies[i].GetComponent<AISystem>().OnEnemyRewind();
        //}
    }

    public void KillEnemy()
    {
        targetEnemy.GetComponent<AISystem>().OnEnemyDeath();
        detector.SetActive(true);
        GameManager.instance.rewindManager.IncreaseRewindAmount();
        playerInputScript.bCanAttack = true;
        playerInputScript.EnableMovement();
        damageController.EnableDamage();
        //for (int i = 0; i < enemies.Count - 1; i++)
        //{
        //    enemies[i].GetComponent<AISystem>().EnemyState = enemiesCache[i].EnemyState;
        //}
    }

   
}
