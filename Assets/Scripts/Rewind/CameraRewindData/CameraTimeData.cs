using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTimeData
{
    public bool bIsLockedOn;
    public Transform target, player;

    //targeted enemy needs to be added
    public CameraTimeData(bool _bIsLockedOn, Transform _target, Transform _player) 
    {
        bIsLockedOn = _bIsLockedOn;
        target = _target;
        player = _player;
    }
}
