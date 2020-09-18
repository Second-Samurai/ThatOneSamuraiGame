using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayerPit : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerFunctions>().KillPlayer();
        }
           // other.gameObject.transform.position = respawnPoint.position;
    }
 
}
