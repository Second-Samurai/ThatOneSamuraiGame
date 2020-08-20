using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
    void OnEntityDamage();
}

public class PDamageController : MonoBehaviour, IDamageable
{
    StatHandler playerStats;

    public void Init(StatHandler playerStats) {
        this.playerStats = playerStats;
    }

    public void OnEntityDamage()
    {
        Debug.Log("Player is Damaged");
    }
}
