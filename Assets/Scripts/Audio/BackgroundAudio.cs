using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioClip birdAndTrees;
    public AudioClip menuMusic;

    public AudioSource birdsAndTreesSource;
    public AudioSource menuMusicSource;


    // Start is called before the first frame update
    void Start()
    {
        audioManager = gameObject.GetComponent<AudioManager>();
        menuMusic = GameManager.instance.audioManager.FindSound("Menu");
        birdAndTrees = GameManager.instance.audioManager.FindSound("Birds");
        if(!menuMusicSource.clip) menuMusicSource.clip = menuMusic;
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
}
