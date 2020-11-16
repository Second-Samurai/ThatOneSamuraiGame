using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsAudio : MonoBehaviour
{
    private AudioManager audioManager;
    public AudioClip optionsSelect;
    public AudioClip startGame;

    public AudioSource audioSource;
    void Start()
    {
        audioManager = GameManager.instance.audioManager;
        audioSource = GameManager.instance.audioManager.backgroundAudio.optionsSelectSource;
        startGame = GameManager.instance.audioManager.FindSound("selectbuttonsfx");
        optionsSelect = GameManager.instance.audioManager.FindSound("scrollingsfx");
    }

    public void PlaySelect()
    {
        audioSource.clip = audioManager.backgroundAudio.startGame;
        audioSource.PlayOneShot(audioManager.backgroundAudio.startGame, 1);
    }

    public void ButtonSelect()
    {
        audioSource.clip = audioManager.backgroundAudio.optionsSelect;
        audioSource.PlayOneShot(audioManager.backgroundAudio.optionsSelect, 1);
    }

    public void PlayScore() 
    {
        audioManager.backgroundAudio.PlayScore();
        audioManager.backgroundAudio.PauseMenuMusic();
    }

}
