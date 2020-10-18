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
    private float min;
    private float minLow;

    // Start is called before the first frame update
    void Start()
    {
        //audioPlayer = gameObject.GetComponent<AudioPlayer>();
        audioManager = GameManager.instance.audioManager;
        grunts = GameManager.instance.audioManager.FindAll("grunt").ToArray();
        dyingSounds = GameManager.instance.audioManager.FindAll("dying").ToArray();
        armourBreakSounds = GameManager.instance.audioManager.FindAll("Armour").ToArray();

        min = Random.Range(.7f, 1);
        minLow = Random.Range(.5f, .7f);

    }

    private void Grunt() 
    {
        int i = Random.Range(0, grunts.Length);
        audioPlayer.PlayOnce(grunts[i], audioManager.SFXVol.value, min, 1f);
    }

    private void GruntLow()
    {
        int i = Random.Range(0, grunts.Length);
        audioPlayer.PlayOnce(grunts[i], audioManager.SFXVol.value, minLow, .7f);
    }

    private void Dying()
    {
        int i = Random.Range(0, dyingSounds.Length);
        audioPlayer.PlayOnce(dyingSounds[i], audioManager.SFXVol.value, min, 1f);
    }

    private void DyingLow()
    {
        int i = Random.Range(0, dyingSounds.Length);
        audioPlayer.PlayOnce(dyingSounds[i], audioManager.SFXVol.value, minLow, .7f);
    }

    public void ArmourBreak()
    {
        int i = Random.Range(0, dyingSounds.Length);
        audioPlayer.PlayOnce(dyingSounds[i], audioManager.SFXVol.value, minLow, .7f);
    }
}
