using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popop : MonoBehaviour
{
    public GameObject popup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            popup.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            popup.SetActive(false);
    }
}
