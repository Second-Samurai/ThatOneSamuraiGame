using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BossThemeManager : MonoBehaviour
{
    public AudioSource opening;
    public AudioSource bassLoop;
    public AudioSource drumloop;

    public AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameManager.instance.audioManager.GetComponent<AudioManager>();
        BackgroundAudio audio = AudioManager.instance.gameObject.GetComponent<BackgroundAudio>();
        audio.PauseAllMusic();
        StartOpening();
        Invoke("StartBassLoop", 4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (opening.volume != audioManager.BGMVol)
        {
            opening.volume = audioManager.BGMVol * 3f;
            bassLoop.volume = audioManager.BGMVol * 3f;
            drumloop.volume = audioManager.BGMVol * 3f;
        }
    }

    void StartOpening()
    {
        opening.Play();
    }

    void StartBassLoop() 
    {
        bassLoop.Play();
    }

    public void DrumLoop() 
    {
        bassLoop.Stop();
        opening.Stop();
        drumloop.Play();
    }
    public void StopAll()
    {
        bassLoop.Stop();
        opening.Stop();
        drumloop.Stop();
    }

    public void PauseAll() 
    {
        bassLoop.Pause();
        opening.Pause();
        drumloop.Pause();
    }

    public void UnPauseAll()
    {
        bassLoop.UnPause();
        opening.UnPause();
        drumloop.UnPause();
    }

}
