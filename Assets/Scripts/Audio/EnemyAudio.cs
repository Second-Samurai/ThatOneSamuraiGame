using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private AudioManager audioManager;
    public AudioPlayer audioPlayer;
    private AudioClip grassStomp;
    private AudioClip bowDraw;
    private AudioClip bowRelease;
    private AudioClip smoke;

    private AudioClip[] grunts;
    public AudioClip[] dyingSounds;
    private AudioClip[] armourBreakSounds;
    private AudioClip[] whoosh;
    private AudioClip[] jump;
    private AudioClip[] heavyStep;
    private AudioClip[] shing;
    private AudioClip[] heavySwing;
    private AudioClip[] taunt;


    private float min;
    private float minLow;

    // Start is called before the first frame update
    void Start()
    {
        //audioPlayer = gameObject.GetComponent<AudioPlayer>();
        audioManager = GameManager.instance.audioManager;
        grunts = GameManager.instance.audioManager.FindAll("grunt").ToArray();
        dyingSounds = GameManager.instance.audioManager.FindAll("dying").ToArray();
        armourBreakSounds = GameManager.instance.audioManager.FindAll("Break").ToArray();
        whoosh = GameManager.instance.audioManager.FindAll("woosh").ToArray();
        jump = GameManager.instance.audioManager.FindAll("Loud").ToArray();
        heavyStep = GameManager.instance.audioManager.FindAll("Loudish").ToArray();
        shing = GameManager.instance.audioManager.FindAll("heavy attack hit").ToArray();
        heavySwing = GameManager.instance.audioManager.FindAll("Heavy attack swing").ToArray();
        taunt = GameManager.instance.audioManager.FindAll("taunt").ToArray();

        grassStomp = GameManager.instance.audioManager.FindSound("Full");
        bowDraw = GameManager.instance.audioManager.FindSound("bowdraw");
        bowRelease = GameManager.instance.audioManager.FindSound("bowrelease");
        smoke = GameManager.instance.audioManager.FindSound("Smoke");

        min = Random.Range(.7f, 1);
        minLow = Random.Range(.5f, .7f);

    }

    private void Grunt() 
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, grunts.Length);
        audioPlayer.PlayOnce(grunts[i], audioManager.SFXVol, min, 1f);
    }

    private void GruntLow()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, grunts.Length);
        audioPlayer.PlayOnce(grunts[i], audioManager.SFXVol, minLow, .7f);
    }

    private void Dying()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, dyingSounds.Length);
        audioPlayer.PlayOnce(dyingSounds[i], audioManager.SFXVol, min, 1f);
    }

    private void DyingLow()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, dyingSounds.Length);
        audioPlayer.PlayOnce(dyingSounds[i], audioManager.SFXVol, minLow, .7f);
    }

    public void ArmourBreak()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, armourBreakSounds.Length);
        audioPlayer.PlayOnce(armourBreakSounds[i], audioManager.SFXVol, minLow, .7f);
    }

    public void Woosh()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, whoosh.Length);
        audioPlayer.PlayOnce(whoosh[i], audioManager.SFXVol, .5f, .8f);
    }

    public void Shing()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, shing.Length);
        audioPlayer.PlayOnce(shing[i], audioManager.SFXVol / 4, .8f, .8f);
    }

    public void GrassStomp()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        audioPlayer.PlayOnce(grassStomp, audioManager.SFXVol / 4);
    }

    public void Draw()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        audioPlayer.PlayOnce(bowDraw, audioManager.SFXVol * 3f, 1, 1);
    }
    public void Release()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        audioPlayer.PlayOnce(bowRelease, audioManager.SFXVol * 3f, 1, 1);
    }

    public void Step()
    {
        int i = Random.Range(0, heavyStep.Length);
        if (audioPlayer.rSources[audioPlayer.activeSource].clip == heavyStep[i])
        {
            audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;
        }
        audioPlayer.PlayOnce(heavyStep[i], audioManager.SFXVol / 1.5f);
    }

    public void LoudStep()
    {
        int i = Random.Range(0, jump.Length);
        if (audioPlayer.rSources[audioPlayer.activeSource].clip == jump[i]) 
        {
            audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;
        }
        audioPlayer.PlayOnce(jump[i], audioManager.SFXVol);
    }

    public void Jump()
    {
        int i = Random.Range(0, jump.Length);
        if (audioPlayer.rSources[audioPlayer.activeSource].clip == jump[i])
        {
            audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 1;
        }
        audioPlayer.PlayOnce(jump[i], audioManager.SFXVol * .5f);
    }

    public void Land()
    {
        int i = Random.Range(0, jump.Length);
        if (audioPlayer.rSources[audioPlayer.activeSource].clip == jump[i])
        {
            audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 1;
        }
        audioPlayer.PlayOnce(jump[i], audioManager.SFXVol, .5f, .7f);
    }

    public void Heavy()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;
        int i = Random.Range(0, heavySwing.Length);
        audioPlayer.PlayOnce(heavySwing[i], audioManager.SFXVol, .5f, .7f);
    }

    public void Smoke()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;
        audioPlayer.PlayOnce(smoke, audioManager.SFXVol, .5f, .5f);
    }

    public void Taunt() 
    {
        int i = Random.Range(0, taunt.Length);
        audioPlayer.PlayOnce(taunt[i], audioManager.SFXVol * 3);
    }
}
