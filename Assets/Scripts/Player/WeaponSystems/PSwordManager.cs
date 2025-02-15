using UnityEngine;

public interface ISwordManager
{
    void SetWeapon(bool hasWeapon, GameObject swordPrefab);

    bool IsWeaponEquipped();
}

public class PSwordManager : MonoBehaviour, ISwordManager
{
    [HideInInspector] public bool hasAWeapon = false; //Must check this with an actual gameobject
    [HideInInspector] public WSwordEffect swordEffect;

    private PSwordHolder pSwordHolder;
    private GameObject swordObject;
    private PlayerFunctions pFunctions;

    public void Init()
    {
        this.pSwordHolder = this.GetComponentInChildren<PSwordHolder>();
        pFunctions = GetComponent<PlayerFunctions>();
        pSwordHolder.Init(this.transform);

    }

    public void SetWeapon(bool hasWeapon, GameObject swordPrefab)
    {
        this.hasAWeapon = hasWeapon;

        if (swordObject != null)
        {
            Destroy(swordObject);
        }

        swordObject = Instantiate(swordPrefab, pSwordHolder.transform);
        swordEffect = swordObject.GetComponent<WSwordEffect>();
        pFunctions.parryEffects = swordObject.GetComponent<ParryEffects>();
        swordEffect.Init(this.transform);
    }

    /// <summary>
    /// Reveals sword when drawn or when needed
    /// </summary>
    public void RevealSword()
    {
        swordObject.SetActive(true);
    }

    /// <summary>
    /// Hides sword when sheathed or when needed
    /// </summary>
    public void HideSword()
    {
        swordObject.SetActive(false);
    }

    /// <summary>
    /// Calls the sword's Slash creation func triggered by animation event.
    /// </summary>
    public void BeginSwordEffect(float slashAngle)
    {
        swordEffect.CreateSlashEffect(slashAngle);
    }

    public bool IsWeaponEquipped() 
        => this.swordObject != null;
    
}
