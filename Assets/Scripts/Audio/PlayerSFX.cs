using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioPlayer audioPlayer;
    private AudioClip walkingGrass;
    private AudioClip walkingPebble;
    private AudioClip walkingStone;
    private AudioClip walkingWood;
    private AudioClip joggingGrass;
    private AudioClip joggingPebble;
    private AudioClip joggingStone;
    private AudioClip joggingWood;
    [SerializeField]
    private AudioClip[] grassRoll;
    private AudioClip[] PebbleRoll;
    private AudioClip[] woodRoll;


    // Start is called before the first frame update

    public void Start()
    {
        walkingGrass = GameManager.instance.audioManager.FindSound("Walking Grass");
        walkingPebble = GameManager.instance.audioManager.FindSound("Walking Pebble");
        walkingStone = GameManager.instance.audioManager.FindSound("Walking Stone");
        walkingWood = GameManager.instance.audioManager.FindSound("Walking Wood");
        joggingGrass = GameManager.instance.audioManager.FindSound("Jogging Grass");
        joggingPebble = GameManager.instance.audioManager.FindSound("Jogging Pebble");
        joggingStone = GameManager.instance.audioManager.FindSound("Jogging Stone");
        joggingWood = GameManager.instance.audioManager.FindSound("Jogging Wood");
        grassRoll = GameManager.instance.audioManager.FindAll("Grass Roll").ToArray();
        PebbleRoll = GameManager.instance.audioManager.FindAll("Pebble Roll").ToArray();
        woodRoll = GameManager.instance.audioManager.FindAll("Wood Roll").ToArray();


    }

    // Update is called once per frame

    private void Step() 
    {
        audioPlayer.PlayOnce(walkingGrass);
    }

    private void Sprint()
    {
        audioPlayer.PlayOnce(walkingGrass, .5f, .8f);
    }

    private void Dodge()
    {
        int i = Random.Range(0, grassRoll.Length);
        audioPlayer.PlayOnce(grassRoll[i]);
        Debug.Log(i);
    }
}
