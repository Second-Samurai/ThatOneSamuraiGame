using UnityEngine;

public class StopTracks : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.backgroundAudio.FadeScore();
        }
    }
}
