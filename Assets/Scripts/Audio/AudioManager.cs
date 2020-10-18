using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public float BGMVol;
    public float SFXVol;
  


    public static AudioManager instance;
    // Start is called before the first frame update
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
            if (s.createSource == true)
            {
                CreateSource(s);
            }
            if (s.createSource == false) 
            {
                DestroyImmediate(s.source);
            }
        }

    }
    // update for debugging
 

    //finds and returns a  sound contaning a given string in its name

    public AudioClip FindSound(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name.Contains(name.ToLower().Trim().Replace(" ", "")));
        if (s == null) 
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
       // Debug.LogWarning(s.clip.name);
        return s.clip;
    }

    // finds and returns a list of every sound contaning a given string in its name
    public List<AudioClip> FindAll(string name) 
    {
        List<AudioClip> passOver = new List<AudioClip>();

        for (int s = 0;  s < sounds.Length; s++) 
        {
 
            sounds[s].clip.name = sounds[s].clip.name.ToLower().Trim().Replace(" ", "");
            //Debug.Log(sounds[s].clip.name);
            if (sounds[s].clip.name.Contains(name.ToLower().Trim().Replace(" ", "")))
            {
                passOver.Add(sounds[s].clip) ;
            }
        }
        if (passOver.Count == 0) 
        {
           // Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
          //  Debug.LogWarning( "length " + passOver.Count);
           return passOver;     
    }

    // finds and plays a  sound contaning a given string in its name
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
}
