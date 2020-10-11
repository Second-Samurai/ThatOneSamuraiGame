using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [HideInInspector]
    public AudioSource[] rSources;
    public int activeSource = 0;
    public bool bIgnoreNext = false;
    // Start is called before the first frame update
    void Start()
    {
        rSources = GetComponents<AudioSource>();
    }

    public void PlayOnce(AudioClip clip)
    {
        if (!bIgnoreNext)
        {
            if (rSources[activeSource].isPlaying)
            {
                activeSource++;
                if (activeSource > rSources.Length - 1) activeSource = 0;
            }

            rSources[activeSource].pitch = Random.Range(.65f, 1f);
            rSources[activeSource].clip = clip;
            rSources[activeSource].Play();
        }
        else bIgnoreNext = false;
    }

    public void PlayOnce(AudioClip clip, float min, float max)
    {
        if (!bIgnoreNext)
        {
            if (rSources[activeSource].isPlaying)
            {
                activeSource++;
                if (activeSource > rSources.Length - 1) activeSource = 0;
            }

            rSources[activeSource].pitch = Random.Range(min, max);
            rSources[activeSource].clip = clip;
            rSources[activeSource].Play();
        }
        else bIgnoreNext = false;
    }

    public void PlayOnce(AudioClip clip, float min, float max, bool looping)
    {
        if (!bIgnoreNext)
        {
            if (rSources[activeSource].isPlaying)
            {
                activeSource++;
                if (activeSource > rSources.Length - 1) activeSource = 0;
            }
            if (looping == true) rSources[activeSource].loop = true;
            else rSources[activeSource].loop = false;


            rSources[activeSource].pitch = Random.Range(min, max);
            rSources[activeSource].clip = clip;
            rSources[activeSource].Play();
        }
        else bIgnoreNext = false;
    }

    public void StopSource()
    {
        rSources[activeSource].Stop();
    }
}
