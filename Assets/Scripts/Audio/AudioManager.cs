using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public static AudioManager instance;
    
    public Sound[] sounds;
    public float BGMVol;
    public float SFXVol;
    public BackgroundAudio backgroundAudio;
    public TrackManager trackManager;
    public bool IsLightSaber = false;
    public bool check = false;

    [FormerlySerializedAs("m_BossThemeManager")]
    public BossThemeManager BossThemeManager;

    #endregion Fields

    #region - - - - - - Unity Lifecycle Methods - - - - - -

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds) 
        {
            s.name = s.clip.name.ToLower().Trim().Replace(" ", "");
            if (s.createSource == true) CreateSource(s);
            if (s.createSource == false) DestroyImmediate(s.source);
        }
        
        SFXVol = PlayerPrefs.GetFloat("SFXVolume");
        BGMVol = PlayerPrefs.GetFloat("BGMVolume");
        backgroundAudio = gameObject.GetComponent<BackgroundAudio>();
        trackManager = gameObject.GetComponentInChildren<TrackManager>();
    }

    #endregion Unity Lifecycle Methods

    #region - - - - - - Methods - - - - - -

    /// <summary>
    /// finds and returns a sound containing a given string in its name
    /// </summary>
    public AudioClip FindSound(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name.Contains(name.ToLower().Trim().Replace(" ", "")));
        if (s == null) 
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s.clip;
    }

    /// <summary>
    /// finds and returns a list of every sound containing a given string in its name
    /// </summary>
    public List<AudioClip> FindAll(string name) 
    {
        List<AudioClip> passOver = new List<AudioClip>();
        for (int s = 0;  s < sounds.Length; s++) 
        {
            sounds[s].clip.name = sounds[s].clip.name.ToLower().Trim().Replace(" ", "");
            if (sounds[s].clip.name.Contains(name.ToLower().Trim().Replace(" ", "")))
                passOver.Add(sounds[s].clip) ;
        }
        if (passOver.Count == 0) 
            return null;
        
        return passOver;     
    }

    /// <summary>
    /// finds and plays a  sound containing a given string in its name
    /// </summary>
    public void Play(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name.Contains(name.ToLower().Trim().Replace(" ", "")));
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (s.source == null) 
        {
            CreateSource(s);
            s.source.Play();
        }
        s.source.Play();
    }

    private void CreateSource(Sound s) 
    {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.playOnAwake = false;

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
    }

    #endregion Methods
  
}
