﻿public class StatHandler
{
    public EntityStatData currentData;

    public void Init(EntityStatData currentData) 
    {
        this.currentData = currentData;
        this._currentHealth = currentData.maxHealth;
        this._currentGuard = currentData.maxGuard;
        _maxGuard = currentData.maxGuard;
    }

    #region HEALTH RELATED VARIABLES

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

            if (_currentHealth < 0) {
                _currentHealth = 0; //Ground value to zero
            }
            else if (_currentHealth > currentData.maxHealth) {
                _currentHealth = currentData.maxHealth;
            }
        }
    }

    #endregion

    #region GUARD RELATED VARIABLES

    private float _maxGuard;

    public float maxGuard
    {
        get { return _maxGuard;  }
        set { _maxGuard = value; }
    }

    private float _currentGuard = 0;
    public float CurrentGuard
    {
        get { return _currentGuard; }
        set
        {
            _currentGuard = value;

            if (_currentGuard < 0){
                _currentGuard = 0; //Ground value to zero
            }
            else if (_currentGuard > maxGuard){
                _currentGuard = maxGuard;
            }
        }
    }

    #endregion

    #region DAMAGE RELATED VARIABLES

    public float baseDamage
    {
        get { return currentData.baseDamage;  }
    }

    #endregion
}
