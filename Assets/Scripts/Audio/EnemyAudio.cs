using UnityEngine;

public class EnemyAudio : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public AudioPlayer audioPlayer;
    public AudioClip[] dyingSounds;

    private AudioManager m_AudioManager;

    private AudioClip grassStomp;
    private AudioClip bowDraw;
    private AudioClip bowRelease;
    private AudioClip smoke;
    
    private AudioClip[] grunts;
    private AudioClip[] armourBreakSounds;
    private AudioClip[] whoosh;
    private AudioClip[] jump;
    private AudioClip[] heavyStep;
    private AudioClip[] shing;
    private AudioClip[] heavySwing;
    private AudioClip[] taunt;
    
    private float min;
    private float minLow;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    void Start()
    {
        this.m_AudioManager = AudioManager.instance;
        
        grunts = this.m_AudioManager.FindAll("grunt").ToArray();
        dyingSounds = this.m_AudioManager.FindAll("dying").ToArray();
        armourBreakSounds = this.m_AudioManager.FindAll("Break").ToArray();
        whoosh = this.m_AudioManager.FindAll("woosh").ToArray();
        jump = this.m_AudioManager.FindAll("Loud").ToArray();
        heavyStep = this.m_AudioManager.FindAll("Loudish").ToArray();
        shing = this.m_AudioManager.FindAll("heavy attack hit").ToArray();
        heavySwing = this.m_AudioManager.FindAll("Heavy attack swing").ToArray();
        taunt = this.m_AudioManager.FindAll("taunt").ToArray();

        grassStomp = this.m_AudioManager.FindSound("Full");
        bowDraw = this.m_AudioManager.FindSound("bowdraw");
        bowRelease = this.m_AudioManager.FindSound("bowrelease");
        smoke = this.m_AudioManager.FindSound("Smoke");

        min = Random.Range(.7f, 1);
        minLow = Random.Range(.5f, .7f);
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    private void Grunt() 
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, grunts.Length);
        audioPlayer.PlayOnce(grunts[i], this.m_AudioManager.SFXVol, min, 1f);
    }

    private void GruntLow()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, grunts.Length);
        audioPlayer.PlayOnce(grunts[i], this.m_AudioManager.SFXVol, minLow, .7f);
    }

    private void Dying()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, dyingSounds.Length);
        audioPlayer.PlayOnce(dyingSounds[i], this.m_AudioManager.SFXVol, min, 1f);
    }

    private void DyingLow()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, dyingSounds.Length);
        audioPlayer.PlayOnce(dyingSounds[i], this.m_AudioManager.SFXVol, minLow, .7f);
    }

    public void ArmourBreak()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, armourBreakSounds.Length);
        audioPlayer.PlayOnce(armourBreakSounds[i], this.m_AudioManager.SFXVol, minLow, .7f);
    }

    public void Woosh()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, whoosh.Length);
        audioPlayer.PlayOnce(whoosh[i], this.m_AudioManager.SFXVol, .5f, .8f);
    }

    public void Shing()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        int i = Random.Range(0, shing.Length);
        audioPlayer.PlayOnce(shing[i], this.m_AudioManager.SFXVol / 4, .8f, .8f);
    }

    public void GrassStomp()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        audioPlayer.PlayOnce(grassStomp, this.m_AudioManager.SFXVol / 4);
    }

    public void Draw()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        audioPlayer.PlayOnce(bowDraw, this.m_AudioManager.SFXVol * 3f, 1, 1);
    }
    public void Release()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;

        audioPlayer.PlayOnce(bowRelease, this.m_AudioManager.SFXVol * 3f, 1, 1);
    }

    public void Step()
    {
        int i = Random.Range(0, heavyStep.Length);
        if (audioPlayer.rSources[audioPlayer.activeSource].clip == heavyStep[i])
        {
            audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;
        }
        audioPlayer.PlayOnce(heavyStep[i], this.m_AudioManager.SFXVol / 1.5f);
    }

    public void LoudStep()
    {
        int i = Random.Range(0, jump.Length);
        if (audioPlayer.rSources[audioPlayer.activeSource].clip == jump[i]) 
        {
            audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;
        }
        audioPlayer.PlayOnce(jump[i], this.m_AudioManager.SFXVol);
    }

    public void Jump()
    {
        int i = Random.Range(0, jump.Length);
        if (audioPlayer.rSources[audioPlayer.activeSource].clip == jump[i])
        {
            audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 1;
        }
        audioPlayer.PlayOnce(jump[i], this.m_AudioManager.SFXVol * .5f);
    }

    public void Land()
    {
        int i = Random.Range(0, jump.Length);
        if (audioPlayer.rSources[audioPlayer.activeSource].clip == jump[i])
        {
            audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 1;
        }
        audioPlayer.PlayOnce(jump[i], this.m_AudioManager.SFXVol, .5f, .7f);
    }

    public void Heavy()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;
        int i = Random.Range(0, heavySwing.Length);
        audioPlayer.PlayOnce(heavySwing[i], this.m_AudioManager.SFXVol, .5f, .7f);
    }

    public void Smoke()
    {
        audioPlayer.rSources[audioPlayer.activeSource].spatialBlend = 0;
        audioPlayer.PlayOnce(smoke, this.m_AudioManager.SFXVol, .5f, .5f);
    }

    public void Taunt() 
    {
        int i = Random.Range(0, taunt.Length);
        audioPlayer.PlayOnce(taunt[i], this.m_AudioManager.SFXVol * 3);
    }

    #endregion Methods
  
}
