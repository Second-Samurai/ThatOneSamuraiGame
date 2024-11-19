using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    private AudioManager m_AudioManager;
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


    public void Start()
    {
        this.m_AudioManager = AudioManager.instance;
        
        walkingGrass = this.m_AudioManager.FindSound("Walking Grass");
        walkingPebble = this.m_AudioManager.FindSound("Walking Pebble");
        walkingStone = this.m_AudioManager.FindSound("Walking Stone");
        walkingWood = this.m_AudioManager.FindSound("Walking Wood");
        armour = this.m_AudioManager.FindSound("armour jingle 3");
        takeoff = this.m_AudioManager.FindSound("Loudish Stomp 1 SFX");
        unsheath = this.m_AudioManager.FindSound("sheath 2");


        grassRoll = this.m_AudioManager.FindAll("Grass Roll").ToArray();
        PebbleRoll = this.m_AudioManager.FindAll("Pebble Roll").ToArray();
        woodRoll = this.m_AudioManager.FindAll("Wood Roll").ToArray();
        parry = this.m_AudioManager.FindAll("Parry").ToArray();
        armourJingle = this.m_AudioManager.FindAll("armour").ToArray();
        whoosh = this.m_AudioManager.FindAll("woosh").ToArray();
        bigSmack = this.m_AudioManager.FindAll("Very").ToArray();
        allUnsheath = this.m_AudioManager.FindAll("sheath ").ToArray();

        walkingClip = walkingGrass;

    }

    // Update is called once per frame

    private void Step() 
    {
        audioPlayer.PlayOnce(walkingClip, this.m_AudioManager.SFXVol);
    }

    private void Sprint()
    {
        audioPlayer.PlayOnce(walkingClip, this.m_AudioManager.SFXVol, .5f, .8f);
    }

    public void Armour() 
    {
        int j = Random.Range(0, armourJingle.Length);
        audioPlayer.PlayOnce(armourJingle[j], this.m_AudioManager.SFXVol);
    }

    private void Whoosh()
    {
        if (this.m_AudioManager.LightSaber == false)
        {
            int j = Random.Range(0, whoosh.Length);
            audioPlayer.PlayOnce(whoosh[j], this.m_AudioManager.SFXVol * 2);
        }
       
    }

    private void Jump()
    {
        audioPlayer.PlayOnce(takeoff, this.m_AudioManager.SFXVol*2);
    }

    private void Dodge()
    {
        int i = Random.Range(0, grassRoll.Length);
        int j = Random.Range(0, armourJingle.Length);
        audioPlayer.PlayOnce(armourJingle[j], this.m_AudioManager.SFXVol/2);
        audioPlayer.PlayOnce(rollArray[i], this.m_AudioManager.SFXVol);
    }
    private void Parry()
    {
        int i = Random.Range(0, parry.Length);
        audioPlayer.PlayOnce(parry[i], this.m_AudioManager.SFXVol);
    }


    public void Smack()
    {
        int i = Random.Range(0, bigSmack.Length);
        audioPlayer.PlayOnce(bigSmack[i], this.m_AudioManager.SFXVol / 4);
    }

    public void Unsheath() 
    {
        audioPlayer.PlayOnce(unsheath, this.m_AudioManager.SFXVol);
    }

    public void AllUnsheath()
    {
        int i = Random.Range(0, allUnsheath.Length);
        audioPlayer.PlayOnce(allUnsheath[i], this.m_AudioManager.SFXVol);
    }
}
