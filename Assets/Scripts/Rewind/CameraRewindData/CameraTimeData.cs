using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTimeData
{
    public bool bIsLockedOn;
    public Transform target;

    //targeted enemy needs to be added
    public CameraTimeData(bool _bIsLockedOn, Transform _target) 
    {
        bIsLockedOn = _bIsLockedOn;
        target = _target;
    }
}
