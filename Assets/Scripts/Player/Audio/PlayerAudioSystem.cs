using UnityEngine;

public interface IPlayerAttackAudio
{

    #region - - - - - - Methods - - - - - -

    void PlaySlash();
    
    void PlayHit();
    
    void PlayHeavySwing();
    
    void PlayHeavyHit();
    
    void IgnoreNextSwordPlayerTrack();
    
    #endregion Methods
  
}

public class PlayerAudioSystem : MonoBehaviour, IPlayerAttackAudio
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] 
    [RequiredField] 
    private AudioPlayer m_GeneralAudioPlayer;
    
    [Header("Attack Audio")]
    [SerializeField]
    [RequiredField] 
    private AudioPlayer m_SwordAudioPlayer;
    [Space]
    [SerializeField] private AudioClip m_LightAttackSlashSound;
    [SerializeField] private AudioClip m_LightAttackHitSound;
    [SerializeField] private AudioClip m_HeavySlashSound;
    [SerializeField] private AudioClip m_HeavyHitSound;
    private AudioClip[] m_SaberWhooshSounds;
    private AudioClip[] m_LightSaberHitSounds;
    
    private AudioManager m_AudioManager;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_AudioManager = AudioManager.instance;
        this.m_LightSaberHitSounds = AudioManager.instance.FindAll("lightSaber-Slash").ToArray();
        this.m_SaberWhooshSounds = AudioManager.instance.FindAll("lightSaber-Swing ").ToArray();
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public void PlaySlash()
    {
        if (this.m_AudioManager.IsLightSaber)
        {
            int _SelectedIndex = Random.Range(0, this.m_SaberWhooshSounds.Length);
            this.m_SwordAudioPlayer.PlayOnce(this.m_SaberWhooshSounds[_SelectedIndex], this.m_AudioManager.SFXVol / 2);
            return;
        }
        
        if (!this.m_LightAttackSlashSound) 
            this.m_LightAttackSlashSound = AudioManager.instance.FindSound("Light Attack Swing 1");
        this.m_SwordAudioPlayer.PlayOnce(this.m_LightAttackSlashSound, this.m_AudioManager.SFXVol);
    }

    public void PlayHit()
    {
        if (this.m_AudioManager.IsLightSaber)
        {
            int _SelectedIndex = Random.Range(0, this.m_LightSaberHitSounds.Length);
            this.m_SwordAudioPlayer.PlayOnce(this.m_LightSaberHitSounds[_SelectedIndex], this.m_AudioManager.SFXVol / 2);
            return;
        }
        
        if (!this.m_LightAttackHitSound) 
            this.m_LightAttackHitSound = AudioManager.instance.FindSound("Light Attack Hit 1");
        this.m_GeneralAudioPlayer.PlayOnce(this.m_LightAttackHitSound, this.m_AudioManager.SFXVol);
    }

    public void PlayHeavySwing()
    {
        if (this.m_AudioManager.IsLightSaber)
        {
            int _SelectedIndex = Random.Range(0, this.m_SaberWhooshSounds.Length);
            this.m_SwordAudioPlayer.PlayOnce(this.m_SaberWhooshSounds[_SelectedIndex], this.m_AudioManager.SFXVol / 2, .5f, .7f);
            return;
        }
        
        if (!this.m_HeavySlashSound) 
            this.m_HeavySlashSound = AudioManager.instance.FindSound("Heavy Attack Swing 2");
        this.m_SwordAudioPlayer.PlayOnce(this.m_HeavySlashSound, this.m_AudioManager.SFXVol);
    }

    public void PlayHeavyHit()
    {
        if (this.m_AudioManager.IsLightSaber)
        {
            int _SelectedIndex = Random.Range(0, this.m_LightSaberHitSounds.Length);
            this.m_SwordAudioPlayer.PlayOnce(this.m_LightSaberHitSounds[_SelectedIndex], this.m_AudioManager.SFXVol * 2, .5f, .7f);
            return;
        }
        
        if (!this.m_HeavyHitSound) 
            this.m_HeavyHitSound = AudioManager.instance.FindSound("Light Attack Hit 3");
        this.m_GeneralAudioPlayer.PlayOnce(this.m_HeavyHitSound, this.m_AudioManager.SFXVol);
    }

    void IPlayerAttackAudio.IgnoreNextSwordPlayerTrack()
        => this.m_SwordAudioPlayer.bIgnoreNext = true;

    #endregion Methods

}
