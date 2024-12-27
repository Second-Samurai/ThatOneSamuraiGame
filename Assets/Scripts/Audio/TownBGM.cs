using UnityEngine;

public class TownBGM : MonoBehaviour
{

    private BackgroundAudio backgroundAudio;

    void Start()
    {
        backgroundAudio = AudioManager.instance.backgroundAudio;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            backgroundAudio.PlayFire();
        }
    }
}
