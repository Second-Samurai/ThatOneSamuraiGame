using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioPlayer audioPlayer;
    private AudioClip walkingGrass;
    private AudioClip walkingPebble;
    private AudioClip walkingStone;
    private AudioClip walkingWood;

    [SerializeField]
    private AudioClip[] grassRoll;
    private AudioClip[] PebbleRoll;
    private AudioClip[] woodRoll;
    private AudioClip[] parry;



    // Start is called before the first frame update

    public void Start()
    {
        audioManager = GameManager.instance.audioManager;
        walkingGrass = GameManager.instance.audioManager.FindSound("Walking Grass");
        walkingPebble = GameManager.instance.audioManager.FindSound("Walking Pebble");
        walkingStone = GameManager.instance.audioManager.FindSound("Walking Stone");
        walkingWood = GameManager.instance.audioManager.FindSound("Walking Wood");
       
        grassRoll = GameManager.instance.audioManager.FindAll("Grass Roll").ToArray();
        PebbleRoll = GameManager.instance.audioManager.FindAll("Pebble Roll").ToArray();
        woodRoll = GameManager.instance.audioManager.FindAll("Wood Roll").ToArray();

        parry = GameManager.instance.audioManager.FindAll("Parry").ToArray();



    }

    // Update is called once per frame

    private void Step() 
    {
        audioPlayer.PlayOnce(walkingGrass, audioManager.SFXVol);
    }

    private void Sprint()
    {
        audioPlayer.PlayOnce(walkingGrass, audioManager.SFXVol, .5f, .8f);
    }

    private void Dodge()
    {
        int i = Random.Range(0, grassRoll.Length);
        audioPlayer.PlayOnce(grassRoll[i], audioManager.SFXVol);
        //Debug.Log(i);
    }
    private void Parry()
    {
        int i = Random.Range(0, parry.Length);
        audioPlayer.PlayOnce(parry[i], audioManager.SFXVol);
        //Debug.Log(i);
    }

}
