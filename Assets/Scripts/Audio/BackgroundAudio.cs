using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioClip birdAndTrees;
    public AudioClip menuMusic;
    public AudioClip optionsSelect;
    public AudioClip startGame;
    public AudioClip backgroudMusic;
    public AudioClip doorClose;
    public AudioClip doorSlam;
    public AudioClip saberHum;


    public AudioSource birdsAndTreesSource;
    public AudioSource menuMusicSource;
    public AudioSource optionsSelectSource;
    public AudioSource backgroundMusicSource;
    public AudioSource doorSource;
    public AudioSource hum;

    public bool bActive;
   



    // Start is called before the first frame update
    void Start()
    {
        bActive = true;
        audioManager = gameObject.GetComponent<AudioManager>();
        //birdsAndTreesSource = gameObject.GetComponent<AudioSource>();
        //menuMusicSource = gameObject.GetComponent<AudioSource>();
        //optionsSelectSource = gameObject.GetComponent<AudioSource>();
        //backgroundMusicSource = gameObject.GetComponent<AudioSource>();


        menuMusic = GameManager.instance.audioManager.FindSound("Menu");
        birdAndTrees = GameManager.instance.audioManager.FindSound("Birds");
        startGame = GameManager.instance.audioManager.FindSound("selectbuttonsfx");
        optionsSelect = GameManager.instance.audioManager.FindSound("scrollingsfx");
        backgroudMusic = GameManager.instance.audioManager.FindSound("background music");
        doorClose = GameManager.instance.audioManager.FindSound("gate open");
        doorSlam = GameManager.instance.audioManager.FindSound("shut");
        saberHum = GameManager.instance.audioManager.FindSound("hum");

        // audiosource settings
        menuMusicSource.loop = true;
        birdsAndTreesSource.loop = true;
        backgroundMusicSource.playOnAwake = false;
        backgroundMusicSource.loop = true;

        //audiosource clips
        if (!menuMusicSource.clip) menuMusicSource.clip = menuMusic;
        if(!birdsAndTreesSource.clip) birdsAndTreesSource.clip = birdAndTrees;
        if (!backgroundMusicSource.clip) backgroundMusicSource.clip = backgroudMusic;
        birdsAndTreesSource.Play();
        menuMusicSource.Play();

        doorSource.loop = false;

    }

    // Update is called once per frame
    void Update()
    {
        birdsAndTreesSource.volume = audioManager.BGMVol;
        menuMusicSource.volume = audioManager.BGMVol;
        
        if (bActive)
        {
            backgroundMusicSource.volume = audioManager.BGMVol;
        }

        if (audioManager.LightSaber == true && audioManager.check == false) 
        {
            PlayHum();
            audioManager.check = true;
        }
    }

    public void PlayScore() 
    {
        backgroundMusicSource.Play();
    }

    public void FadeScore()
    {
        bActive = false;
        backgroundMusicSource.DOFade(0, 5);
    }


    public void PauseMenuMusic() 
    {
        menuMusicSource.Pause();
    }

    public void PlayMenuMusic()
    {
        backgroundMusicSource.Stop();
        birdsAndTreesSource.Stop();
        menuMusicSource.Stop();
        hum.Stop();
        menuMusicSource.Play();
    }

    public void PauseAllMusic()
    {
        backgroundMusicSource.Stop();
        birdsAndTreesSource.Stop();
        menuMusicSource.Stop();
        hum.Stop();
    }

    public void PauseMusic()
    {
        backgroundMusicSource.Pause();
        hum.Pause();
    }

    public void ResumeMusic() 
    {
        backgroundMusicSource.UnPause();
        hum.UnPause();
    }

    public void Select(AudioSource audioSource)
    {
        audioSource.PlayOneShot(optionsSelect, 1);
    }

    public void StartGameSelect(AudioSource audioSource)
    {
        audioSource.PlayOneShot(startGame, 1);
    }

    public void AtmosFadeOut(bool Fadein)
    { 
            if (Fadein)
            {
            backgroundMusicSource.DOFade(audioManager.BGMVol, 2);
            }
            else
            {
            backgroundMusicSource.DOFade(0, 2);
            }
    }

    public void AtmosFadeOut(bool Fadein, float time)
    {
        if (Fadein)
        {
            backgroundMusicSource.DOFade(audioManager.BGMVol, time);
        }
        else
        {
            backgroundMusicSource.DOFade(0, time);
        }
    }

    public void PlayClose() 
    {
        doorSource.clip = doorClose;
        doorSource.Play();
    
    }

    public void PlaySlam()
    {
        doorSource.clip = doorSlam;
        doorSource.Play();
    }

    public void PlayHum() 
    {
        hum.clip = saberHum;
        hum.volume = .1f;
        hum.loop = true;
        hum.Play();
    }
}
