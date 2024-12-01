using DG.Tweening;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public AudioClip birdAndTrees;
    public AudioClip menuMusic;
    public AudioClip optionsSelect;
    public AudioClip startGame;
    public AudioClip backgroudMusic;
    public AudioClip doorClose;
    public AudioClip doorSlam;
    public AudioClip saberHum;
    public AudioClip fire;
    public AudioClip rain;
    public AudioClip[] thunder;


    public AudioSource birdsAndTreesSource;
    public AudioSource menuMusicSource;
    public AudioSource optionsSelectSource;
    public AudioSource backgroundMusicSource;
    public AudioSource doorSource;
    public AudioSource hum;
    public AudioSource ThunderSource;

    public bool bActive;

    private AudioManager m_AudioManager;


    // Start is called before the first frame update
    void Start()
    {
        bActive = true;
        AudioManager _AudioManager = gameObject.GetComponent<AudioManager>();
        this.m_AudioManager = _AudioManager;
        //birdsAndTreesSource = gameObject.GetComponent<AudioSource>();
        //menuMusicSource = gameObject.GetComponent<AudioSource>();
        //optionsSelectSource = gameObject.GetComponent<AudioSource>();
        //backgroundMusicSource = gameObject.GetComponent<AudioSource>();

        menuMusic = _AudioManager.FindSound("Menu");
        birdAndTrees = _AudioManager.FindSound("Birds");
        startGame = _AudioManager.FindSound("selectbuttonsfx");
        optionsSelect = _AudioManager.FindSound("scrollingsfx");
        backgroudMusic = _AudioManager.FindSound("background music");
        doorClose = _AudioManager.FindSound("gate open");
        doorSlam = _AudioManager.FindSound("shut");
        saberHum = _AudioManager.FindSound("hum");
        fire = _AudioManager.FindSound("fire");
        rain = _AudioManager.FindSound("Rainfall W thunder");

        thunder = _AudioManager.FindAll("thunder").ToArray();

        // audiosource settings
        menuMusicSource.loop = true;
        birdsAndTreesSource.loop = true;
        backgroundMusicSource.playOnAwake = false;
        backgroundMusicSource.loop = true;

        //audiosource clips
        if (!menuMusicSource.clip) menuMusicSource.clip = menuMusic;
        if (!birdsAndTreesSource.clip) birdsAndTreesSource.clip = birdAndTrees;
        if (!backgroundMusicSource.clip) backgroundMusicSource.clip = backgroudMusic;
        birdsAndTreesSource.Play();
        menuMusicSource.Play();

        doorSource.loop = false;

    }

    // Update is called once per frame
    void Update()
    {
        birdsAndTreesSource.volume = this.m_AudioManager.BGMVol;
        menuMusicSource.volume = this.m_AudioManager.BGMVol;
        doorSource.volume = this.m_AudioManager.BGMVol;
        hum.volume = this.m_AudioManager.SFXVol / 4;

        if (bActive)
        {
            backgroundMusicSource.volume = this.m_AudioManager.BGMVol;
        }

        if (this.m_AudioManager.LightSaber == true && this.m_AudioManager.check == false)
        {
            PlayHum();
            this.m_AudioManager.check = true;
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

    // public void ResumeMusic()
    // {
    //     backgroundMusicSource.UnPause();
    //     hum.UnPause();
    // }
    //
    // public void Select(AudioSource audioSource)
    // {
    //     audioSource.PlayOneShot(optionsSelect, 1);
    // }

    // public void StartGameSelect(AudioSource audioSource)
    // {
    //     audioSource.PlayOneShot(startGame, 1);
    // }
    //
    // public void AtmosFadeOut(bool Fadein)
    // {
    //     if (Fadein)
    //     {
    //         backgroundMusicSource.DOFade(this.m_AudioManager.BGMVol, 2);
    //     }
    //     else
    //     {
    //         backgroundMusicSource.DOFade(0, 2);
    //     }
    // }

    // public void AtmosFadeOut(bool Fadein, float time)
    // {
    //     if (Fadein)
    //     {
    //         backgroundMusicSource.DOFade(this.m_AudioManager.BGMVol, time);
    //     }
    //     else
    //     {
    //         backgroundMusicSource.DOFade(0, time);
    //     }
    // }

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
        hum.volume = .01f;
        hum.loop = true;
        hum.Play();
    }

    public void PlayFire()
    {
        birdsAndTreesSource.clip = fire;
        birdsAndTreesSource.Play();

    }

    public void PlayRain()
    {
        birdsAndTreesSource.clip = rain;
        birdsAndTreesSource.Play();
    }

    public void PlayThunder()
    {
        int j = Random.Range(0, thunder.Length);
        ThunderSource.PlayOneShot(thunder[j], 1f);
    }

    public void PlayBirds()
    {
        birdsAndTreesSource.clip = birdAndTrees;
        birdsAndTreesSource.Play();
    }

}
