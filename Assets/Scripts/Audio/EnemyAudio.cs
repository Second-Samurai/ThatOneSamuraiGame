using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private AudioManager audioManager;
    public AudioPlayer audioPlayer;
    private AudioClip[] grunts;
    public AudioClip[] dyingSounds;
    private AudioClip[] armourBreakSounds;
    private AudioClip[] whoosh;
    private AudioClip grassStomp;
    private AudioClip[] jump;
    private AudioClip[] heavyStep;


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
        grassStomp = GameManager.instance.audioManager.FindSound("GrassStomp");

        min = Random.Range(.7f, 1);
        minLow = Random.Range(.5f, .7f);

    }

    private void Grunt() 
    {
        int i = Random.Range(0, grunts.Length);
        audioPlayer.PlayOnce(grunts[i], audioManager.SFXVol, min, 1f);
    }

    private void GruntLow()
    {
        int i = Random.Range(0, grunts.Length);
        audioPlayer.PlayOnce(grunts[i], audioManager.SFXVol, minLow, .7f);
    }

    private void Dying()
    {
        int i = Random.Range(0, dyingSounds.Length);
        audioPlayer.PlayOnce(dyingSounds[i], audioManager.SFXVol, min, 1f);
    }

    private void DyingLow()
    {
        int i = Random.Range(0, dyingSounds.Length);
        audioPlayer.PlayOnce(dyingSounds[i], audioManager.SFXVol, minLow, .7f);
    }

    public void ArmourBreak()
    {
        int i = Random.Range(0, armourBreakSounds.Length);
        audioPlayer.PlayOnce(armourBreakSounds[i], audioManager.SFXVol, minLow, .7f);
    }

    public void Woosh()
    {
        int i = Random.Range(0, whoosh.Length);
        audioPlayer.PlayOnce(whoosh[i], audioManager.SFXVol, .5f, .8f);
    }

    public void GrassStomp()
    {
        audioPlayer.PlayOnce(grassStomp, audioManager.SFXVol / 2);
    }

   

    public void Step()
    {
        int i = Random.Range(0, heavyStep.Length);
        audioPlayer.PlayOnce(heavyStep[i], audioManager.SFXVol / 1.5f);
    }

    public void LoudStep()
    {
        int i = Random.Range(0, jump.Length);
        audioPlayer.PlayOnce(jump[i], audioManager.SFXVol);
    }

    public void Jump()
    {
        int i = Random.Range(0, jump.Length);
        audioPlayer.PlayOnce(jump[i], audioManager.SFXVol);
    }
}
