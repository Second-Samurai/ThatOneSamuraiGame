using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class LRainTrigger : MonoBehaviour
{
    public GameObject rainParticles;
    private ColorAdjustments colorAdjustments;


    private void Awake()
    {
        rainParticles.SetActive(false);
    }

    public void Start()
    {
        GameManager.instance.postProcessingController.defaultVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        rainParticles.SetActive(true);
    }

    public void EndRain() 
    {
        rainParticles.SetActive(false);

        DOTween.To(() => colorAdjustments.postExposure.value, x => colorAdjustments.postExposure.value = x, 0, 5f);
    }
}
