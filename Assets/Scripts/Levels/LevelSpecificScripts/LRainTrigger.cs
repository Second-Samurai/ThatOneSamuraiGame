using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRainTrigger : MonoBehaviour
{
    public GameObject rainParticles;

    private void Awake()
    {
        rainParticles.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        rainParticles.SetActive(true);
    }
}
