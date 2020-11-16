using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBGM : MonoBehaviour
{

    private BackgroundAudio backgroundAudio;

    void Start()
    {
        backgroundAudio = GameManager.instance.audioManager.backgroundAudio;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            backgroundAudio.PlayFire();
            Debug.Log("FIRE");
        }
    }
}
