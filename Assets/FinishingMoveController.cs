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

    public PlayableAsset[] finishingMoves;

    GameObject targetEnemy;

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
        SetTargetEnemy(enemy.GetComponentInChildren<Animator>());
        SelectFinishingMove();
        _cutsceneDirector.Play();
    }

    public void KillEnemy()
    {
        targetEnemy.GetComponent<AISystem>().OnEnemyDeath();
    }

   
}
