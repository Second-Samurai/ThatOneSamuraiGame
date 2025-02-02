using UnityEngine;

/// <summary>
/// Mostly empty but is responsible for identifying the position of the hand and
/// location to where swords will spawn.
/// </summary>
public class PSwordHolder : MonoBehaviour
{
    public WSwordEffect swordEffect;

    private Transform playerTransform;

    public void Init(Transform playerTransform)
    {
    }

    public WSwordEffect SetWeaponToHand(GameObject createdSword)
    {
        swordEffect = createdSword.GetComponent<WSwordEffect>();
        return null;
    }

}
