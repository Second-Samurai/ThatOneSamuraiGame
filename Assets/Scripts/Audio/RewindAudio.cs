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

    public AudioSource heartSource;
    public AudioSource freezeSource;
    public AudioSource idleSource;
    public AudioSource resumeSource;
    public AudioSource deathSource;
    public BossThemeManager bossThemeManager;
    public float maxRewind;
    public bool played;



    // Start is called before the first frame update
    void Start()
    {
        played = false;
        _rewindManager = gameObject.GetComponent<RewindManager>();
        maxRewind = _rewindManager.maxRewindQuantity;
        audioManager = GameManager.instance.audioManager;
        audioPlayer = gameObject.GetComponent<AudioPlayer>();
        heartBeat = GameManager.instance.audioManager.FindSound("HeartBeatSlow");
        timeFreeze = GameManager.instance.audioManager.FindSound("freeze");
        timeResume = GameManager.instance.audioManager.FindSound("Restarts");
        idle = GameManager.instance.audioManager.FindSound("idle");
        Death = GameManager.instance.audioManager.FindSound("deathsfx");

        bossThemeManager = GameManager.instance.audioManager.BossThemeManager;
        
        heartSource.loop = true;
        heartSource.playOnAwake = false;
        heartSource.clip = heartBeat;

        freezeSource.loop = false;
        freezeSource.playOnAwake = false;
        freezeSource.clip = timeFreeze;

        idleSource.loop = true;
        idleSource.playOnAwake = false;
        idleSource.clip = idle;

        resumeSource.loop = false;
        resumeSource.playOnAwake = false;
        resumeSource.clip = timeResume;

        deathSource.loop = false;
        deathSource.playOnAwake = false;
        deathSource.clip = Death;

    }

    public void HeartBeat()
    {
        heartSource.volume = audioManager.SFXVol;
        heartSource.Play();
       // audioPlayer.PlayOnce(heartBeat, audioManager.SFXVol, 1f, 1f, true);
    }

    public void Idle()
    {
        idleSource.volume = audioManager.SFXVol;
        idleSource.Play();

    }

    public void Freeze()
    {
        bossThemeManager.PauseAll();
        freezeSource.volume = audioManager.SFXVol;
        freezeSource.Play();

    }

    public void Resume()
    {
        bossThemeManager.UnPauseAll();
        resumeSource.volume = audioManager.SFXVol;
        resumeSource.Play();

    }

    public void DeathSFX()
    {
        bossThemeManager.StopAll();
        deathSource.volume = audioManager.SFXVol;
        deathSource.Play();
    }

    public void StopSource() 
    {

        resumeSource.Stop();
        idleSource.Stop();
        freezeSource.Stop();
        if (_rewindManager.maxRewindQuantity <= 0) 
        {
            heartSource.Stop();
        }

    }

   

}
