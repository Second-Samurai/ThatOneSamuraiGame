using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ArcherTimeData
{
    public Vector3 lastDirection, shotDirection;
    public CurrentState currentState;
    public float shotTimer;

    public ArcherTimeData(Vector3 _LastDirection, Vector3 _shotDirection, CurrentState _currentState, float _shotTimer) 
    {
        lastDirection = _LastDirection;
        shotDirection = _shotDirection;
        currentState = _currentState;
        shotTimer = _shotTimer;

    }

}
