using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickupCutscene : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        { 
            PickupSword();
            //Debug.Log(other.gameObject.name); 
        }
    }

    void PickupSword()
    {
        PlayerFunctions player = GameManager.instance.playerController.gameObject.GetComponent<PlayerFunctions>();
        //player.lSword.SetActive(true);
        player.rSword.SetActive(true);
        player.gameObject.GetComponent<PlayerInputScript>().bCanAttack = true;
        this.gameObject.SetActive(false);
    }

}
