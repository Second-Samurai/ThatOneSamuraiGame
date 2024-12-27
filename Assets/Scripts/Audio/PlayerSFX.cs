using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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
    public AudioClip[] rollArray;
    private AudioClip unsheath;

    [SerializeField]
    public AudioClip[] grassRoll;
    public AudioClip[] PebbleRoll;
    public AudioClip[] woodRoll;
    private AudioClip[] parry;
    private AudioClip[] armourJingle;
    private AudioClip[] whoosh;
    private AudioClip[] bigSmack;
    private AudioClip[] allUnsheath;
    private AudioClip[] saberwhoosh;







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
        unsheath = GameManager.instance.audioManager.FindSound("sheath 2");


        grassRoll = GameManager.instance.audioManager.FindAll("Grass Roll").ToArray();
        PebbleRoll = GameManager.instance.audioManager.FindAll("Pebble Roll").ToArray();
        woodRoll = GameManager.instance.audioManager.FindAll("Wood Roll").ToArray();
        parry = GameManager.instance.audioManager.FindAll("Parry").ToArray();
        armourJingle = GameManager.instance.audioManager.FindAll("armour").ToArray();
        whoosh = GameManager.instance.audioManager.FindAll("woosh").ToArray();
        bigSmack = GameManager.instance.audioManager.FindAll("Very").ToArray();
        allUnsheath = GameManager.instance.audioManager.FindAll("sheath ").ToArray();

        walkingClip = walkingGrass;

    }

    // Update is called once per frame

    private void Step() 
    {
        audioPlayer.PlayOnce(walkingClip, audioManager.SFXVol);
    }

    private void Sprint()
    {
        audioPlayer.PlayOnce(walkingClip, audioManager.SFXVol, .5f, .8f);
    }

    public void Armour() 
    {
        int j = Random.Range(0, armourJingle.Length);
        audioPlayer.PlayOnce(armourJingle[j], audioManager.SFXVol);
    }

    private void Whoosh()
    {
        if (audioManager.LightSaber == false)
        {
            int j = Random.Range(0, whoosh.Length);
            audioPlayer.PlayOnce(whoosh[j], audioManager.SFXVol * 2);
        }
       
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
        audioPlayer.PlayOnce(rollArray[i], audioManager.SFXVol);
    }
    private void Parry()
    {
        int i = Random.Range(0, parry.Length);
        audioPlayer.PlayOnce(parry[i], audioManager.SFXVol);
    }


    public void Smack()
    {
        int i = Random.Range(0, bigSmack.Length);
        audioPlayer.PlayOnce(bigSmack[i], audioManager.SFXVol / 4);
    }

    public void Unsheath() 
    {
        audioPlayer.PlayOnce(unsheath, audioManager.SFXVol);
    }

    public void AllUnsheath()
    {
        int i = Random.Range(0, allUnsheath.Length);
        audioPlayer.PlayOnce(allUnsheath[i], audioManager.SFXVol);
    }
}
