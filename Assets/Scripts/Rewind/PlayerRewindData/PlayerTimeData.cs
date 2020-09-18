using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeData
{
    public bool lockedOn;
    public bool swordCollider;


    public PlayerTimeData(bool _lockedOn, bool _swordCollider) 
    {
        lockedOn = _lockedOn;
        swordCollider = _swordCollider;
    }
}
