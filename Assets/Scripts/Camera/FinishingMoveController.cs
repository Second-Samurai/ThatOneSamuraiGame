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

    GameObject targetEnemy;

    List<Transform> enemies; 
    List<AISystem> enemiesCache;

    // Start is called before the first frame update
    void Start()
    {
        _cutsceneDirector = GetComponent<PlayableDirector>();
        BindToTrack("Cinemachine Track", GameManager.instance.mainCamera.GetComponent<CinemachineBrain>());
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
        detector.SetActive(false);
        playerInputScript.DisableMovement();
        SetTargetEnemy(enemy.GetComponentInChildren<Animator>());
        SelectFinishingMove();
        _cutsceneDirector.Play();
        
        enemies = GameManager.instance.enemyTracker.currentEnemies;
        for (int i = 0; i < enemies.Count-1; i++)
        {
            enemiesCache[i] = enemies[i].gameObject.GetComponent<AISystem>();
            enemies[i].gameObject.GetComponent<AISystem>().OnEnemyRewind();
        }
    }

    public void KillEnemy()
    {
        targetEnemy.GetComponent<AISystem>().OnEnemyDeath();
        detector.SetActive(true);
        GameManager.instance.rewindManager.IncreaseRewindAmount();
        playerInputScript.EnableMovement();
        for (int i = 0; i < enemies.Count - 1; i++)
        {
            enemies[i].gameObject.GetComponent<AISystem>().EnemyState = enemiesCache[i].EnemyState;
        }
    }

   
}
