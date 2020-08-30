using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordColliderOverride : MonoBehaviour
{
    public Collider _swordCollider;

    public void ColOn()
    {
        _swordCollider.enabled = true;
    }
    public void ColOff()
    {
        _swordCollider.enabled = false;
    }
}
