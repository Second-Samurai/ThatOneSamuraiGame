using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageFade : MonoBehaviour
{
    [SerializeField]
    private TrackManager _trackManager;
    [SerializeField]
    private BackgroundAudio _backgroundAudio;
    // Start is called before the first frame update
    void Start()
    {
        _trackManager = GameManager.instance.audioManager.trackManager;
        _backgroundAudio = GameManager.instance.audioManager.GetComponent<BackgroundAudio>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _trackManager.FadeOutAll();
            _backgroundAudio.PlayScore();
            GameManager.instance.enemyTracker.bAtVillage = true;
        }
    }
}
