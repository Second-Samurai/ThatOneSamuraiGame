using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
    void OnEntityDamage();
}

public class PDamageController : MonoBehaviour, IDamageable
{
    public void Init() {

    }

    public void OnEntityDamage()
    {
        Debug.Log("Player is Damaged");
    }
}
