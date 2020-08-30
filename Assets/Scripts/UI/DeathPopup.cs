using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPopup : MonoBehaviour
{
    public GameObject popup;
    private int triggerCount = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && triggerCount <= 0)
        {
            triggerCount--;
            popup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            popup.SetActive(false);
    }
}
