using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    private AudioClip birdAndTrees;
    public AudioSource birdsAndTreesSource;

    // Start is called before the first frame update
    void Start()
    {
        birdAndTrees = GameManager.instance.audioManager.FindSound("Birds");
        birdsAndTreesSource.clip = birdAndTrees;
        birdsAndTreesSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
