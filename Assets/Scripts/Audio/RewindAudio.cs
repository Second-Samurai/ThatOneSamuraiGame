using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindAudio : MonoBehaviour
{ 
    private AudioManager m_AudioManager;
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
        this.m_AudioManager = AudioManager.instance;
        
        played = false;
        
        audioPlayer = gameObject.GetComponent<AudioPlayer>();
        heartBeat = this.m_AudioManager.FindSound("HeartBeatSlow");
        timeFreeze = this.m_AudioManager.FindSound("freeze");
        timeResume = this.m_AudioManager.FindSound("Restarts");
        idle = this.m_AudioManager.FindSound("idle");
        Death = this.m_AudioManager.FindSound("deathsfx");

        bossThemeManager = this.m_AudioManager.BossThemeManager;
        
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
        heartSource.volume = this.m_AudioManager.SFXVol;
        heartSource.Play(); 
    }

    public void Idle()
    {
        idleSource.volume = this.m_AudioManager.SFXVol;
        idleSource.Play(); 
    }

    public void Freeze()
    {
        bossThemeManager.PauseAll();
        freezeSource.volume = this.m_AudioManager.SFXVol;
        freezeSource.Play(); 
    }

    public void Resume()
    {
        bossThemeManager.UnPauseAll();
        resumeSource.volume = this.m_AudioManager.SFXVol;
        resumeSource.Play(); 
    }

    public void DeathSFX()
    {
        bossThemeManager.StopAll();
        deathSource.volume = this.m_AudioManager.SFXVol;
        deathSource.Play();
    }

    public void StopSource() 
    { 
        resumeSource.Stop();
        idleSource.Stop();
        freezeSource.Stop();  
    } 
}
