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
        _trackManager = AudioManager.instance.trackManager;
        _backgroundAudio = AudioManager.instance.GetComponent<BackgroundAudio>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _trackManager.FadeOutAll();
            _backgroundAudio.PlayScore();
            GameManager.instance.EnemyTracker.bAtVillage = true;
        }
    }
}
