using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindAudio : MonoBehaviour
{
    [HideInInspector]
    private RewindManager _rewindManager;
    public AudioManager audioManager;
    private AudioClip heartBeat;
    private AudioPlayer audioPlayer;
    private AudioClip timeFreeze;
    private AudioClip timeResume;
    private AudioClip idle;
    private AudioClip Death;

    public float maxRewind;
    public bool played;


    // Start is called before the first frame update
    void Start()
    {
        played = false;
        _rewindManager = gameObject.GetComponent<RewindManager>();
        maxRewind = _rewindManager.maxRewindResource;
        audioManager = GameManager.instance.audioManager;
        audioPlayer = gameObject.GetComponent<AudioPlayer>();
        heartBeat = GameManager.instance.audioManager.FindSound("HeartBeatSlow");
        timeFreeze = GameManager.instance.audioManager.FindSound("freeze");
        timeResume = GameManager.instance.audioManager.FindSound("Restarts");
        idle = GameManager.instance.audioManager.FindSound("idle");
        Death = GameManager.instance.audioManager.FindSound("deathsfx");
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
        audioPlayer.PlayOnce(timeFreeze, audioManager.SFXVol, 1f, 1f, false);
    }

    public void Resume()
    {
        audioPlayer.PlayOnce(timeResume, audioManager.SFXVol, 1f, 1f, false);

    }

    public void DeathSFX()
    {
        //audioSource.clip = Death;
        Debug.Log("BABABOI");
        audioPlayer.PlayOnce(Death, 1, 1, 1, false);
    }

    public void StopSource() 
    {
 
            audioPlayer.rSources[audioPlayer.activeSource].loop = false;
            audioPlayer.StopSource();

    }

   

}
