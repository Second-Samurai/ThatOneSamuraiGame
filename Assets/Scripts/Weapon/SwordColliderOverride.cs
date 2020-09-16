using System;
using Enemies;
using UnityEngine;

public class SwordColliderOverride : MonoBehaviour
{
    public Collider swordCollider;
    private AISystem _aiSystem;

    private void Start()
    {
        _aiSystem = GetComponent<AISystem>();
    }

    public void ColOn()
    {
        // Don't turn on the collider if the enemy is dead.
        // As an example, lightattack state might call the event even as the enemy is dying. Even though the death
        // animation is playing AND it calls ColOff, ColOn can be triggered by the light attack animation.
        // Likely an animation exit time issue
        if (!_aiSystem.bIsDead)
        {
            swordCollider.enabled = true;
        }
    }
    public void ColOff()
    {
        swordCollider.enabled = false;
    }
}
