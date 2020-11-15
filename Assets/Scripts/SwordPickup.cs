using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    public enum WeaponType
    {
        Katana,
        Lightsaber
    }

    public WeaponType weaponType;

    /// <summary>
    /// Sets gameobject to player's swordmanager
    /// </summary>
    private void PickupSword(ISwordManager swordManager, GameObject holder)
    {
        if(weaponType == WeaponType.Katana)
        {
            swordManager.SetWeapon(true, GameManager.instance.gameSettings.katanaPrefab);
        }
        else if (weaponType == WeaponType.Lightsaber)
        {
            swordManager.SetWeapon(true, GameManager.instance.gameSettings.laserSword);
            this.gameObject.SetActive(false);
            return;
        }
        
        ICombatController playerCombatController = holder.GetComponent<ICombatController>();
        playerCombatController.DrawSword();

        PlayerFunctions player = GameManager.instance.playerController.gameObject.GetComponent<PlayerFunctions>();
        player.gameObject.GetComponent<PlayerInputScript>().bCanAttack = true; //TODO: CHANGE: input not incharge of attacking (combat controller maybe doing it already)
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickupSword(other.GetComponent<ISwordManager>(), other.gameObject);
        }
    }
}
