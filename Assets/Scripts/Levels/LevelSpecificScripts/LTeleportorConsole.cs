using UnityEngine;

public class LTeleportorConsole : MonoBehaviour
{
    [Header("Teleporters")]
    public LTeleportationRings[] teleportationRings;


    [Header("Console Requirements")]
    public int expectedKillCount;
    public int expectedKeyCount;

    private UITeleportation teleportationUI;

    // Start is called before the first frame update
    void Start()
    {
        teleportationUI = GameObject.FindObjectOfType<UITeleportation>();
    }

    private void ActivateTeleporters()
    {
        foreach (LTeleportationRings rings in teleportationRings)
        {
            rings.gameObject.SetActive(true);
            rings.canBeUsed = true;
        }
    }

    private void CheckValidActivation(int givenKeys, int totalKills)
    {
        if (givenKeys >= expectedKeyCount && totalKills >= expectedKillCount)
        {
            ActivateTeleporters();
            teleportationUI.ConsoleSuccessful();
        }
        else
        {
            teleportationUI.ConsoleUnsucessful();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            ISecretValidator entityValidator = other.GetComponent<ISecretValidator>();
            CheckValidActivation(entityValidator.GetKeyCount(), entityValidator.GetKillCount());
        }
    }
}
