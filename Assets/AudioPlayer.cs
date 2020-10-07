using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayOnce(AudioClip clip)
    {
        source.pitch = Random.Range(.65f, 1f);
        source.clip = clip;
        source.PlayOneShot(clip);
    }
}
