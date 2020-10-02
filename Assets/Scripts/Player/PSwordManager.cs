using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwordManager
{
    void SetWeapon(bool hasWeapon, GameObject swordPrefab);
}

public class PSwordManager : MonoBehaviour, ISwordManager
{
    [HideInInspector] public bool _hasAWeapon = false; //Must check this with an actual gameobject

    private WSwordEffect _swordEffect;
    private GameObject _swordObject;

    public void Init(GameObject swordEffect)
    {
        //this._swordEffect = swordEffect;
        this._swordObject = swordEffect.transform.gameObject;
    }

    public void SetWeapon(bool hasWeapon, GameObject swordPrefab)
    {
        this._hasAWeapon = hasWeapon;
    }

    /// <summary>
    /// Reveals sword when drawn or when needed
    /// </summary>
    public void RevealSword()
    {
        _swordObject.SetActive(true);
    }

    /// <summary>
    /// Hides sword when sheathed or when needed
    /// </summary>
    public void HideSword()
    {
        _swordObject.SetActive(false);
    }
}
