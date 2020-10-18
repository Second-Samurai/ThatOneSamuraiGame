using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindAudio : MonoBehaviour
{
    private AudioManager audioManager;
    private AudioClip heartBeat;
    private AudioPlayer audioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameManager.instance.audioManager;
        audioPlayer = gameObject.GetComponent<AudioPlayer>();
        heartBeat = GameManager.instance.audioManager.FindSound("HeartBeatSlow");
        
    }

    public void HeartBeat()
    {
        audioPlayer.PlayOnce(heartBeat, audioManager.SFXVol.value, 1f, 1f, true);
    }

    public void StopHeartBeat() 
    {
        audioPlayer.rSources[audioPlayer.activeSource].loop = false;
        audioPlayer.StopSource();
    }
}
