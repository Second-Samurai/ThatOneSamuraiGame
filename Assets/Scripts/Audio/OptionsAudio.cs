using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsAudio : MonoBehaviour
{
    private BackgroundAudio backgroundAudio;
    public AudioClip optionsSelect;
    public AudioClip startGame;

    public AudioSource audioSource;
    void Start()
    {
        backgroundAudio = GameManager.instance.audioManager.backgroundAudio;
        audioSource = GameManager.instance.audioManager.backgroundAudio.optionsSelectSource;
        startGame = GameManager.instance.audioManager.FindSound("selectbuttonsfx");
        optionsSelect = GameManager.instance.audioManager.FindSound("scrollingsfx");
    }

    public void PlaySelect()
    {
        audioSource.clip = backgroundAudio.startGame;
        audioSource.PlayOneShot(backgroundAudio.startGame, 1);
    }

    public void ButtonSelect()
    {
        audioSource.clip = backgroundAudio.optionsSelect;
        audioSource.PlayOneShot(backgroundAudio.optionsSelect, 1);
    }

    public void PlayScore() 
    {
        backgroundAudio.PlayScore();
    }
}
