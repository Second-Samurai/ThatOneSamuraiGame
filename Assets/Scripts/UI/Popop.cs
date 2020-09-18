using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Popop : MonoBehaviour
{
    public GameObject popup;
    public Image popupImage;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            popup.SetActive(true);
            popupImage.DOFade(1, 1.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            popup.SetActive(false);
        }
    }
}
