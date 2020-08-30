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
        this._currentGuard = currentData.maxGuard;
    }

    //--------Health Related Variables--------

    public float maxHealth
    {
        get { return currentData.maxHealth;  } 
    }

    private float _currentHealth = 0;
    public float CurrentHealth 
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = value;

            if (_currentHealth < 0)
            {
                _currentHealth = 0; //Ground value to zero
            }
        }
    }

    //---------Guard Related Variables----------

    public float maxGuard
    {
        get { return currentData.maxGuard;  }
    }

    private float _currentGuard = 0;
    public float CurrentGuard
    {
        get { return _currentGuard; }
        set
        {
            _currentGuard = value;

            if (_currentGuard < 0)
            {
                _currentGuard = 0; //Ground value to zero
            }
        }
    }

    //---------Damage Related Variables---------

    public float baseDamage
    {
        get { return currentData.baseDamage;  }
    }
}
