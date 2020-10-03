using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickup : MonoBehaviour
{

    private void PickupSword(ISwordManager swordManager)
    {
        PlayerFunctions player = GameManager.instance.playerController.gameObject.GetComponent<PlayerFunctions>();
        //player.lSword.SetActive(true);
        //player.rSword.SetActive(true);

        swordManager.SetWeapon(true, GameManager.instance.gameSettings.katanaPrefab);



        player.gameObject.GetComponent<PlayerInputScript>().bCanAttack = true; //TODO: CHANGE: input not incharge of attacking (combat controller maybe doing it already)
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickupSword(other.GetComponent<ISwordManager>());
            //Debug.Log(other.gameObject.name); 
        }
    }

}
