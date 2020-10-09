using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindAudio : MonoBehaviour
{
    private AudioClip heartBeat;
    private AudioPlayer audioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = gameObject.GetComponent<AudioPlayer>();
        heartBeat = GameManager.instance.audioManager.FindSound("HeartBeatSlow");
        
    }

    public void HeartBeat()
    {
        for (int a = 0; a < audioPlayer.rSources.Length; a++) 
        {
            audioPlayer.rSources[a].loop = true;
        }
        audioPlayer.PlayOnce(heartBeat, 1f, 1f);
    }

    public void StopHeartBeat() 
    {
        for (int a = 0; a < audioPlayer.rSources.Length; a++)
        {
            audioPlayer.rSources[a].loop = false;
        }
        audioPlayer.StopSource();
    }
}
