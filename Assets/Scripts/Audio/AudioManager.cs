using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    

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

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds) 
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.name = s.clip.name;
        }

    }
    // update for debugging
    //public void Update()
    //{
    //    if (Keyboard.current.bKey.wasPressedThisFrame) 
    //    {
    //        FindAll("wood Roll");
    //        FindAll("PebbleRoll");

    //    }
    //}


    public AudioClip FindSound(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name.Contains(name));
        if (s == null) 
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s.clip;
    }

    public List<Sound> FindAll(string name) 
    {
        List<Sound> passOver = new List<Sound>();

        for (int s = 0;  s < sounds.Length; s++) 
        {
 
            sounds[s].clip.name = sounds[s].clip.name.ToLower().Trim().Replace(" ", "");
            Debug.Log(sounds[s].clip.name);
            if (sounds[s].clip.name.Contains(name.ToLower().Trim().Replace(" ", "")))
            {
                passOver.Add(sounds[s]) ;
            }
        }

        for (int s = 0; s < passOver.Count; s++)
        {

            Debug.LogWarning(passOver[s].clip + "length " + passOver.Count);
           return passOver;
        }
        Debug.LogWarning("Sound: " + name + " not found!");
        return null;
    }
}
