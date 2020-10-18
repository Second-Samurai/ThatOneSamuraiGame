using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindAudio : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;
    private AudioClip heartBeat;
    private AudioPlayer audioPlayer;
    private AudioClip timeFreeze;
    private AudioClip timeResume;
    private AudioClip idle;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameManager.instance.audioManager;
        audioPlayer = gameObject.GetComponent<AudioPlayer>();
        heartBeat = GameManager.instance.audioManager.FindSound("HeartBeatSlow");
        timeFreeze = GameManager.instance.audioManager.FindSound("freeze");
        timeResume = GameManager.instance.audioManager.FindSound("Restarts");
        idle = GameManager.instance.audioManager.FindSound("idle");

    }

    public void HeartBeat()
    {
        audioPlayer.PlayOnce(heartBeat, audioManager.SFXVol, 1f, 1f, true);
    }

    public void Idle()
    {
        audioPlayer.PlayOnce(idle, audioManager.SFXVol, 1f, 1f, true);
    }

    public void Freeze()
    {
        audioPlayer.PlayOnce(timeFreeze, audioManager.SFXVol, 1f, 1f);
    }

    public void Resume()
    {
        audioPlayer.PlayOnce(timeResume, audioManager.SFXVol, 1f, 1f);
    }

    public void StopSource() 
    {
        audioPlayer.rSources[audioPlayer.activeSource].loop = false;
        audioPlayer.StopSource();
    }


}
