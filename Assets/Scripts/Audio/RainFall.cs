using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;



public class RainFall : MonoBehaviour
{

    private BackgroundAudio backgroundAudio;
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        backgroundAudio = GameManager.instance.audioManager.backgroundAudio;
        GameManager.instance.postProcessingController.defaultVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            backgroundAudio.PlayRain();
            DOTween.To(() => colorAdjustments.postExposure.value, x => colorAdjustments.postExposure.value = x, -1.5f, 5f);


        }
    }

}
