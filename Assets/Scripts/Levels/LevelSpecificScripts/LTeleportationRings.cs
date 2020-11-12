using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class LTeleportationRings : MonoBehaviour
{
    [Header("Rings")]
    public Transform outerRing;
    public Transform innerRing;
    public Transform teleportationPoint;

    [HideInInspector]
    public bool canBeUsed = false;
    public bool isActive = false;
    public bool isPlayerDetected = false;

    private LTeleportationRings destinationRing;
    private UITeleportation teleportationUI;

    // Start is called before the first frame update
    void Start()
    {
        //leportationUI = this.GetComponent<UITeleportation>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        OnActive();
    }

    private void OnActive()
    {
        transform.Rotate(0, 15, 0);
    }

    private void AnimateRingLevitation()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canBeUsed) return;
    }
}
