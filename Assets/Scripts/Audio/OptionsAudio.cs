using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsAudio : MonoBehaviour
{
    public AudioClip optionsSelect;
    public AudioClip startGame;

    public AudioSource audioSource;
    void Start()
    {
        audioSource = GameManager.instance.audioManager.backgroundAudio.optionsSelectSource;
        startGame = GameManager.instance.audioManager.FindSound("selectbuttonsfx");
        optionsSelect = GameManager.instance.audioManager.FindSound("scrollingsfx");
    }

    public void PlaySelect()
    {
        audioSource.clip = startGame;
        audioSource.PlayOneShot(startGame, 1);
    }

    public void ButtonSelect()
    {
        audioSource.clip = optionsSelect;
        audioSource.PlayOneShot(optionsSelect, 1);
    }
}
