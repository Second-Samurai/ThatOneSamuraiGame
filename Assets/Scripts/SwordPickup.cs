using System.Collections;
using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    public enum WeaponType
    {
        Katana,
        Lightsaber
    }

    #region - - - - - - Fields - - - - - -

    public WeaponType weaponType;

    private PlayerAttackState m_PlayerAttackState;

    #endregion Fields

    #region - - - - - - Lifecycle Methods - - - - - -

    private void Start() 
        => this.m_PlayerAttackState = GameManager.instance.PlayerController
                                        .GetComponent<IPlayerState>()
                                        .PlayerAttackState;

    #endregion Lifecycle Methods

    #region - - - - - - Methods - - - - - -

    /// <summary>
    /// Sets gameobject to player's swordmanager
    /// </summary>
    private void PickupSword(ISwordManager swordManager, GameObject holder)
    {
        if(weaponType == WeaponType.Katana)
        {
            swordManager.SetWeapon(true, GameManager.instance.gameSettings.katanaPrefab);
            AudioManager.instance.LightSaber = false;
        }
        else if (weaponType == WeaponType.Lightsaber)
        {
            swordManager.SetWeapon(true, GameManager.instance.gameSettings.laserSword);
            this.gameObject.SetActive(false);
            AudioManager.instance.LightSaber = true;
            return;
        }
        
        ICombatController playerCombatController = holder.GetComponent<ICombatController>();
        playerCombatController.DrawSword();

        this.m_PlayerAttackState.CanAttack = true;
        
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickupSword(other.GetComponent<ISwordManager>(), other.gameObject);
        }
    }

    #endregion Methods
    
}
