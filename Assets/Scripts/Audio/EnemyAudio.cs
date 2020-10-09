using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioPlayer audioPlayer;
    private AudioClip[] grunts;
    private AudioClip[] dyingSounds;
    private float min;
    private float minLow;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = gameObject.GetComponent<AudioPlayer>();
        grunts = GameManager.instance.audioManager.FindAll("grunt").ToArray();
        dyingSounds = GameManager.instance.audioManager.FindAll("dying").ToArray();

        min = Random.Range(.7f, 1);
        minLow = Random.Range(.5f, .7f);

    }

    private void Grunt() 
    {
        int i = Random.Range(0, grunts.Length);
        audioPlayer.PlayOnce(grunts[i], min, 1f);
    }

    private void GruntLow()
    {
        int i = Random.Range(0, grunts.Length);
        audioPlayer.PlayOnce(grunts[i], minLow, .7f);
    }

    private void Dying()
    {
        int i = Random.Range(0, dyingSounds.Length);
        audioPlayer.PlayOnce(dyingSounds[i], min, 1f);
    }

    private void DyingLow()
    {
        int i = Random.Range(0, dyingSounds.Length);
        audioPlayer.PlayOnce(dyingSounds[i], minLow, .7f);
    }
}
