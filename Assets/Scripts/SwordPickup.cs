using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    /// <summary>
    /// Sets gameobject to player's swordmanager
    /// </summary>
    private void PickupSword(ISwordManager swordManager)
    {
        swordManager.SetWeapon(true, GameManager.instance.gameSettings.katanaPrefab);

        PlayerFunctions player = GameManager.instance.playerController.gameObject.GetComponent<PlayerFunctions>();
        player.gameObject.GetComponent<PlayerInputScript>().bCanAttack = true; //TODO: CHANGE: input not incharge of attacking (combat controller maybe doing it already)
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickupSword(other.GetComponent<ISwordManager>());
        }
    }
}
