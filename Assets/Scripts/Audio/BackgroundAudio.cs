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


    public AudioSource birdsAndTreesSource;
    public AudioSource menuMusicSource;
    public AudioSource optionsSelectSource;
   



    // Start is called before the first frame update
    void Start()
    {
        audioManager = gameObject.GetComponent<AudioManager>();
        menuMusic = GameManager.instance.audioManager.FindSound("Menu");
        birdAndTrees = GameManager.instance.audioManager.FindSound("Birds");
        startGame = GameManager.instance.audioManager.FindSound("selectbuttonsfx");
        optionsSelect = GameManager.instance.audioManager.FindSound("scrollingsfx");

        if (!menuMusicSource.clip) menuMusicSource.clip = menuMusic;
        if(!birdsAndTreesSource.clip) birdsAndTreesSource.clip = birdAndTrees;
        birdsAndTreesSource.Play();
        menuMusicSource.Play();

    }

    // Update is called once per frame
    void Update()
    {
        birdsAndTreesSource.volume = audioManager.BGMVol;
        menuMusicSource.volume = audioManager.BGMVol;

    }

    public void PauseMusic()
    {
        menuMusicSource.Pause();
    }

    public void ResumeMusic() 
    {
        menuMusicSource.UnPause();
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
