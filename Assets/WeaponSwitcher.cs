using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public MeshRenderer swordHand, swordSheath;
    public GameObject bowHand, bowBack;
    public MeshRenderer glaiveHand, glaiveSheath;

    public void EnableSword(bool val)
    {
        swordHand.enabled = val;
        swordSheath.enabled = !val;
    }

    public void EnableGlaive(bool val)
    {
        glaiveHand.enabled = val;
        glaiveSheath.enabled = !val;
    }

    public void EnableBow(bool val)
    {
        bowHand.SetActive(val);
        bowBack.SetActive(!val);
    }
}
