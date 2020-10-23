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
    private AudioClip armour;
    private AudioClip takeoff;
    public AudioClip walkingClip;

    [SerializeField]
    private AudioClip[] grassRoll;
    private AudioClip[] PebbleRoll;
    private AudioClip[] woodRoll;
    private AudioClip[] parry;
    private AudioClip[] armourJingle;
    private AudioClip[] whoosh;
    private AudioClip[] bigSmack;






    // Start is called before the first frame update

    public void Start()
    {
        audioManager = GameManager.instance.audioManager;
        walkingGrass = GameManager.instance.audioManager.FindSound("Walking Grass");
        walkingPebble = GameManager.instance.audioManager.FindSound("Walking Pebble");
        walkingStone = GameManager.instance.audioManager.FindSound("Walking Stone");
        walkingWood = GameManager.instance.audioManager.FindSound("Walking Wood");
        armour = GameManager.instance.audioManager.FindSound("armour jingle 3");
        takeoff = GameManager.instance.audioManager.FindSound("Loudish Stomp 1 SFX");


        grassRoll = GameManager.instance.audioManager.FindAll("Grass Roll").ToArray();
        PebbleRoll = GameManager.instance.audioManager.FindAll("Pebble Roll").ToArray();
        woodRoll = GameManager.instance.audioManager.FindAll("Wood Roll").ToArray();

        parry = GameManager.instance.audioManager.FindAll("Parry").ToArray();

        armourJingle = GameManager.instance.audioManager.FindAll("armour").ToArray();

        whoosh = GameManager.instance.audioManager.FindAll("woosh").ToArray();

        bigSmack = GameManager.instance.audioManager.FindAll("Very").ToArray();

        walkingClip = walkingGrass;

    }

    // Update is called once per frame

    private void Step() 
    {
        audioPlayer.PlayOnce(walkingClip, audioManager.SFXVol);
    }

    private void Sprint()
    {
        audioPlayer.PlayOnce(walkingGrass, audioManager.SFXVol, .5f, .8f);
    }

    public void Armour() 
    {
        int j = Random.Range(0, armourJingle.Length);
        audioPlayer.PlayOnce(armourJingle[j], audioManager.SFXVol);
    }

    private void Whoosh()
    {
        int j = Random.Range(0, whoosh.Length);
        audioPlayer.PlayOnce(whoosh[j], audioManager.SFXVol *2);
    }

    private void Jump()
    {
        audioPlayer.PlayOnce(takeoff, audioManager.SFXVol*2);
    }

    private void Dodge()
    {
        int i = Random.Range(0, grassRoll.Length);
        int j = Random.Range(0, armourJingle.Length);
        audioPlayer.PlayOnce(armourJingle[j], audioManager.SFXVol/2);
        audioPlayer.PlayOnce(grassRoll[i], audioManager.SFXVol);
        //Debug.Log(i);
    }
    private void Parry()
    {
        int i = Random.Range(0, parry.Length);
        audioPlayer.PlayOnce(parry[i], audioManager.SFXVol);
        //Debug.Log(i);
    }


    public void Smack()
    {
        int i = Random.Range(0, bigSmack.Length);
        audioPlayer.PlayOnce(bigSmack[i], audioManager.SFXVol / 4);
    }
}
