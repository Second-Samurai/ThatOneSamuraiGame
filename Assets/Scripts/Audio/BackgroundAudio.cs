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


    public AudioSource birdsAndTreesSource;
    public AudioSource menuMusicSource;
    public AudioSource optionsSelectSource;
    public AudioSource backgroundMusicSource;
   



    // Start is called before the first frame update
    void Start()
    {
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

    }

    // Update is called once per frame
    void Update()
    {
        birdsAndTreesSource.volume = audioManager.BGMVol;
        menuMusicSource.volume = audioManager.BGMVol;
        backgroundMusicSource.volume = audioManager.BGMVol;

    }

    public void PlayScore() 
    {
        backgroundMusicSource.Play();
        menuMusicSource.Pause();
    }



    public void PauseMusic()
    {
        backgroundMusicSource.Pause();
    }

    public void ResumeMusic() 
    {
        backgroundMusicSource.UnPause();
    }

    public void Select(AudioSource audioSource)
    {
        audioSource.PlayOneShot(optionsSelect, 1);
    }

    public void StartGameSelect(AudioSource audioSource)
    {
        audioSource.PlayOneShot(startGame, 1);
    }
}
