using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler
{
    public EntityStatData currentData;

    public void Init(EntityStatData currentData) 
    {
        this.currentData = currentData;
        this._currentHealth = currentData.maxHealth;
    }

    private float _currentHealth = 0;
    public float CurrentHealth 
    {
        get {
            return _currentHealth;
        }

        set {
            _currentHealth = value;
        }
    }
}
