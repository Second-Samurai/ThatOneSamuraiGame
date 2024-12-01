using UnityEngine;

public class OptionsAudio : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public AudioSource audioSource;
    public AudioClip optionsSelect;
    public AudioClip startGame;

    private AudioManager audioManager;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    void Start()
    {
        audioManager = AudioManager.instance;
        audioSource = AudioManager.instance.backgroundAudio.optionsSelectSource;
        startGame = AudioManager.instance.FindSound("selectbuttonsfx");
        optionsSelect = AudioManager.instance.FindSound("scrollingsfx");
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

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

    #endregion Methods

}
