using System;
using Enemies;
using UnityEngine;

public class SwordColliderOverride : MonoBehaviour
{
    public Collider swordCollider;
    private AISystem _aiSystem;
    public Collider glaiveCol;

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

    public void ColOn(int wInt)
    {
        Debug.Log(wInt);
        if (_aiSystem.enemyType != EnemyType.BOSS)
        {
            // Don't turn on the collider if the enemy is dead.
            // As an example, lightattack state might call the event even as the enemy is dying. Even though the death
            // animation is playing AND it calls ColOff, ColOn can be triggered by the light attack animation.
            // Likely an animation exit time issue
            if (!_aiSystem.bIsDead)
                swordCollider.enabled = true;
        }
        else
        {
            if(wInt == 0)
            {
                swordCollider.enabled = true;
            }
            else if(wInt == 1)
            {
                glaiveCol.enabled = true;
            }
        }
    }
    public void ColOff()
    {
        swordCollider.enabled = false;
    }
    public void ColOff(int wInt)
    {
        if (_aiSystem.enemyType != EnemyType.BOSS)
        {
            // Don't turn on the collider if the enemy is dead.
            // As an example, lightattack state might call the event even as the enemy is dying. Even though the death
            // animation is playing AND it calls ColOff, ColOn can be triggered by the light attack animation.
            // Likely an animation exit time issue
             
              swordCollider.enabled = false;
            
        }
        else
        {
            if (wInt == 0)
            {
                swordCollider.enabled = false;
            }
            else if (wInt == 1)
            {
                glaiveCol.enabled = false;
            }
        }
    }
}
